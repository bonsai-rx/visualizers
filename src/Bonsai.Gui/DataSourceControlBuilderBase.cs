using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides an abstract base class for UI controls which can be bound to each
    /// data source from an observable sequence.
    /// </summary>
    public abstract class DataSourceControlBuilderBase : ControlBuilderBase
    {
        static readonly Range<int> argumentRange = Range.Create(lowerBound: 1, upperBound: 1);

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
            var source = arguments.First();
            var sourceType = source.Type.GetGenericArguments()[0];
            var valueType = ExpressionHelper.GetGenericTypeBindings(typeof(IList<>), sourceType);
            return Expression.Call(Expression.Constant(this), nameof(Generate), valueType, source);
        }

        /// <summary>
        /// Generates an observable sequence of values containing the currently
        /// selected item from the data source whenever the selection changes.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the values in the data source.
        /// </typeparam>
        /// <param name="source">
        /// A sequence of collections representing the data sources to bind to
        /// the UI control. Only one collection is bound at any one time.
        /// </param>
        /// <returns>
        /// A sequence of values representing the currently selected item in
        /// the UI control.
        /// </returns>
        protected abstract IObservable<TValue> Generate<TValue>(IObservable<IList<TValue>> source);
    }
}
