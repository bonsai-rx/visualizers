using System.ComponentModel;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a combo box control and generates
    /// a sequence of notifications whenever the selection changes.
    /// </summary>
    [TypeVisualizer(typeof(ComboBoxVisualizer))]
    [Description("Interfaces with a combo box control and generates a sequence of notifications whenever the selection changes.")]
    public class ComboBoxBuilder : ListControlBuilderBase
    {
    }
}
