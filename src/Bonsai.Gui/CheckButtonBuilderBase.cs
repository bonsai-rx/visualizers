using System;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides an abstract base class with common check button control functionality.
    /// </summary>
    public class CheckButtonBuilderBase : ButtonBuilderBase<bool>
    {
        internal readonly BehaviorSubject<bool> _Checked = new(false);

        /// <summary>
        /// Gets or sets a value indicating whether the button is checked.
        /// </summary>
        [Category(nameof(CategoryAttribute.Appearance))]
        [Description("Indicates whether the button is checked.")]
        public bool Checked
        {
            get => _Checked.Value;
            set => _Checked.OnNext(value);
        }

        /// <inheritdoc/>
        protected override IObservable<bool> Generate()
        {
            return _Checked;
        }
    }
}
