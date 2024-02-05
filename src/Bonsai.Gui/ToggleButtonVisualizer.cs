using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a toggle button control.
    /// </summary>
    public class ToggleButtonVisualizer : ControlVisualizerBase<CheckBox, ToggleButtonBuilder>
    {
        /// <inheritdoc/>
        protected override CheckBox CreateControl(IServiceProvider provider, ToggleButtonBuilder builder)
        {
            var checkBox = new CheckBox();
            checkBox.Dock = DockStyle.Fill;
            checkBox.Size = new Size(300, 150);
            checkBox.Appearance = Appearance.Button;
            checkBox.TextAlign = ContentAlignment.MiddleCenter;
            checkBox.SubscribeTo(builder._Text, value => checkBox.Text = value);
            checkBox.CheckedChanged += (sender, e) =>
            {
                builder._CheckedChanged.OnNext(checkBox.Checked);
            };
            return checkBox;
        }
    }
}
