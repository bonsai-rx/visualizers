using System.ComponentModel;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a list box control and generates
    /// a sequence of notifications whenever the selection changes.
    /// </summary>
    [TypeVisualizer(typeof(ListBoxVisualizer))]
    [Description("Interfaces with a list box control and generates a sequence of notifications whenever the selection changes.")]
    public class ListBoxBuilder : ListControlBuilderBase
    {
    }
}
