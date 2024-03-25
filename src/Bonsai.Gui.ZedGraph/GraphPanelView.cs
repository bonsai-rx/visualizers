using System;
using System.Windows.Forms;
using ZedGraph;
using System.Globalization;

namespace Bonsai.Gui.ZedGraph
{
    partial class GraphPanelView : GraphPanel
    {
        readonly ToolStripEditableLabel minEditableLabelX;
        readonly ToolStripEditableLabel maxEditableLabelX;
        readonly ToolStripEditableLabel minEditableLabelY;
        readonly ToolStripEditableLabel maxEditableLabelY;
        readonly ToolStripEditableLabel capacityEditableLabel;
        readonly ToolStripEditableLabel spanEditableLabel;

        public GraphPanelView()
        {
            InitializeComponent();
            autoScaleButtonX.Checked = true;
            autoScaleButtonY.Checked = true;
            spanEditableLabel = new ToolStripEditableLabel(spanValueLabel, OnSpanEdit);
            capacityEditableLabel = new ToolStripEditableLabel(capacityValueLabel, OnCapacityEdit);
            minEditableLabelX = new ToolStripEditableLabel(minStatusLabelX, OnXMinEdit);
            maxEditableLabelX = new ToolStripEditableLabel(maxStatusLabelX, OnXMaxEdit);
            minEditableLabelY = new ToolStripEditableLabel(minStatusLabelY, OnYMinEdit);
            maxEditableLabelY = new ToolStripEditableLabel(maxStatusLabelY, OnYMaxEdit);
            GraphPane.AxisChangeEvent += GraphPane_AxisChangeEvent;
            MouseMoveEvent += GraphPanelView_MouseMoveEvent;
            MouseClick += GraphPanelView_MouseClick;
            components.Add(spanEditableLabel);
            components.Add(capacityEditableLabel);
            components.Add(minEditableLabelX);
            components.Add(maxEditableLabelX);
            components.Add(minEditableLabelY);
            components.Add(maxEditableLabelY);
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

        public bool AutoScaleXVisible
        {
            get { return autoScaleButtonX.Visible; }
            set
            {
                autoScaleButtonX.Visible = value;
                minEditableLabelX.Enabled = value;
                maxEditableLabelX.Enabled = value;
            }
        }

        public bool AutoScaleYVisible
        {
            get { return autoScaleButtonY.Visible; }
            set
            {
                autoScaleButtonY.Visible = value;
                minEditableLabelY.Enabled = value;
                maxEditableLabelY.Enabled = value;
            }
        }

        public bool IsTimeSpan { get; set; }

        public event EventHandler AutoScaleXChanged
        {
            add { autoScaleButtonX.CheckedChanged += value; }
            remove { autoScaleButtonX.CheckedChanged -= value; }
        }

        public event EventHandler AutoScaleYChanged
        {
            add { autoScaleButtonY.CheckedChanged += value; }
            remove { autoScaleButtonY.CheckedChanged -= value; }
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
            var scaleX = pane.XAxis.Scale;
            var scaleY = pane.YAxis.Scale;
            autoScaleButtonX.Checked = pane.XAxis.Scale.MaxAuto;
            autoScaleButtonY.Checked = pane.YAxis.Scale.MaxAuto;
            spanValueLabel.Text = IsTimeSpan
                ? TimeSpan.FromDays(span).ToString()
                : span.ToString("G5", CultureInfo.InvariantCulture);
            capacityValueLabel.Text = capacity.ToString(CultureInfo.InvariantCulture);
            minStatusLabelX.Text = scaleX.Min.ToString("G5", CultureInfo.InvariantCulture);
            maxStatusLabelX.Text = scaleX.Max.ToString("G5", CultureInfo.InvariantCulture);
            minStatusLabelY.Text = scaleY.Min.ToString("G5", CultureInfo.InvariantCulture);
            maxStatusLabelY.Text = scaleY.Max.ToString("G5", CultureInfo.InvariantCulture);
            OnAxisChanged(EventArgs.Empty);
        }

        private void autoScaleButtonX_CheckedChanged(object sender, EventArgs e)
        {
            AutoScaleX = autoScaleButtonX.Checked;
            minStatusLabelX.Visible = !autoScaleButtonX.Checked;
            maxStatusLabelX.Visible = !autoScaleButtonX.Checked;
        }

        private void autoScaleButtonY_CheckedChanged(object sender, EventArgs e)
        {
            AutoScaleY = autoScaleButtonY.Checked;
            minStatusLabelY.Visible = !autoScaleButtonY.Checked;
            maxStatusLabelY.Visible = !autoScaleButtonY.Checked;
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

        private void OnXMinEdit(string text)
        {
            if (double.TryParse(text, out double min))
            {
                XMin = min;
            }
        }

        private void OnXMaxEdit(string text)
        {
            if (double.TryParse(text, out double max))
            {
                XMax = max;
            }
        }

        private void OnYMinEdit(string text)
        {
            if (double.TryParse(text, out double min))
            {
                YMin = min;
            }
        }

        private void OnYMaxEdit(string text)
        {
            if (double.TryParse(text, out double max))
            {
                YMax = max;
            }
        }
    }
}
