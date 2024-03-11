using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a list box control bound
    /// to an arbitrary data source.
    /// </summary>
    public class ListBoxDataSourceVisualizer : ControlVisualizerBase<ListBox, ListBoxDataSourceBuilder>
    {
        /// <inheritdoc/>
        protected override ListBox CreateControl(IServiceProvider provider, ListBoxDataSourceBuilder builder)
        {
            var listBox = new ListBox();
            listBox.Dock = DockStyle.Fill;
            listBox.Size = new Size(300, 150);
            listBox.SubscribeTo(builder._DisplayMember, value => listBox.DisplayMember = value);
            listBox.SubscribeTo(builder._DataSource, value => listBox.DataSource = value);
            listBox.SelectedIndexChanged += (sender, e) =>
            {
                var index = listBox.SelectedIndex;
                var selectedValue = index < 0 ? null : listBox.Items[index];
                builder._SelectedItem.OnNext(selectedValue);
            };
            return listBox;
        }
    }
}
