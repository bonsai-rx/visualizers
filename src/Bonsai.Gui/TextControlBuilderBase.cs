using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
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

    /// <summary>
    /// Provides an abstract base class with common UI text and event source control functionality.
    /// </summary>
    /// <typeparam name="TEvent">The type of event notifications emitted by the UI control.</typeparam>
    public abstract class TextControlBuilderBase<TEvent> : TextControlBuilderBase, INamedElement
    {
        /// <summary>
        /// Builds the expression tree for configuring and calling the UI control.
        /// </summary>
        /// <inheritdoc/>
        public override Expression Build(IEnumerable<Expression> arguments)
        {
            return Expression.Call(Expression.Constant(this), nameof(Generate), null);
        }

        /// <summary>
        /// Generates an observable sequence of event notifications from the UI control.
        /// </summary>
        /// <returns>
        /// An observable sequence of events of type <typeparamref name="TEvent"/>.
        /// </returns>
        protected abstract IObservable<TEvent> Generate();
    }
}
