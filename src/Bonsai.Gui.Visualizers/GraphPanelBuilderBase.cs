using System.ComponentModel;
using Bonsai.Expressions;

namespace Bonsai.Gui.Visualizers
{
    /// <summary>
    /// Provides an abstract base class with common mashup graph panel functionality.
    /// </summary>
    public abstract class GraphPanelBuilderBase : ZeroArgumentExpressionBuilder, INamedElement
    {
        /// <summary>
        /// Gets or sets the name of the visualizer window.
        /// </summary>
        [Category(nameof(CategoryAttribute.Design))]
        [Description("The name of the visualizer window.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether the scale values are reversed on the X-axis.
        /// </summary>
        [Category(nameof(CategoryAttribute.Appearance))]
        [Description("Specifies whether the scale values are reversed on the X-axis.")]
        public bool ReverseX { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether the scale values are reversed on the Y-axis.
        /// </summary>
        [Category(nameof(CategoryAttribute.Appearance))]
        [Description("Specifies whether the scale values are reversed on the Y-axis.")]
        public bool ReverseY { get; set; }

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
    }
}
