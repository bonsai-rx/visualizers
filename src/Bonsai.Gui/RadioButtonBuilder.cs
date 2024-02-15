using System.ComponentModel;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a radio button control and generates
    /// a sequence of notifications whenever the checked status changes.
    /// </summary>
    [TypeVisualizer(typeof(RadioButtonVisualizer))]
    [Description("Interfaces with a radio button control and generates a sequence of notifications whenever the checked status changes.")]
    public class RadioButtonBuilder : CheckButtonBuilderBase
    {
    }
}
