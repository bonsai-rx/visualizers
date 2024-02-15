using System.ComponentModel;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a check box control and generates
    /// a sequence of notifications whenever the checked status changes.
    /// </summary>
    [TypeVisualizer(typeof(CheckBoxVisualizer))]
    [Description("Interfaces with a check box control and generates a sequence of notifications whenever the checked status changes.")]
    public class CheckBoxBuilder : CheckButtonBuilderBase
    {
    }
}
