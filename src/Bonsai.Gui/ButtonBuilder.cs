using System;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a button control and generates
    /// a sequence of notifications whenever the button is clicked.
    /// </summary>
    [TypeVisualizer(typeof(ButtonVisualizer))]
    [Description("Interfaces with a button control and generates a sequence of notifications whenever the button is clicked.")]
    public class ButtonBuilder : ButtonBuilderBase<string>
    {
        internal readonly Subject<string> _Click = new();

        /// <inheritdoc/>
        protected override IObservable<string> Generate()
        {
            return _Click;
        }
    }
}
