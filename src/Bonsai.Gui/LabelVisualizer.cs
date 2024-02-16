using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a label control.
    /// </summary>
    public class LabelVisualizer : ControlVisualizerBase<Label, LabelBuilder>
    {
        /// <inheritdoc/>
        protected override Label CreateControl(IServiceProvider provider, LabelBuilder builder)
        {
            var label = new Label();
            label.Dock = DockStyle.Fill;
            label.Size = new Size(300, label.Height);
            return label;
        }
    }
}
