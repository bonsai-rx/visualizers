using System.ComponentModel;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a label control.
    /// </summary>
    [TypeVisualizer(typeof(LabelVisualizer))]
    [Description("Interfaces with a label control.")]
    public class LabelBuilder : TextControlBuilderBase
    {
    }
}
