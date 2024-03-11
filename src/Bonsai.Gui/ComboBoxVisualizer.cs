using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a combo box control.
    /// </summary>
    public class ComboBoxVisualizer : ControlVisualizerBase<ComboBox, ComboBoxBuilder>
    {
        /// <inheritdoc/>
        protected override ComboBox CreateControl(IServiceProvider provider, ComboBoxBuilder builder)
        {
            var comboBox = new ComboBox();
            comboBox.Dock = DockStyle.Fill;
            comboBox.Size = new Size(300, 150);
            comboBox.DataSource = builder._Items;
            comboBox.SubscribeTo(builder._Items, values => comboBox.DataSource = values);
            comboBox.SubscribeTo(builder._SelectedItem, value =>
            {
                var index = value == null ? -1 : comboBox.Items.IndexOf(value);
                if (index < 0 && comboBox.Items.Count > 0) index = 0;
                comboBox.SelectedIndex = index;
            });
            comboBox.SelectedIndexChanged += (sender, e) =>
            {
                var index = comboBox.SelectedIndex;
                var selectedValue = index < 0 ? string.Empty : comboBox.Items[index];
                builder._SelectedItem.OnNext((string)selectedValue);
            };
            return comboBox;
        }
    }
}
