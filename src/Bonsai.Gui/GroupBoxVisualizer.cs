using System;
using System.Drawing;
using System.Windows.Forms;
using Bonsai.Design;
using Bonsai.Gui;
using Bonsai;

[assembly: TypeVisualizer(typeof(DialogTypeVisualizer), Target = typeof(MashupSource<GroupBoxVisualizer>))]

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer that displays a frame around a group of other
    /// visualizers with an optional caption.
    /// </summary>
    public class GroupBoxVisualizer : ContainerControlVisualizerBase<GroupBox, GroupBoxBuilder>
    {
        /// <inheritdoc/>
        protected override void AddControl(int index, Control control)
        {
            Control.Controls.Add(control);
        }

        /// <inheritdoc/>
        protected override GroupBox CreateControl(IServiceProvider provider, GroupBoxBuilder builder)
        {
            var groupBox = new GroupBox();
            groupBox.Dock = DockStyle.Fill;
            groupBox.Size = new Size(320, 240);
            groupBox.SubscribeTo(builder._Text, value => groupBox.Text = value);
            return groupBox;
        }
    }
}
