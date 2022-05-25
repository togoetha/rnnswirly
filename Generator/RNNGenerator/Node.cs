using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ImageTransform
{
    public class WayPoint
    {
        public int ID { get; set; }
        public PointF Point { get; set; }
        public int[] Links { get; set; }
    }

    public class FogNode
    {
        public PointF Point { get; set; }
        public int Memory { get; set; }
        public int FreeMemory { get; set; }
        public int Network { get; set; }
        public int FreeNetwork { get; set; }

        public double Distance(PointF p)
        {
            return Math.Sqrt((p.X - Point.X) * (p.X - Point.X) + (p.Y - Point.Y) * (p.Y - Point.Y));
        }

        public double MetricDistance(PointF p)
        {
            var deployed = Memory == 768 ? 1 : 0;
            var dist = Math.Sqrt((p.X - Point.X) * (p.X - Point.X) + (p.Y - Point.Y) * (p.Y - Point.Y) + deployed * 10000 + 36 * ScaleNegSigmoid(FreeMemory, 1024) + 400 * ScaleNegSigmoid(FreeNetwork, 100));
            return dist;
        }

        private double ScaleNegSigmoid(double val, double scale)
        {
            var value = (1 - Sigmoid(6 * val / scale)) * scale;
            return value;
        }

        private double Sigmoid(double val)
        {
            return Math.Pow(Math.E, val) / (Math.Pow(Math.E, val) + 1);
        }
    }

    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Dictionary<Node, int> Pings { get; set; }
        public List<Node> SortedPings { get; set; }
        public bool Active { get; set; }

        public Node()
        {
            Pings = new Dictionary<Node, int>();
        }

        public double Distance(Node node)
        {
            return Math.Sqrt(Math.Pow(node.X - X, 2) + Math.Pow(node.Y - Y, 2));
        }

        public override string ToString()
        {
            return "X " + X + " Y" + Y;
        }
    }

    public class PingNode
    {
        public Node Node { get; set; }
        public int Ping { get; set; }
    }
}
