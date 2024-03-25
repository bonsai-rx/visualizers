using System;
using System.Windows.Forms;
using ZedGraph;
using System.Globalization;

namespace Bonsai.Gui.Visualizers
{
    partial class RollingGraphPanelView : RollingGraphPanel
    {
        readonly ToolStripEditableLabel minEditableLabel;
        readonly ToolStripEditableLabel maxEditableLabel;
        readonly ToolStripEditableLabel capacityEditableLabel;
        readonly ToolStripEditableLabel spanEditableLabel;

        public RollingGraphPanelView()
        {
            InitializeComponent();
            autoScaleButton.Checked = true;
            spanEditableLabel = new ToolStripEditableLabel(spanValueLabel, OnSpanEdit);
            capacityEditableLabel = new ToolStripEditableLabel(capacityValueLabel, OnCapacityEdit);
            minEditableLabel = new ToolStripEditableLabel(minStatusLabel, OnMinEdit);
            maxEditableLabel = new ToolStripEditableLabel(maxStatusLabel, OnMaxEdit);
            GraphPane.AxisChangeEvent += GraphPane_AxisChangeEvent;
            MouseMoveEvent += GraphPanelView_MouseMoveEvent;
            MouseClick += GraphPanelView_MouseClick;
            components.Add(spanEditableLabel);
            components.Add(capacityEditableLabel);
            components.Add(minEditableLabel);
            components.Add(maxEditableLabel);
        }

        protected StatusStrip StatusStrip
        {
            get { return statusStrip; }
        }

        public bool CanEditSpan
        {
            get { return spanEditableLabel.Enabled; }
            set { spanEditableLabel.Enabled = value; }
        }

        public bool CanEditCapacity
        {
            get { return capacityEditableLabel.Enabled; }
            set { capacityEditableLabel.Enabled = value; }
        }

        public bool AutoScaleVisible
        {
            get { return autoScaleButton.Visible; }
            set
            {
                autoScaleButton.Visible = value;
                minEditableLabel.Enabled = value;
                maxEditableLabel.Enabled = value;
            }
        }

        private bool IsTimeSpan
        {
            get
            {
                var baseAxis = GraphPane.BarSettings.BarBaseAxis();
                return baseAxis.Type == AxisType.Date || baseAxis.Type == AxisType.DateAsOrdinal;
            }
        }

        public event EventHandler AutoScaleChanged
        {
            add { autoScaleButton.CheckedChanged += value; }
            remove { autoScaleButton.CheckedChanged -= value; }
        }

        public event EventHandler AxisChanged;

        protected virtual void OnAxisChanged(EventArgs e)
        {
            AxisChanged?.Invoke(this, e);
        }

        private bool GraphPanelView_MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            var pane = MasterPane.FindChartRect(e.Location);
            if (pane != null)
            {
                pane.ReverseTransform(e.Location, out double x, out double y);
                cursorStatusLabel.Text = string.Format("Cursor: ({0:G5}, {1:G5})", x, y);
            }
            return false;
        }

        private void GraphPanelView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                statusStrip.Visible = !statusStrip.Visible;
                ((ViewPane)MasterPane).StatusGap = statusStrip.Visible ? statusStrip.Height : 0;
                ZedGraphControl_ReSize(this, EventArgs.Empty);
            }
        }

        private void GraphPane_AxisChangeEvent(GraphPane pane)
        {
            var span = Span;
            var capacity = Capacity;
            var scale = ScaleAxis.Scale;
            autoScaleButton.Checked = scale.MaxAuto;
            spanValueLabel.Text = IsTimeSpan
                ? TimeSpan.FromDays(span).ToString()
                : span.ToString("G5", CultureInfo.InvariantCulture);
            capacityValueLabel.Text = capacity.ToString(CultureInfo.InvariantCulture);
            minStatusLabel.Text = scale.Min.ToString("G5", CultureInfo.InvariantCulture);
            maxStatusLabel.Text = scale.Max.ToString("G5", CultureInfo.InvariantCulture);
            OnAxisChanged(EventArgs.Empty);
        }

        private void autoScaleButton_CheckedChanged(object sender, EventArgs e)
        {
            AutoScale = autoScaleButton.Checked;
            minStatusLabel.Visible = !autoScaleButton.Checked;
            maxStatusLabel.Visible = !autoScaleButton.Checked;
        }

        private void OnSpanEdit(string text)
        {
            if (IsTimeSpan)
            {
                if (TimeSpan.TryParse(text, out TimeSpan timeSpan))
                {
                    Span = timeSpan.TotalDays;
                }
            }
            else if (double.TryParse(text, out double span))
            {
                Span = span;
            }
        }

        private void OnCapacityEdit(string text)
        {
            if (int.TryParse(text, out int capacity))
            {
                Capacity = capacity;
            }
        }

        private void OnMinEdit(string text)
        {
            if (double.TryParse(text, out double min))
            {
                Min = min;
            }
        }

        private void OnMaxEdit(string text)
        {
            if (double.TryParse(text, out double max))
            {
                Max = max;
            }
        }
    }
}
