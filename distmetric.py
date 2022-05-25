import math
from pickletools import float8
import random
import tensorflow as tf

import time
import numpy

from model_profiler import model_profiler

ndims = 5
train_samples = 200000
validate_samples = 200000
writeresults = False

def sigmoid(val):
  return math.pow(math.e, val)/(math.pow(math.e, val) + 1)

def scale_neg_sigmoid(val, scale):
  return (1 - sigmoid(6 * val / scale)) * scale

def gm1_dist(x, y, deployed, mem, net):
  return (x * x) + (y * y)

def generate_euclid(samples): 
  x_euclid = numpy.empty([samples,ndims], dtype=numpy.float32)
  y_euclid = numpy.empty(samples, dtype=numpy.float32)
  euclid_maxdist = 256 * math.sqrt(2)
  for i in range(samples):
    xrnd = random.randint(0, 255)
    yrnd = random.randint(0, 255)
    memrnd = random.randint(0, 4096)
    netrnd = random.randint(0, 100)
    deployedrnd = random.randint(0, 1)
    distsqr = gm1_dist(xrnd, yrnd, deployedrnd, memrnd, netrnd) 
    x_euclid[i] = [xrnd / 256, yrnd / 256, deployedrnd, memrnd / 1024, netrnd / 100]#, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    y_euclid[i] = distsqr / (euclid_maxdist * euclid_maxdist)
  return x_euclid, y_euclid

def gm2_dist(x, y, deployed, mem, net):
  return (x * x) + (y * y) + deployed * 10000 + 49 * scale_neg_sigmoid(mem, 1024) + 900 * scale_neg_sigmoid(net, 100)

def generate_resourced(samples): 
  x_resourced = numpy.empty([samples,ndims], dtype=numpy.float32)
  y_resourced = numpy.empty(samples, dtype=numpy.float32)
  resourced_maxdist = 459
  for i in range(samples):
    xrnd = random.randint(0, 255)
    yrnd = random.randint(0, 255)
    memrnd = random.randint(0, 1024)
    netrnd = random.randint(0, 100)
    deployedrnd = random.randint(0, 1)
    distsqr = gm2_dist(xrnd, yrnd, deployedrnd, memrnd, netrnd) #(xrnd * xrnd) + (yrnd * yrnd) + deployedrnd * 10000 + 49 * scale_neg_sigmoid(memrnd, 1024) + 900 * scale_neg_sigmoid(netrnd, 100)
    x_resourced[i] = [xrnd / 256, yrnd / 256, deployedrnd, memrnd / 1024, netrnd / 100]#, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    y_resourced[i] = distsqr / (resourced_maxdist * resourced_maxdist)
  return x_resourced, y_resourced

def experiment_run(trainx, trainy, validatex, validatey, maxdist, file):
  model.fit(trainx, trainy, epochs=5)
  start_time = time.time()
  output = model(validatex)#.evaluate(validatex, validatey, verbose=2)
  print("--- %s seconds ---" % (time.time() - start_time))

  if (writeresults):
    ootput = model(validatex)
    pct_diffs = numpy.empty(validate_samples, dtype=float)
    abs_diffs = numpy.empty(validate_samples, dtype=float)
    with open(file, "a") as o:
      for i in range(validate_samples):
        predist = math.sqrt(max(0, ootput[i][0]) * maxdist)
        realdist = math.sqrt(validatey[i] * maxdist)
        pct_diffs[i] = 100 * abs(predist - realdist) / (realdist + 0.000000000001)
        abs_diffs[i] = abs(predist - realdist)
        o.write(str(realdist) + "," + str(predist) + "\n")

    print("Avg acc " + str(100-numpy.average(pct_diffs)) + "%")
    print("Stdev acc " + str(numpy.std(pct_diffs)) + "%")
    print("Avg abs err " + str(numpy.average(abs_diffs)) + "%")
    print("Stdev acc " + str(numpy.std(abs_diffs)) + "%")

  converter = tf.lite.TFLiteConverter.from_keras_model(model)
  tflite_model = converter.convert()

  # Save the TF Lite model as file
  f = open("distmetric.tflite", "wb")
  f.write(tflite_model)
  f.close()

  interpreter = tf.lite.Interpreter(model_content=tflite_model)
  interpreter.allocate_tensors()

  start_time = time.time()
  for i in range(validate_samples):
    interpreter.set_tensor(interpreter.get_input_details()[0]["index"], [validatex[i]])
    interpreter.invoke()
    interpreter.tensor(interpreter.get_output_details()[0]["index"])()[0]
  print("--- %s seconds ---" % (time.time() - start_time))


model = tf.keras.models.Sequential([
  tf.keras.layers.InputLayer(input_shape=(ndims)),
  tf.keras.layers.Dense(ndims, activation='tanh'),
  tf.keras.layers.Dense(ndims, activation='tanh'),
  tf.keras.layers.Dense(ndims, activation='tanh'),
  tf.keras.layers.Dense(1)
])

loss_fn = tf.keras.losses.MeanSquaredError()

model.summary()

use_units = ['GPU IDs', 'KFLOPs', 'KB', 'Kilo', 'KB']
profile = model_profiler(model, 1, use_units=use_units)

print(profile)

model.compile(optimizer='adam',
              loss=loss_fn,
              metrics=['accuracy'])


print("EUCLID")
trainx, trainy = generate_euclid(train_samples) 
validatex, validatey = generate_euclid(validate_samples) 
maxdist = gm1_dist(255,255,1,0,0)
experiment_run(trainx, trainy, validatex, validatey, maxdist, "euclid.csv")

print("RESOURCED")
trainx, trainy = generate_resourced(train_samples) 
validatex, validatey = generate_resourced(validate_samples)
maxdist = gm2_dist(255,255,1,0,0)
experiment_run(trainx, trainy, validatex, validatey, maxdist, "resourced.csv")


