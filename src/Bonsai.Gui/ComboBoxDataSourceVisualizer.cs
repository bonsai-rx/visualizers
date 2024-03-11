using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a combo box control bound
    /// to an arbitrary data source.
    /// </summary>
    public class ComboBoxDataSourceVisualizer : ControlVisualizerBase<ComboBox, ComboBoxDataSourceBuilder>
    {
        /// <inheritdoc/>
        protected override ComboBox CreateControl(IServiceProvider provider, ComboBoxDataSourceBuilder builder)
        {
            var comboBox = new ComboBox();
            comboBox.Dock = DockStyle.Fill;
            comboBox.Size = new Size(300, 150);
            comboBox.SubscribeTo(builder._DisplayMember, value => comboBox.DisplayMember = value);
            comboBox.SubscribeTo(builder._DataSource, value => comboBox.DataSource = value);
            comboBox.SelectedIndexChanged += (sender, e) =>
            {
                var index = comboBox.SelectedIndex;
                var selectedValue = index < 0 ? null : comboBox.Items[index];
                builder._SelectedItem.OnNext(selectedValue);
            };
            return comboBox;
        }
    }
}
