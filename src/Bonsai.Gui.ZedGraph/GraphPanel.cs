using ZedGraph;

namespace Bonsai.Gui.ZedGraph
{
    internal class GraphPanel : BoundedGraphPanel
    {
        bool autoScaleX;
        bool autoScaleY;

        public GraphPanel()
        {
            autoScaleX = true;
            autoScaleY = true;
        }

        public double XMin
        {
            get { return GraphPane.XAxis.Scale.Min; }
            set
            {
                GraphPane.XAxis.Scale.Min = value;
                GraphPane.AxisChange();
                Invalidate();
            }
        }

        public double XMax
        {
            get { return GraphPane.XAxis.Scale.Max; }
            set
            {
                GraphPane.XAxis.Scale.Max = value;
                GraphPane.AxisChange();
                Invalidate();
            }
        }

        public double YMin
        {
            get { return GraphPane.YAxis.Scale.Min; }
            set
            {
                GraphPane.YAxis.Scale.Min = value;
                GraphPane.AxisChange();
                Invalidate();
            }
        }

        public double YMax
        {
            get { return GraphPane.YAxis.Scale.Max; }
            set
            {
                GraphPane.YAxis.Scale.Max = value;
                GraphPane.AxisChange();
                Invalidate();
            }
        }

        public bool AutoScaleX
        {
            get { return autoScaleX; }
            set
            {
                var changed = autoScaleX != value;
                autoScaleX = value;
                if (changed)
                {
                    GraphPane.XAxis.Scale.MaxAuto = autoScaleX;
                    GraphPane.XAxis.Scale.MinAuto = autoScaleX;
                    if (autoScaleX) Invalidate();
                }
            }
        }

        public bool AutoScaleY
        {
            get { return autoScaleY; }
            set
            {
                var changed = autoScaleY != value;
                autoScaleY = value;
                if (changed)
                {
                    GraphPane.YAxis.Scale.MaxAuto = autoScaleY;
                    GraphPane.YAxis.Scale.MinAuto = autoScaleY;
                    if (autoScaleY) Invalidate();
                }
            }
        }
    }
}
