using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a check box control.
    /// </summary>
    public class CheckBoxVisualizer : ControlVisualizerBase<CheckBox, CheckBoxBuilder>
    {
        /// <inheritdoc/>
        protected override CheckBox CreateControl(IServiceProvider provider, CheckBoxBuilder builder)
        {
            var checkBox = new CheckBox();
            checkBox.Dock = DockStyle.Fill;
            checkBox.Size = new Size(300, 75);
            checkBox.Checked = builder.Checked;
            checkBox.SubscribeTo(builder._Text, value => checkBox.Text = value);
            checkBox.SubscribeTo(builder._Checked, value => checkBox.Checked = value);
            checkBox.CheckedChanged += (sender, e) =>
            {
                builder._Checked.OnNext(checkBox.Checked);
            };
            return checkBox;
        }
    }
}
