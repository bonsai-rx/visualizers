using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive;

namespace Bonsai.Gui.ZedGraph
{
    /// <summary>
    /// Represents an operator that specifies a mashup graph panel that can be used
    /// to combine multiple plots sharing the same axes.
    /// </summary>
    [TypeVisualizer(typeof(GraphPanelVisualizer))]
    [Description("Specifies a mashup graph panel that can be used to combine multiple plots sharing the same axes.")]
    public class GraphPanelBuilder : GraphPanelBuilderBase
    {
        /// <summary>
        /// Gets or sets a value specifying a fixed lower limit for the X-axis range.
        /// If no fixed range is specified, the graph limits can be edited online.
        /// </summary>
        [Category("Range")]
        [Description("Specifies the optional fixed lower limit of the X-axis range.")]
        public double? XMin { get; set; }

        /// <summary>
        /// Gets or sets a value specifying a fixed upper limit for the X-axis range.
        /// If no fixed range is specified, the graph limits can be edited online.
        /// </summary>
        [Category("Range")]
        [Description("Specifies the optional fixed upper limit of the X-axis range.")]
        public double? XMax { get; set; }

        /// <summary>
        /// Gets or sets a value specifying a fixed lower limit for the Y-axis range.
        /// If no fixed range is specified, the graph limits can be edited online.
        /// </summary>
        [Category("Range")]
        [Description("Specifies the optional fixed lower limit of the Y-axis range.")]
        public double? YMin { get; set; }

        /// <summary>
        /// Gets or sets a value specifying a fixed upper limit for the Y-axis range.
        /// If no fixed range is specified, the graph limits can be edited online.
        /// </summary>
        [Category("Range")]
        [Description("Specifies the optional fixed upper limit of the Y-axis range.")]
        public double? YMax { get; set; }

        internal VisualizerController Controller { get; set; }

        internal class VisualizerController
        {
            internal double? Span;
            internal int? Capacity;
            internal double? XMin;
            internal double? XMax;
            internal double? YMin;
            internal double? YMax;
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
                Span = Span,
                Capacity = Capacity,
                XMin = XMin,
                XMax = XMax,
                YMin = YMin,
                YMax = YMax,
                ReverseX = ReverseX,
                ReverseY = ReverseY
            };
            return Expression.Call(typeof(Observable), nameof(Observable.Never), new[] { typeof(Unit) });
        }
    }
}
