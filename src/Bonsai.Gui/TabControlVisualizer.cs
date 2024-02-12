using System;
using Bonsai.Design;
using Bonsai.Gui;
using Bonsai;
using System.Windows.Forms;
using System.Drawing;

[assembly: TypeVisualizer(typeof(DialogTypeVisualizer), Target = typeof(MashupSource<TabControlVisualizer>))]

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer that can be used to arrange other visualizers
    /// using a related set of tab pages.
    /// </summary>
    public class TabControlVisualizer : ContainerControlVisualizerBase<TabControl, TabControlBuilder>
    {
        /// <inheritdoc/>
        protected override TabControl CreateControl(IServiceProvider provider, TabControlBuilder builder)
        {
            var tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Size = new Size(320, 240);
            return tabControl;
        }

        /// <inheritdoc/>
        protected override void AddControl(int index, Control control)
        {
            var tabPage = new TabPage(control.Name);
            tabPage.Controls.Add(control);
            Control.Controls.Add(tabPage);
        }
    }
}
