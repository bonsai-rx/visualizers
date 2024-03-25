using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive;
using ZedGraph;

namespace Bonsai.Gui.ZedGraph
{
    /// <summary>
    /// Represents an operator that specifies a mashup graph panel that can be used
    /// to combine multiple graphs displaying data over a shared axis.
    /// </summary>
    [TypeVisualizer(typeof(RollingGraphPanelVisualizer))]
    [Description("Specifies a mashup graph panel that can be used to combine multiple graphs displaying data over a shared axis.")]
    public class RollingGraphPanelBuilder : GraphPanelBuilderBase
    {
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
            internal bool ReverseX;
            internal bool ReverseY;
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
                Max = Max,
                ReverseX = ReverseX,
                ReverseY = ReverseY
            };
            return Expression.Call(typeof(Observable), nameof(Observable.Never), new[] { typeof(Unit) });
        }
    }
}
