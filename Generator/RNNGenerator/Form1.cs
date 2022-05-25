using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ImageTransform
{
    public partial class Form1 : Form
    {
        private List<FogNode> FogNodes { get; set; }
        private List<PointF> EdgeNodes { get; set; }
        private Dictionary<int, WayPoint> WayPoints { get; set; }
        private List<int[]> Paths { get; set; }

        private PointF PlayerPosition { get; set; }
        private FogNode PlayerFogNode { get; set; }

        Dictionary<int, Dictionary<FogNode, double[][]>> MetricHistory;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerateNodes_Click(object sender, EventArgs e)
        {
            var rd = new Random();

            FogNodes = new List<FogNode>();

            int fogNodes = 0;
            var ok = int.TryParse(txtFogNodes.Text, out fogNodes);
            if (ok)
            {
                for (int node = 0; node < fogNodes; node++)
                {
                    var nodePoint = new FogNode() { Point = new Point(rd.Next(256), rd.Next(256)), Memory = 1024, FreeMemory = 768, Network = 100, FreeNetwork = 100};
                    FogNodes.Add(nodePoint);
                }
            }

            EdgeNodes = new List<PointF>();

            int edgeNodes = 0;
            ok = int.TryParse(txtEdgeNodes.Text, out edgeNodes);
            if (ok)
            {
                for (int node = 0; node < edgeNodes; node++)
                {
                    var nodePoint = new Point(rd.Next(256), rd.Next(256));
                    EdgeNodes.Add(nodePoint);
                    //"assign" to "fog node"
                    FogNode closestFree = null;
                    foreach (var fnode in FogNodes)
                    {
                        if (fnode.FreeMemory >= 100 && fnode.FreeNetwork >= 5)
                        {
                            if (closestFree == null || closestFree.MetricDistance(nodePoint) > fnode.MetricDistance(nodePoint))
                                closestFree = fnode;
                        }
                    }
                    closestFree.FreeMemory -= 35;
                    closestFree.FreeNetwork -= 5;
                }
            } 
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            ParsePaths();
            UpdateDraw(true);
        }

        private void ParsePaths()
        {
            WayPoints = new Dictionary<int, WayPoint>();
            var waypoints = txtWaypoints.Text.Split(new char[] { '\n' });
            foreach (var wp in waypoints)
            {
                var parts = wp.Split(new char[] { ';' });
                //parts = parts.Take(parts.Length - 1).ToArray();
                var waypoint = new WayPoint() { 
                    ID = int.Parse(parts[0]), 
                    Point = new Point(int.Parse(parts[1]), int.Parse(parts[2])), 
                    Links = parts.Skip(3).Select(p => int.Parse(p)).ToArray() 
                };
                WayPoints.Add(waypoint.ID, waypoint);
            }

            Paths = new List<int[]>();
            var paths = txtPaths.Text.Split(new char[] { '\n' });
            foreach (var path in paths)
            {
                var pathpoints = path.Split(new char[] { ';' }).Select(p => int.Parse(p)).ToArray();
                Paths.Add(pathpoints);
            }
            if (Paths.Count > 0)
            {
                PlayerPosition = WayPoints[Paths[0][0]].Point;
            }
        }

        private void UpdateDraw(bool edgeNodes)
        {
            var time = DateTime.Now;

            var g = pnlTopo.CreateGraphics();
            g.Clear(Color.White);

            var brshRed = new SolidBrush(Color.Red);
            var brshGreen = new SolidBrush(Color.LightGreen);
            var brshBlue = new SolidBrush(Color.Blue);

            Random rd = new Random();

            if (edgeNodes)
            {
                foreach (var edgeNode in EdgeNodes)
                {
                    g.FillEllipse(brshBlue, new Rectangle(((int)edgeNode.X - 1) * 2, ((int)edgeNode.Y - 1) * 2, 4, 4));
                }
            }
            foreach (var fogNode in FogNodes)
            {
                g.FillEllipse(brshRed, new Rectangle(((int)fogNode.Point.X - 1)*2, ((int)fogNode.Point.Y - 1)*2, 6, 6));
            }

            var brshBlack = new SolidBrush(Color.Black);
            var yellowLinePen = new Pen(Color.Yellow, 2.5f);
            var blueLinePen = new Pen(Color.Blue, 2.5f);
            //WayPoint prevwp = null;
            foreach (var wp in WayPoints)
            {
                foreach (var otherwp in wp.Value.Links)
                {
                    var prevwp = WayPoints[otherwp];
                    var scaledPoint1 = new Point((int)(prevwp.Point.X * 2), (int)(prevwp.Point.Y * 2));
                    var scaledPoint2 = new Point((int)(wp.Value.Point.X * 2), (int)(wp.Value.Point.Y * 2));
                    g.DrawLine(yellowLinePen, scaledPoint1, scaledPoint2);
                }
                g.FillEllipse(brshBlack, new Rectangle((int)(wp.Value.Point.X - 2) * 2, (int)(wp.Value.Point.Y - 2) * 2, 8, 8));
                var point = new Point((int)(wp.Value.Point.X - 5) * 2, (int)(wp.Value.Point.Y - 8) * 2);
                var fontFamily = new FontFamily("Arial");
                var font = new Font(
                   fontFamily,
                   10,
                   FontStyle.Regular,
                   GraphicsUnit.Pixel);
                g.DrawString(wp.Value.ID.ToString(), font, brshBlack, point);
        
            }

            var linePen = new Pen(Color.Green, 2.5f);
            foreach (var path in Paths)
            {
                int prev = -1;
                foreach (var wp in path)
                {
                    if (prev != -1)
                    {
                        var scaledPoint1 = new Point((int)WayPoints[prev].Point.X * 2, (int)WayPoints[prev].Point.Y * 2);
                        var scaledPoint2 = new Point((int)WayPoints[wp].Point.X * 2, (int)WayPoints[wp].Point.Y * 2);
                        g.DrawLine(linePen, scaledPoint1, scaledPoint2);
                    }
                    prev = wp;
                }
            }

            if (PlayerPosition != null)
            {
                var brshPurple = new SolidBrush(Color.Purple);
                g.FillEllipse(brshPurple, new Rectangle((int)(PlayerPosition.X - 2) * 2, (int)(PlayerPosition.Y - 2) * 2, 8, 8));
                brshPurple.Dispose();
            }
            if (PlayerFogNode != null)
            {
                g.DrawEllipse(blueLinePen, new Rectangle((int)(PlayerFogNode.Point.X - 3) * 2, (int)(PlayerFogNode.Point.Y - 3) * 2, 12, 12));
            }

            brshBlack.Dispose();
            brshBlue.Dispose();
            brshRed.Dispose();
            brshGreen.Dispose();
            linePen.Dispose();
            yellowLinePen.Dispose();

            g.Dispose();
            Console.WriteLine("Redraw took " + (DateTime.Now - time).TotalMilliseconds + "ms");
        }



        private void btnSim_Click(object sender, EventArgs e)
        {
            MetricHistory = new Dictionary<int, Dictionary<FogNode, double[][]>>();
            for (int i = 0; i < Paths.Count; i++)
            {
                var path = Paths[i];
                MetricHistory.Add(i, new Dictionary<FogNode, double[][]>());

                PlayerFogNode = null;
                int timesteps = 0;
                var ok = int.TryParse(txtTimesteps.Text, out timesteps);

                //var path = Paths[0];

                PointF prevPoint = WayPoints[path[0]].Point;
                double totalDist = 0;
                for (int wp = 1; wp < path.Length; wp++)
                {
                    var curPoint = WayPoints[path[wp]].Point;
                    totalDist += Math.Sqrt((curPoint.X - prevPoint.X) * (curPoint.X - prevPoint.X) + (curPoint.Y - prevPoint.Y) * (curPoint.Y - prevPoint.Y));
                    prevPoint = curPoint;
                }

                var distPerStep = totalDist / timesteps;

                //int prevWaypointIdx = 0;
                var prevWaypoint = WayPoints[path[0]].Point;
                int curWaypointIdx = 1;
                var curWaypoint = WayPoints[path[1]].Point;
                var currentSegDist = Math.Sqrt((curWaypoint.X - prevWaypoint.X) * (curWaypoint.X - prevWaypoint.X) + (curWaypoint.Y - prevWaypoint.Y) * (curWaypoint.Y - prevWaypoint.Y));

                int currentSegSteps = (int)(currentSegDist / distPerStep);
                int prevEndStep = 0;
                int currentSegEndstep = currentSegSteps;

                float x = 0;
                float y = 0;
                for (int step = 0; step < timesteps; step++)
                {
                    //update position
                    x = prevWaypoint.X + ((float)(curWaypoint.X - prevWaypoint.X) / currentSegSteps) * (step - prevEndStep);
                    y = prevWaypoint.Y + ((float)(curWaypoint.Y - prevWaypoint.Y) / currentSegSteps) * (step - prevEndStep);
                    PlayerPosition = new Point((int)x, (int)y);
                    //check segment end
                    if (step == currentSegEndstep && step < timesteps - 1)
                    {
                        //next segment
                        prevWaypoint = curWaypoint;
                        curWaypointIdx++;
                        if (curWaypointIdx < path.Length)
                        {
                            curWaypoint = WayPoints[path[curWaypointIdx]].Point;

                            currentSegDist = Math.Sqrt((curWaypoint.X - prevWaypoint.X) * (curWaypoint.X - prevWaypoint.X) + (curWaypoint.Y - prevWaypoint.Y) * (curWaypoint.Y - prevWaypoint.Y));
                            currentSegSteps = (int)(currentSegDist / distPerStep);
                            prevEndStep = currentSegEndstep;
                            currentSegEndstep += currentSegSteps;
                        }
                    }

                    PlayerFogNode = GetClosestFogNodeSoSwirly(PlayerFogNode, x, y);

                    lblTimestep.Text = step + "/" + timesteps;
                    if (step % 20 == 0)
                    {
                        UpdateDraw(false);
                        Thread.Sleep(50);
                    }

                    AddNodeMetrics(i, timesteps, step, MetricHistory);
                }
            }
        }

        private void AddNodeMetrics(int path, int totalSteps, int step, Dictionary<int, Dictionary<FogNode, double[][]>> metricHistory)
        {
            foreach (var fnode in FogNodes)
            {
                if (step == 0)
                {
                    metricHistory[path][fnode] = new double[totalSteps][];
                }
                var qos = 0d;
                if (PlayerFogNode != null) 
                    qos = PlayerFogNode.MetricDistance(PlayerPosition);

                //x,y,servicedeployed,mem,network,soswirlyqos
                metricHistory[path][fnode][step] = new double[] {
                    Math.Abs(PlayerPosition.X - fnode.Point.X),
                    Math.Abs(PlayerPosition.Y - fnode.Point.Y),
                    fnode.FreeMemory < 768 ? 1 : 0,
                    fnode.FreeMemory,
                    fnode.FreeNetwork,
                    qos
                };
            }
        }

        private FogNode GetClosestFogNodeSoSwirly(FogNode current, float x, float y)
        {
            var nodePoint = new PointF(x, y);
            //"assign" to "fog node"
            FogNode closestFree = null;
            foreach (var fnode in FogNodes)
            {
                if (fnode.FreeMemory >= 100 && fnode.FreeNetwork >= 5)
                {
                    if (closestFree == null || closestFree.MetricDistance(nodePoint) > fnode.MetricDistance(nodePoint))
                        closestFree = fnode;
                }
            }

            if (current == null)
            {
                return closestFree;
            }

            var curDist = current.MetricDistance(nodePoint);
            var closestDist = closestFree.MetricDistance(nodePoint);
            if (curDist > 100 && closestDist < 100 && curDist > closestDist)
            {
                return closestFree;
            }
            return current;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var lines = new List<string>();

            foreach (var run in MetricHistory)
            {
                foreach (var fnode in run.Value)
                {
                    var stepIdx = 0;
                    foreach (var step in fnode.Value)
                    {
                        var metrics = string.Join(";", step);
                        //metrics = metrics.Substring(0, metrics.Length - 1);
                        //run,node,x,y,servicedeployed,mem,network,soswirlyqos
                        lines.Add(run.Key + ";" + fnode.Key.Point.X + ":" + fnode.Key.Point.Y + ";" + stepIdx + ";" + (stepIdx-1) + ";" + metrics);

                        stepIdx++;
                    }
                }
            }

            File.WriteAllLines("pathmetrics.csv", lines.ToArray());
        }
    }
}
