using System;
using System.Windows.Forms;
using ZedGraph;

namespace Bonsai.Gui.Visualizers
{
    internal class GraphPanel : GraphControl
    {
        double span;
        int capacity;
        bool autoScaleX;
        bool autoScaleY;
        int? setCapacity;

        public GraphPanel()
        {
            autoScaleX = true;
            autoScaleY = true;
            IsShowContextMenu = false;
            ZoomEvent += GraphPanel_ZoomEvent;
        }

        public double Span
        {
            get { return span; }
            set
            {
                span = value;
                Invalidate();
            }
        }

        public int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
                setCapacity = capacity;
                Invalidate();
            }
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

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            double? maxValue = null;
            var curveList = GraphPane.CurveList;
            for (int i = 0; i < curveList.Count; i++)
            {
                if (curveList[i].Points is BoundedPointPairList boundedList)
                {
                    if (span <= 0)
                    {
                        boundedList.SetBounds(double.MinValue, double.MaxValue);
                    }
                    else if (boundedList.Count > 0)
                    {
                        maxValue = Math.Max(
                            maxValue.GetValueOrDefault(double.MinValue),
                            boundedList[boundedList.Count - 1].Z);
                    }
                }
            }

            if (maxValue != null)
            {
                var lowerBound = maxValue.GetValueOrDefault() - span;
                for (int i = 0; i < curveList.Count; i++)
                {
                    if (curveList[i].Points is BoundedPointPairList boundedList)
                    {
                        boundedList.SetBounds(lowerBound, double.MaxValue);
                    }
                }
            }

            if (setCapacity != null)
            {
                for (int i = 0; i < curveList.Count; i++)
                {
                    if (curveList[i].Points is BoundedPointPairList boundedList)
                    {
                        boundedList.SetCapacity(capacity);
                    }
                }

                setCapacity = null;
            }

            base.OnInvalidated(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.P)
            {
                DoPrint();
            }

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                SaveAs();
            }

            if (e.KeyCode == Keys.Back)
            {
                ZoomOut(GraphPane);
            }

            base.OnKeyDown(e);
        }

        private void GraphPanel_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            MasterPane.AxisChange();
        }
    }
}
