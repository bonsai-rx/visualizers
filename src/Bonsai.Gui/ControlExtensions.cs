using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using Bonsai.Design;
using Bonsai.Expressions;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a set of static methods for subscribing delegates to
    /// observable sequences of event notifications.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Subscribes the control to change notifications in the specified
        /// observable collection.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> collection.
        /// </typeparam>
        /// <param name="control">The control on which to observe notifications.</param>
        /// <param name="source">
        /// A data collection that provides notifications when items are added,
        /// removed, or when the whole list is refreshed.
        /// </param>
        /// <param name="action">
        /// The action to invoke on each new version of the observable collection.
        /// </param>
        /// <returns>
        /// A disposable object used to unsubscribe from the observable sequence.
        /// </returns>
        public static IDisposable SubscribeTo<TSource>(
            this Control control,
            ObservableCollection<TSource> source,
            Action<TSource[]> action)
        {
            var collectionChanged = Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                handler => source.CollectionChanged += handler,
                handler => source.CollectionChanged -= handler)
                .Select(evt => source.ToArray());
            return SubscribeTo(control, collectionChanged, action);
        }

        /// <summary>
        /// Subscribes the control to event notifications from the specified
        /// observable sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="control">The control on which to observe notifications.</param>
        /// <param name="source">The observable sequence of event notifications.</param>
        /// <param name="action">The action to invoke on each event notification.</param>
        /// <returns>
        /// A disposable object used to unsubscribe from the observable sequence.
        /// </returns>
        public static IDisposable SubscribeTo<TSource>(this Control control, IObservable<TSource> source, Action<TSource> action)
        {
            var handleCreated = Observable.FromEventPattern<EventHandler, EventArgs>(
                handler => control.HandleCreated += handler,
                handler => control.HandleCreated -= handler);
            var handleDestroyed = Observable.FromEventPattern<EventHandler, EventArgs>(
                handler => control.HandleDestroyed += handler,
                handler => control.HandleDestroyed -= handler);
            var notifications = handleCreated
                .SelectMany(_ => source.ObserveOn(control))
                .TakeUntil(handleDestroyed);
            return notifications.Subscribe(action);
        }

        internal static void SubscribeTo(this Control control, ExpressionBuilder builder)
        {
            if (string.IsNullOrEmpty(control.Name))
            {
                control.Name = ExpressionBuilder.GetElementDisplayName(builder);
            }

            if (builder is ControlBuilderBase controlBuilder)
            {
                control.SubscribeTo(controlBuilder._Enabled, value => control.Enabled = value);
                control.SubscribeTo(controlBuilder._Visible, value => control.Visible = value);
                control.SubscribeTo(controlBuilder._Font, value => control.Font = value);
            }

            if (builder is TextControlBuilderBase textControlBuilder)
            {
                control.SubscribeTo(textControlBuilder._Text, value => control.Text = value);
            }
        }
    }
}
