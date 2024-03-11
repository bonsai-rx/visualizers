using System.ComponentModel;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a combo box control bound to each data
    /// source in the sequence and generates notifications whenever the selection changes.
    /// </summary>
    [TypeVisualizer(typeof(ComboBoxDataSourceVisualizer))]
    [Description("Interfaces with a combo box control bound to each data source in the sequence and generates notifications whenever the selection changes.")]
    public class ComboBoxDataSourceBuilder : ListControlDataSourceBuilderBase
    {
    }
}
