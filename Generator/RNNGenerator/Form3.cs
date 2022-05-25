using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageTransform
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void btnSelect1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    var filePath = openFileDialog.FileName;
                    txtNodedataF.Text = filePath;
                }
            }
        }

        private void btnSelect2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    var filePath = openFileDialog.FileName;
                    txtPredictionsF.Text = filePath;
                }
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            var predLength = 956; //1000-32-2-10, i really don't feel like programming flexible stuff at this moment
            var numNodes = 50;

            var fogNodes = new FogNode[numNodes];
            var nodeDistances = new Dictionary<int, PointF[]>();
            var soswirlyQoS = new string[predLength];

            var nodeData = GetCsvLines(txtNodedataF.Text);
            var prevNode = "";
            var lineNum = -31; //due to the -32, don't feel like fixing this right now since it depends on a parameter in Python. Sigh.
            var nodeNum = -1;
            var nodeLineNum = 0;

            foreach (var line in nodeData)
            {
                if (line[1] != prevNode) {
                    nodeNum++;
                    var coords = line[1].Split(new char[] { ':' });
                    fogNodes[nodeNum] = new FogNode() {
                        Point = new PointF(float.Parse(coords[0]), float.Parse(coords[1])),
                        Memory = 1024,
                        Network = 100,
                        FreeMemory = int.Parse(line[7]),
                        FreeNetwork = int.Parse(line[8]),
                    };
                    nodeLineNum = 0;

                    prevNode = line[1];
                }
                if (lineNum >= 0 && lineNum < predLength)
                {
                    soswirlyQoS[lineNum] = line[9];
                }

                if (nodeLineNum - 32 >= 0 && nodeLineNum < predLength)
                {
                    if (!nodeDistances.ContainsKey(nodeNum))
                        nodeDistances[nodeNum] = new PointF[predLength];

                    nodeDistances[nodeNum][nodeLineNum - 32] = new PointF(float.Parse(line[4]), float.Parse(line[5]));
                }

                nodeLineNum++;
                lineNum++;
            }

            var sosmartyQoS = new double[predLength];
            var predictions = GetCsvLines(txtPredictionsF.Text);
            var currentIdx = -1;
            for (var lineIdx = 0; lineIdx < predictions.Length; lineIdx++)
            {
                var line = predictions[lineIdx];
                var predLine = line.Select(d => double.Parse(d) * Math.Sqrt(169504)).ToArray();
                currentIdx = GetClosestFogNodeSoSmarty(fogNodes, currentIdx, predLine);
                var fnodeDist = nodeDistances[currentIdx][lineIdx];
                var fnode = fogNodes[currentIdx];
                sosmartyQoS[lineIdx] = fnode.MetricDistance(new PointF(fnode.Point.X - fnodeDist.X, fnode.Point.Y - fnodeDist.Y));
            }

            //I doubt I ever wrote such trash code as the last 50 ish lines
            txtSoSmartySteps.Text = string.Join("\n", sosmartyQoS);
            txtSoSwirlySteps.Text = string.Join("\n", soswirlyQoS);
        }

        private string[][] GetCsvLines(string filename)
        {
            var lines = File.ReadAllLines(filename);
            return lines.Select(l => l.Split(';')).ToArray();
        }

        private int GetClosestFogNodeSoSmarty(FogNode[] fogNodes, int currentIdx, double[] nodeDistances)
        {
            //var nodePoint = new PointF(x, y);
            //"assign" to "fog node"
            int closestFree = -1;
            for (int idx = 0; idx < nodeDistances.Length; idx++)
            {
                var fnode = fogNodes[idx];
                if (fnode.FreeMemory >= 100 && fnode.FreeNetwork >= 5)
                {
                    if (closestFree == -1 || nodeDistances[closestFree] > nodeDistances[idx])
                        closestFree = idx;
                }
            }

            if (currentIdx == -1)
            {
                return closestFree;
            }

            var curDist = nodeDistances[currentIdx];
            var closestDist = nodeDistances[closestFree];
            if (curDist > 100 && curDist > closestDist) // && closestDist < 100
            {
                return closestFree;
            }
            return currentIdx;
        }
    }
}
