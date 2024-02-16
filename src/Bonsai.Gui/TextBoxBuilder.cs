using System;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a text box control and generates
    /// a sequence of notifications whenever the text changes.
    /// </summary>
    [TypeVisualizer(typeof(TextBoxVisualizer))]
    public class TextBoxBuilder : TextControlBuilderBase<string>
    {
        internal readonly BehaviorSubject<bool> _Multiline = new(true);

        /// <summary>
        /// Gets or sets a value specifying whether the text box is multiline.
        /// </summary>
        [Description("Specifies whether the text box is multiline.")]
        public bool Multiline
        {
            get => _Multiline.Value;
            set => _Multiline.OnNext(value);
        }

        /// <summary>
        /// Generates an observable sequence of values containing the contents
        /// of the text box whenever the text changes.
        /// </summary>
        /// <returns>
        /// A sequence of <see cref="string"/> values representing the contents
        /// of the text box.
        /// </returns>
        protected override IObservable<string> Generate()
        {
            return _Text;
        }
    }
}
