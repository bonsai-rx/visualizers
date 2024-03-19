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
        /// Builds the expression tree for configuring and calling the
        /// graph panel visualizer.
        /// </summary>
        /// <inheritdoc/>
        public override Expression Build(IEnumerable<Expression> arguments)
        {
            return Expression.Call(typeof(Observable), nameof(Observable.Never), new[] { typeof(Unit) });
        }
    }
}
