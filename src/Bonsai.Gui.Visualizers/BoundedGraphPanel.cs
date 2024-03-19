using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace Bonsai.Gui.Visualizers
{
    internal abstract class BoundedGraphPanel : GraphControl
    {
        double span;
        int capacity;
        int? setCapacity;

        public BoundedGraphPanel()
        {
            IsShowContextMenu = false;
            ZoomEvent += GraphPanel_ZoomEvent;
            MasterPane = new ViewPane(MasterPane);
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

        internal class ViewPane : MasterPane
        {
            public ViewPane(MasterPane masterPane)
                : base(masterPane.Title.Text, masterPane.Rect)
            {
                Margin.All = 0;
                Title.IsVisible = false;
                Add(masterPane.PaneList[0]);
            }

            public float StatusGap { get; set; }

            public override void ReSize(Graphics g, RectangleF rect)
            {
                rect.Height -= StatusGap;
                base.ReSize(g, rect);
            }
        }
    }
}
