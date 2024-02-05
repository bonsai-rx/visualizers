using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a button control.
    /// </summary>
    public class ButtonVisualizer : ControlVisualizerBase<Button, ButtonBuilder>
    {
        /// <inheritdoc/>
        protected override Button CreateControl(IServiceProvider provider, ButtonBuilder builder)
        {
            var button = new Button();
            button.Dock = DockStyle.Fill;
            button.Size = new Size(300, 150);
            button.SubscribeTo(builder._Text, value => button.Text = value);
            button.Click += (sender, e) =>
            {
                builder._Click.OnNext(button.Name);
            };
            return button;
        }
    }
}
