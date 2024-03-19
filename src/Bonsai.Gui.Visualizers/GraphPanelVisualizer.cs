using System;
using System.Windows.Forms;
using Bonsai.Design;
using Bonsai.Expressions;
using ZedGraph;

namespace Bonsai.Gui.Visualizers
{
    /// <summary>
    /// Provides a type visualizer that can be used to overlay multiple plots sharing
    /// the same axes in a single graph panel.
    /// </summary>
    public class GraphPanelVisualizer : MashupControlVisualizerBase<GraphControl, GraphPanelBuilder>
    {
        Type indexType;
        BarSettings barSettings;
        GraphPanelBuilder graphBuilder;

        /// <summary>
        /// Gets or sets the maximum span of data displayed at any one moment in the graph.
        /// </summary>
        public double Span { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of points displayed at any one moment in the graph.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the lower limit of the axis range when using a fixed scale.
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// Gets or sets the upper limit of the axis range when using a fixed scale.
        /// </summary>
        public double Max { get; set; } = 1;

        /// <summary>
        /// Gets or sets a value indicating whether the axis range should be recalculated
        /// automatically as the graph updates.
        /// </summary>
        public bool AutoScale { get; set; } = true;

        internal BarSettings BarSettings => barSettings;

        private Axis BarBaseAxis()
        {
            return graphBuilder.BaseAxis switch
            {
                BarBase.Y => Control.GraphPane.YAxis,
                BarBase.Y2 => Control.GraphPane.Y2Axis,
                BarBase.X2 => Control.GraphPane.X2Axis,
                _ => Control.GraphPane.XAxis
            };
        }

        internal void EnsureIndex(Type type)
        {
            if (indexType == null)
            {
                indexType = type;
                var baseAxis = BarBaseAxis();
                if (type == typeof(string))
                {
                    GraphHelper.FormatOrdinalAxis(baseAxis, indexType);
                }
                if (type == typeof(XDate))
                {
                    GraphHelper.FormatLinearDateAxis(baseAxis);
                }
            }
            else ThrowHelper.ThrowIfNotEquals(indexType, type, "Only overlays with identical axis are allowed.");
        }

        internal void EnsureBarSettings(BarSettings settings)
        {
            const string ErrorMessage = "All bar graph overlays must have bar settings compatible with the graph panel.";
            ThrowHelper.ThrowIfNotEquals(barSettings.Base, settings.Base, ErrorMessage);
            ThrowHelper.ThrowIfNotEquals(barSettings.ClusterScaleWidth, settings.ClusterScaleWidth, ErrorMessage);
            ThrowHelper.ThrowIfNotEquals(barSettings.ClusterScaleWidthAuto, settings.ClusterScaleWidthAuto, ErrorMessage);
            ThrowHelper.ThrowIfNotEquals(barSettings.MinBarGap, settings.MinBarGap, ErrorMessage);
            ThrowHelper.ThrowIfNotEquals(barSettings.MinClusterGap, settings.MinClusterGap, ErrorMessage);
            ThrowHelper.ThrowIfNotEquals(barSettings.Type, settings.Type, ErrorMessage);
        }

        /// <inheritdoc/>
        protected override GraphControl CreateControl(IServiceProvider provider, GraphPanelBuilder builder)
        {
            var view = new GraphPanelView();
            var context = (ITypeVisualizerContext)provider.GetService(typeof(ITypeVisualizerContext));
            var graphPanelBuilder = (GraphPanelBuilder)ExpressionBuilder.GetVisualizerElement(context.Source).Builder;
            var controller = graphPanelBuilder.Controller;
            barSettings = view.GraphPane.BarSettings;
            barSettings.Base = controller.BaseAxis;
            barSettings.Type = controller.BarType;

            if (controller.Min.HasValue || controller.Max.HasValue)
            {
                view.AutoScale = false;
                view.AutoScaleVisible = false;
                view.Min = controller.Min.GetValueOrDefault();
                view.Max = controller.Max.GetValueOrDefault();
            }
            else
            {
                view.AutoScale = AutoScale;
                if (!AutoScale)
                {
                    view.Min = Min;
                    view.Max = Max;
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
                Min = view.Min;
                Max = view.Max;
                AutoScale = view.AutoScale;
                Capacity = view.Capacity;
                Span = view.Span;
            };
            graphBuilder = builder;
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
            indexType = null;
            barSettings = null;
            graphBuilder = null;
        }
    }
}
