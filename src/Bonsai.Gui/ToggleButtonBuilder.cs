using System;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a toggle button control and generates
    /// a sequence of notifications whenever the toggle status changes.
    /// </summary>
    [TypeVisualizer(typeof(ToggleButtonVisualizer))]
    [Description("Interfaces with a toggle button control and generates a sequence of notifications whenever the toggle status changes.")]
    public class ToggleButtonBuilder : ButtonBuilderBase<bool>
    {
        internal readonly Subject<bool> _CheckedChanged = new();

        /// <inheritdoc/>
        protected override IObservable<bool> Generate()
        {
            return _CheckedChanged;
        }
    }
}
