using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a radio button control.
    /// </summary>
    public class RadioButtonVisualizer : ControlVisualizerBase<RadioButton, RadioButtonBuilder>
    {
        /// <inheritdoc/>
        protected override RadioButton CreateControl(IServiceProvider provider, RadioButtonBuilder builder)
        {
            var radioButton = new RadioButton();
            radioButton.Dock = DockStyle.Fill;
            radioButton.Size = new Size(300, 75);
            radioButton.Checked = builder.Checked;
            radioButton.SubscribeTo(builder._Text, value => radioButton.Text = value);
            radioButton.SubscribeTo(builder._Checked, value => radioButton.Checked = value);
            radioButton.CheckedChanged += (sender, e) =>
            {
                builder._Checked.OnNext(radioButton.Checked);
            };
            return radioButton;
        }
    }
}
