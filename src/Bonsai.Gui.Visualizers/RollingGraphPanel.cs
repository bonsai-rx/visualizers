using ZedGraph;

namespace Bonsai.Gui.Visualizers
{
    internal class RollingGraphPanel : BoundedGraphPanel
    {
        bool autoScale;

        public RollingGraphPanel()
        {
            autoScale = true;
        }

        public Axis ScaleAxis => GraphPane.BarSettings.Base switch
        {
            BarBase.Y => GraphPane.XAxis,
            BarBase.Y2 => GraphPane.X2Axis,
            BarBase.X2 => GraphPane.Y2Axis,
            _ => GraphPane.YAxis
        };

        public double Min
        {
            get { return ScaleAxis.Scale.Min; }
            set
            {
                ScaleAxis.Scale.Min = value;
                GraphPane.AxisChange();
                Invalidate();
            }
        }

        public double Max
        {
            get { return ScaleAxis.Scale.Max; }
            set
            {
                ScaleAxis.Scale.Max = value;
                GraphPane.AxisChange();
                Invalidate();
            }
        }

        public bool AutoScale
        {
            get { return autoScale; }
            set
            {
                var changed = autoScale != value;
                autoScale = value;
                if (changed)
                {
                    var baseAxis = ScaleAxis;
                    baseAxis.Scale.MaxAuto = autoScale;
                    baseAxis.Scale.MinAuto = autoScale;
                    if (autoScale) Invalidate();
                }
            }
        }
    }
}
