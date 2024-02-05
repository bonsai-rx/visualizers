using System;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a radio button control and generates
    /// a sequence of notifications whenever the checked status changes.
    /// </summary>
    [TypeVisualizer(typeof(RadioButtonVisualizer))]
    [Description("Interfaces with a radio button control and generates a sequence of notifications whenever the checked status changes.")]
    public class RadioButtonBuilder : ButtonBuilderBase<bool>
    {
        internal readonly Subject<bool> _CheckedChanged = new();

        /// <summary>
        /// Gets or sets a value specifying the initial state of the radio button.
        /// </summary>
        [Category(nameof(CategoryAttribute.Appearance))]
        [Description("Specifies the initial state of the radio button.")]
        public bool Checked { get; set; }

        /// <inheritdoc/>
        protected override IObservable<bool> Generate()
        {
            return _CheckedChanged;
        }
    }
}
