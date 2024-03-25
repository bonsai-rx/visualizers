using System;
using System.Windows.Forms;
using Bonsai.Design;
using Bonsai.Expressions;
using ZedGraph;

namespace Bonsai.Gui.ZedGraph
{
    /// <summary>
    /// Provides a type visualizer that can be used to overlay multiple plots sharing
    /// the same axes in a single graph panel.
    /// </summary>
    public class GraphPanelVisualizer : MashupControlVisualizerBase<GraphControl, GraphPanelBuilder>
    {
        Type indexType;

        /// <summary>
        /// Gets or sets the maximum span of data displayed at any one moment in the graph.
        /// </summary>
        public double Span { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of points displayed at any one moment in the graph.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the lower limit of the X-axis range when using a fixed scale.
        /// </summary>
        public double XMin { get; set; }

        /// <summary>
        /// Gets or sets the upper limit of the X-axis range when using a fixed scale.
        /// </summary>
        public double XMax { get; set; } = 1;

        /// <summary>
        /// Gets or sets the lower limit of the Y-axis range when using a fixed scale.
        /// </summary>
        public double YMin { get; set; }

        /// <summary>
        /// Gets or sets the upper limit of the Y-axis range when using a fixed scale.
        /// </summary>
        public double YMax { get; set; } = 1;

        /// <summary>
        /// Gets or sets a value indicating whether the axis range should be recalculated
        /// automatically as the graph updates.
        /// </summary>
        public bool AutoScaleX { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the axis range should be recalculated
        /// automatically as the graph updates.
        /// </summary>
        public bool AutoScaleY { get; set; } = true;

        internal void EnsureIndex(Type type)
        {
            if (indexType == null)
            {
                indexType = type;
                if (type == typeof(XDate) && Control is GraphPanelView view)
                {
                    view.IsTimeSpan = true;
                }
            }
            else ThrowHelper.ThrowIfNotEquals(indexType, type, "Only overlays with identical indices are allowed.");
        }

        /// <inheritdoc/>
        protected override GraphControl CreateControl(IServiceProvider provider, GraphPanelBuilder builder)
        {
            var view = new GraphPanelView();
            var context = (ITypeVisualizerContext)provider.GetService(typeof(ITypeVisualizerContext));
            var graphPanelBuilder = (GraphPanelBuilder)ExpressionBuilder.GetVisualizerElement(context.Source).Builder;
            var controller = graphPanelBuilder.Controller;
            view.GraphPane.XAxis.Scale.IsReverse = controller.ReverseX;
            view.GraphPane.YAxis.Scale.IsReverse = controller.ReverseY;

            if (controller.XMin.HasValue || controller.XMax.HasValue)
            {
                view.AutoScaleX = false;
                view.AutoScaleXVisible = false;
                view.XMin = controller.XMin.GetValueOrDefault();
                view.XMax = controller.XMax.GetValueOrDefault();
            }
            else
            {
                view.AutoScaleX = AutoScaleX;
                if (!AutoScaleX)
                {
                    view.XMin = XMin;
                    view.XMax = XMax;
                }
            }

            if (controller.YMin.HasValue || controller.YMax.HasValue)
            {
                view.AutoScaleY = false;
                view.AutoScaleYVisible = false;
                view.YMin = controller.YMin.GetValueOrDefault();
                view.YMax = controller.YMax.GetValueOrDefault();
            }
            else
            {
                view.AutoScaleY = AutoScaleY;
                if (!AutoScaleY)
                {
                    view.YMin = YMin;
                    view.YMax = YMax;
                }
            }

            if (controller.Capacity.HasValue)
            {
                view.Capacity = controller.Capacity.Value;
                view.CanEditCapacity = false;
            }
            else
            {
                view.Capacity = Capacity;
                view.CanEditCapacity = true;
            }

            if (controller.Span.HasValue)
            {
                view.Span = controller.Span.Value;
                view.CanEditSpan = false;
            }
            else
            {
                view.Span = Span;
                view.CanEditSpan = true;
            }

            view.Dock = DockStyle.Fill;
            view.HandleDestroyed += delegate
            {
                XMin = view.XMin;
                XMax = view.XMax;
                YMin = view.YMin;
                YMax = view.YMax;
                AutoScaleX = view.AutoScaleX;
                Capacity = view.Capacity;
                Span = view.Span;
            };
            return view;
        }

        /// <inheritdoc/>
        protected override void LoadMashupSource(int index, MashupSource mashupSource, IServiceProvider provider)
        {
            Control.ResetColorCycle();
            base.LoadMashupSource(index, mashupSource, provider);
        }

        /// <inheritdoc/>
        public override void Unload()
        {
            base.Unload();
        }
    }
}
