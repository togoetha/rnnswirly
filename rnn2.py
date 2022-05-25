import csv;
import numpy;
import math;
import tensorflow as tf
import time

from model_profiler import model_profiler

from tensorflow.keras.layers import Dense, Flatten, Conv2D
from tensorflow.keras import Model

ndims = 3

def build_model():
    model = tf.keras.Sequential([
        tf.keras.layers.InputLayer(input_shape=(None, 3)),
        tf.keras.layers.GRU(30),#, stateful=True),
        tf.keras.layers.Dense(30, activation='relu'),
        tf.keras.layers.Dense(30, activation='relu'),
        tf.keras.layers.Dense(10, activation='relu'),
        tf.keras.layers.Dense(1)
    ])

    model.summary()
    use_units = ['GPU IDs', 'KFLOPs', 'KB', 'Kilo', 'KB']
    profile = model_profiler(model, 1, use_units=use_units)
    print(profile)

    model.compile(loss=tf.keras.losses.MeanSquaredError(),
                optimizer=tf.keras.optimizers.Adam(1e-4),
                metrics=['accuracy'])
    return model

def sigmoid(val):
  return math.pow(math.e, val)/(math.pow(math.e, val) + 1)

def scale_neg_sigmoid(val, scale):
  return (1 - sigmoid(6 * val / scale)) * scale

def gm2_dist(x, y, deployed, mem, net):
  return (x * x) + (y * y) + (1 - deployed) * 10000 + 36 * scale_neg_sigmoid(mem, 1024) + 400 * scale_neg_sigmoid(net, 100)

# loads training data per trips of 1000 data points from CSV
def load_trips(file,batches,sequencelength,timesteps,qlookahead):
    # trip|nodecoords|step|prevstep|dx|dy|svcdeployed|memfree|netfree|soswirlyqos
    with open(file, newline='') as csvfile:
        reader = csv.reader(csvfile, delimiter=';')
        #batches = reader.line_num / batchsize
        x_values = numpy.empty([batches,timesteps-2,ndims], dtype=numpy.float32)
        y_values = numpy.empty([batches,timesteps-2,1], dtype=numpy.float32)
        batch_x = numpy.empty([timesteps-2,ndims], dtype=numpy.float32)
        batch_y = numpy.empty([timesteps-2,1], dtype=numpy.float32)
        prevnode = ''
        batchnr = 0

        maxqos = gm2_dist(256,256,1,0,0)
        for row in reader:
            node = row[1]
            if prevnode == '':
                prevnode = node
            if node != prevnode:
                x_values[batchnr] = batch_x
                y_values[batchnr] = batch_y
                batchnr += 1
                prevnode = node
                batch_x = numpy.empty([timesteps-2,ndims], dtype=numpy.float32)
                batch_y = numpy.empty([timesteps-2,1], dtype=numpy.float32)
            stepnr = int(row[3])
            realqos = math.sqrt(gm2_dist(float(row[4]),float(row[5]),int(row[6]),int(row[7]),int(row[8]))/maxqos)
            if stepnr >= 0 and stepnr < timesteps - 2:
                batch_x[stepnr] = [ realqos, float(row[2])/timesteps, float(row[3])/timesteps ]
            if stepnr > 0:
                batch_y[stepnr-1] = [ realqos ]
        
        x_values[batchnr] = batch_x
        y_values[batchnr] = batch_y

        #so we have timesteps, but -2 for the first and last since we need to remove -1 and have no "output" qos for last one
        # - sequencelength because we can't form a complete sequence for the first "sequencelength" items
        # - qlookahead because we can't form a complete lookahead for the last items
        batchsize = timesteps - sequencelength - qlookahead - 2
        state_batches = numpy.empty([batches,batchsize,sequencelength,ndims], dtype=numpy.float32)
        output_batches = numpy.empty([batches,batchsize,1], dtype=numpy.float32)

        for batch in range(x_values.shape[0]):
            for start in range(batchsize):
                sequence = x_values[batch][start:start+sequencelength]
                state_batches[batch][start] = sequence
                outputs = y_values[batch][start+sequencelength-1:start+sequencelength+qlookahead-1]
                time_dep_q = 0
                factor = 0
                for i in range(len(outputs)):
                    time_dep_q += math.pow(0.9,i) * outputs[i][0]
                    factor += math.pow(0.9,i)
                output_batches[batch][start] = [time_dep_q/factor]

        return state_batches,output_batches

def train_model():
    #dist = math.sqrt(gm2_dist(50,0,1,500,50))
    fognodes = 50
    series_length = 32
    timesteps = 1000
    q_lookahead = 10
    q_ish_rnn = build_model()

    file = "pathmetrics.csv"
    #there's actually 6 trips worth' of data in this single file, so it's 6x fognodes
    state_batches,qos_scores = load_trips(file,6*fognodes,series_length,timesteps,q_lookahead)

    flat_state_batches = state_batches.reshape(-1, *state_batches.shape[-2:])
    flat_scores = qos_scores.reshape(-1, *qos_scores.shape[-1:])

    q_ish_rnn.fit(x=flat_state_batches, y=flat_scores, epochs=3)

    validatefile = "busymetrics.csv"
    validate_batches,validate_qos = load_trips(validatefile,fognodes,series_length,timesteps,q_lookahead)

    qosperround = numpy.empty([timesteps-series_length-q_lookahead-2,fognodes], dtype=numpy.float32)
    for node in range(validate_batches.shape[0]):
        outputs = q_ish_rnn(validate_batches[node])
        q_ish_rnn.reset_states();
        for round in range(outputs.shape[0]):
            qosperround[round][node] = outputs[round][0]

    with open("qosoutputs_busy.csv", "a") as o:
        for i in range(qosperround.shape[0]):
            line = ';'.join(str(e) for e in qosperround[i])
            o.write(line + "\n")       

    start_time = time.time()
    for node in range(validate_batches.shape[0]):
        q_ish_rnn(validate_batches[node])
        #q_net.reset_states();
    print("--- %s seconds ---" % (time.time() - start_time))

    converter = tf.lite.TFLiteConverter.from_keras_model(q_ish_rnn)
    tflite_model = converter.convert()

    # Save the TF Lite model as file
    f = open("qoepredict.tflite", "wb")
    f.write(tflite_model)
    f.close()

    interpreter = tf.lite.Interpreter(model_content=tflite_model)
    interpreter.allocate_tensors()

    #start_time = time.time()
    #for node in range(validate_batches.shape[0]):
    #    for i in range(0, validate_batches[node].shape[0], 50):
    #        interpreter.set_tensor(interpreter.get_input_details()[0]["index"], validate_batches[node][i:i+50])
    #        interpreter.invoke()
    #        interpreter.tensor(interpreter.get_output_details()[0]["index"])()[0]
    #print("--- %s seconds ---" % (time.time() - start_time))

train_model()
