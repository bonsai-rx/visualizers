using System.ComponentModel;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a list box control bound to each data
    /// source in the sequence and generates notifications whenever the selection changes.
    /// </summary>
    [TypeVisualizer(typeof(ListBoxDataSourceVisualizer))]
    [Description("Interfaces with a list box control bound to each data source in the sequence and generates notifications whenever the selection changes.")]
    public class ListBoxDataSourceBuilder : ListControlDataSourceBuilderBase
    {
    }
}
