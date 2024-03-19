using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive;
using Bonsai.Expressions;
using ZedGraph;

namespace Bonsai.Gui.Visualizers
{
    /// <summary>
    /// Represents an operator that specifies a mashup graph panel that can be used
    /// to combine multiple plots sharing the same axes.
    /// </summary>
    [TypeVisualizer(typeof(GraphPanelVisualizer))]
    [Description("Specifies a mashup graph panel that can be used to combine multiple plots sharing the same axes.")]
    public class GraphPanelBuilder : ZeroArgumentExpressionBuilder, INamedElement
    {
        /// <summary>
        /// Gets or sets the name of the visualizer window.
        /// </summary>
        [Category(nameof(CategoryAttribute.Design))]
        [Description("The name of the visualizer window.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the axis on which the bars in the graph will be displayed.
        /// </summary>
        [TypeConverter(typeof(BaseAxisConverter))]
        [Category(nameof(CategoryAttribute.Appearance))]
        [Description("Specifies the axis on which the bars in the graph will be displayed.")]
        public BarBase BaseAxis { get; set; }

        /// <summary>
        /// Gets or sets a value specifying how the different bars in the graph will be visually arranged.
        /// </summary>
        [Category(nameof(CategoryAttribute.Appearance))]
        [Description("Specifies how the different bars in the graph will be visually arranged.")]
        public BarType BarType { get; set; }

        /// <summary>
        /// Gets or sets the optional maximum span of data displayed at any one moment in the graph.
        /// If no span is specified, all data points will be displayed.
        /// </summary>
        [Category("Range")]
        [Description("The optional maximum span of data displayed at any one moment in the graph. " +
                     "If no span is specified, all data points will be displayed.")]
        public double? Span { get; set; }

        /// <summary>
        /// Gets or sets the optional capacity used for rolling line graphs. If no capacity is specified, all data points will be displayed.
        /// </summary>
        [Category("Range")]
        [Description("The optional capacity used for rolling line graphs. If no capacity is specified, all data points will be displayed.")]
        public int? Capacity { get; set; }

        /// <summary>
        /// Gets or sets a value specifying a fixed lower limit for the axis range.
        /// If no fixed range is specified, the graph limits can be edited online.
        /// </summary>
        [Category("Range")]
        [Description("Specifies the optional fixed lower limit of the axis range.")]
        public double? Min { get; set; }

        /// <summary>
        /// Gets or sets a value specifying a fixed upper limit for the axis range.
        /// If no fixed range is specified, the graph limits can be edited online.
        /// </summary>
        [Category("Range")]
        [Description("Specifies the optional fixed upper limit of the axis range.")]
        public double? Max { get; set; }

        internal VisualizerController Controller { get; set; }

        internal class VisualizerController
        {
            internal BarBase BaseAxis;
            internal BarType BarType;
            internal double? Span;
            internal int? Capacity;
            internal double? Min;
            internal double? Max;
        }

        /// <summary>
        /// Builds the expression tree for configuring and calling the
        /// graph panel visualizer.
        /// </summary>
        /// <inheritdoc/>
        public override Expression Build(IEnumerable<Expression> arguments)
        {
            Controller = new VisualizerController
            {
                BaseAxis = BaseAxis,
                BarType = BarType,
                Span = Span,
                Capacity = Capacity,
                Min = Min,
                Max = Max
            };
            return Expression.Call(typeof(Observable), nameof(Observable.Never), new[] { typeof(Unit) });
        }
    }
}
