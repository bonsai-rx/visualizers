using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a text box control.
    /// </summary>
    public class TextBoxVisualizer : ControlVisualizerBase<TextBox, TextBoxBuilder>
    {
        /// <inheritdoc/>
        protected override TextBox CreateControl(IServiceProvider provider, TextBoxBuilder builder)
        {
            var textBox = new TextBox();
            textBox.Dock = DockStyle.Fill;
            textBox.Multiline = builder._Multiline.Value;
            if (textBox.Multiline)
            {
                textBox.Size = new Size(320, 240);
            }

            textBox.SubscribeTo(builder._Multiline, value => textBox.Multiline = value);
            textBox.TextChanged += (sender, e) =>
            {
                builder._Text.OnNext(textBox.Text);
            };
            return textBox;
        }
    }
}
