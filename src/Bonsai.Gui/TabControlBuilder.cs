using System.ComponentModel;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that specifies a mashup visualizer that can be used
    /// to arrange other visualizers using a related set of tab pages.
    /// </summary>
    [TypeVisualizer(typeof(TabControlVisualizer))]
    [Description("Specifies a mashup visualizer that can be used to arrange other visualizers using a related set of tab pages.")]
    public class TabControlBuilder : ControlBuilderBase
    {
    }
}
