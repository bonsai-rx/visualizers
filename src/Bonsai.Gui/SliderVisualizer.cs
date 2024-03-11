using System;
using System.Drawing;
using System.Windows.Forms;
using Bonsai.Design;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a slider control.
    /// </summary>
    public class SliderVisualizer : ControlVisualizerBase<Slider, SliderBuilder>
    {
        /// <inheritdoc/>
        protected override Slider CreateControl(IServiceProvider provider, SliderBuilder builder)
        {
            var slider = new Slider();
            slider.Dock = DockStyle.Fill;
            slider.Size = new Size(300, slider.Height);
            slider.SubscribeTo(builder._Minimum, value => slider.Minimum = value);
            slider.SubscribeTo(builder._Maximum, value => slider.Maximum = value);
            slider.SubscribeTo(builder._DecimalPlaces, value => slider.DecimalPlaces = value);
            slider.SubscribeTo(builder._Value, value => slider.Value = builder.Value);
            slider.ValueChanged += (sender, e) =>
            {
                builder._Value.OnNext(slider.Value);
            };
            return slider;
        }
    }
}
