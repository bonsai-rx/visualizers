using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Bonsai.Expressions;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Reactive;
using System.Linq;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides an abstract base class with common UI control functionality.
    /// </summary>
    public abstract class ControlBuilderBase : ZeroArgumentExpressionBuilder, INamedElement
    {
        /// <summary>
        /// Gets or sets the name of the control.
        /// </summary>
        [Category(nameof(CategoryAttribute.Design))]
        [Description("The name of the control.")]
        public string Name { get; set; }

        /// <summary>
        /// Builds the expression tree for configuring and calling the UI control.
        /// </summary>
        /// <inheritdoc/>
        public override Expression Build(IEnumerable<Expression> arguments)
        {
            return Expression.Call(typeof(Observable), nameof(Observable.Never), new[] { typeof(Unit) });
        }
    }

    /// <summary>
    /// Provides an abstract base class with common UI event source control functionality.
    /// </summary>
    /// <typeparam name="TEvent">The type of event notifications emitted by the UI control.</typeparam>
    public abstract class ControlBuilderBase<TEvent> : ControlBuilderBase, INamedElement
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

    /// <summary>
    /// Provides an abstract base class with common functionality for UI controls that
    /// accept an optional sequence of command notifications.
    /// </summary>
    /// <typeparam name="TCommand">The type of command notifications accepted by the UI control.</typeparam>
    /// <typeparam name="TEvent">The type of event notifications emitted by the UI control.</typeparam>
    public abstract class ControlBuilderBase<TCommand, TEvent> : ControlBuilderBase, INamedElement
    {
        static readonly Range<int> argumentRange = Range.Create(lowerBound: 0, upperBound: 1);

        /// <summary>
        /// Gets the range of input arguments that this expression builder accepts.
        /// </summary>
        public override Range<int> ArgumentRange => argumentRange;

        /// <summary>
        /// Builds the expression tree for configuring and calling the UI control.
        /// </summary>
        /// <inheritdoc/>
        public override Expression Build(IEnumerable<Expression> arguments)
        {
            var source = arguments.SingleOrDefault();
            var commands = source == null ? Array.Empty<Expression>() : new[] { source };
            return Expression.Call(Expression.Constant(this), nameof(Generate), null, commands);
        }

        /// <summary>
        /// Generates an observable sequence of event notifications from the UI control.
        /// </summary>
        /// <returns>
        /// An observable sequence of events of type <typeparamref name="TEvent"/>.
        /// </returns>
        protected abstract IObservable<TEvent> Generate();

        /// <summary>
        /// Generates an observable sequence of event notifications from the UI control.
        /// </summary>
        /// <param name="source">
        /// An observable sequence of commands of type <typeparamref name="TCommand"/>.
        /// </param>
        /// <returns>
        /// An observable sequence of events of type <typeparamref name="TEvent"/>.
        /// </returns>
        protected abstract IObservable<TEvent> Generate(IObservable<TCommand> source);
    }
}
