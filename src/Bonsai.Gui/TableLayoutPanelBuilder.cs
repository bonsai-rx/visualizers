using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that specifies a mashup visualizer panel that can be used
    /// to arrange other visualizers in a grid.
    /// </summary>
    [DefaultProperty(nameof(CellSpans))]
    [TypeVisualizer(typeof(TableLayoutPanelVisualizer))]
    [Description("Specifies a mashup visualizer panel that can be used to arrange other visualizers in a grid.")]
    public class TableLayoutPanelBuilder : ControlBuilderBase
    {
        /// <summary>
        /// Gets or sets the number of columns in the visualizer grid layout.
        /// </summary>
        [Description("The number of columns in the visualizer grid layout.")]
        public int ColumnCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of rows in the visualizer grid layout.
        /// </summary>
        [Description("The number of rows in the visualizer grid layout.")]
        public int RowCount { get; set; } = 1;

        /// <summary>
        /// Gets a collection of <see cref="ColumnStyle"/> objects specifying the size
        /// ratio of the columns in the visualizer grid layout.
        /// </summary>
        [Category("Table Style")]
        [Description("Specifies the optional size ratio of the columns in the visualizer grid layout.")]
        public Collection<ColumnStyle> ColumnStyles { get; } = new();

        /// <summary>
        /// Gets a collection of <see cref="RowStyle"/> objects specifying the size ratio
        /// of the rows in the visualizer grid layout.
        /// </summary>
        [Category("Table Style")]
        [Description("Specifies the optional size ratio of the rows in the visualizer grid layout.")]
        public Collection<RowStyle> RowStyles { get; } = new();

        /// <summary>
        /// Gets a collection of <see cref="TableLayoutPanelCellSpan"/> objects specifying the
        /// column and row span of each cell in the visualizer grid layout.
        /// </summary>
        [Category("Table Style")]
        [XmlArrayItem("CellSpan")]
        [Description("Specifies the optional column and row span of each cell in the visualizer grid layout.")]
        public Collection<TableLayoutPanelCellSpan> CellSpans { get; } = new();
    }
}
