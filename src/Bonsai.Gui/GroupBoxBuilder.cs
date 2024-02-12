using System.ComponentModel;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that specifies a mashup visualizer that displays a frame
    /// around a group of other visualizers with an optional caption.
    /// </summary>
    [TypeVisualizer(typeof(GroupBoxVisualizer))]
    [Description("Specifies a mashup visualizer that displays a frame around a group of other visualizers with an optional caption.")]
    public class GroupBoxBuilder : TextControlBuilderBase
    {
    }
}
