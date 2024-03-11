using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a list box control.
    /// </summary>
    public class ListBoxVisualizer : ControlVisualizerBase<ListBox, ListBoxBuilder>
    {
        /// <inheritdoc/>
        protected override ListBox CreateControl(IServiceProvider provider, ListBoxBuilder builder)
        {
            var listBox = new ListBox();
            listBox.Dock = DockStyle.Fill;
            listBox.Size = new Size(300, 150);
            listBox.DataSource = builder._Items;
            listBox.SubscribeTo(builder._Items, values => listBox.DataSource = values);
            listBox.SubscribeTo(builder._SelectedItem, value =>
            {
                var index = value == null ? -1 : listBox.Items.IndexOf(value);
                if (index < 0 && listBox.Items.Count > 0) index = 0;
                listBox.SelectedIndex = index;
            });
            listBox.SelectedIndexChanged += (sender, e) =>
            {
                var index = listBox.SelectedIndex;
                var selectedValue = index < 0 ? string.Empty : listBox.Items[index];
                builder._SelectedItem.OnNext((string)selectedValue);
            };
            return listBox;
        }
    }
}
