using System.ComponentModel;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a toggle button control and generates
    /// a sequence of notifications whenever the toggle status changes.
    /// </summary>
    [TypeVisualizer(typeof(ToggleButtonVisualizer))]
    [Description("Interfaces with a toggle button control and generates a sequence of notifications whenever the toggle status changes.")]
    public class ToggleButtonBuilder : CheckButtonBuilderBase
    {
    }
}
