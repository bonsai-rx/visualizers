﻿using System.ComponentModel;
using System.Reactive.Subjects;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides an abstract base class for common UI text control functionality.
    /// </summary>
    [DefaultProperty(nameof(Text))]
    public abstract class TextControlBuilderBase : ControlBuilderBase
    {
        internal readonly BehaviorSubject<string> _Text = new(string.Empty);

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        [Category(nameof(CategoryAttribute.Appearance))]
        [Description("The text associated with this control.")]
        public string Text
        {
            get => _Text.Value;
            set => _Text.OnNext(value);
        }
    }
}
