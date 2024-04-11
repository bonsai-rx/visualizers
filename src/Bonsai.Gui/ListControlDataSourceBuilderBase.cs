using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides an abstract base class for interfacing with combo box and list box controls
    /// bound to each data source in an observable sequence.
    /// </summary>
    public abstract class ListControlDataSourceBuilderBase : DataSourceControlBuilderBase
    {
        internal readonly BehaviorSubject<string> _DisplayMember = new(string.Empty);
        internal readonly BehaviorSubject<object> _DataSource = new(null);
        internal readonly BehaviorSubject<object> _SelectedItem = new(null);

        /// <summary>
        /// Gets or sets the property to display for this list control.
        /// </summary>
        [Editor(typeof(DataMemberSelectorEditor), typeof(UITypeEditor))]
        [Description("The property to display for this list control.")]
        public string DisplayMember
        {
            get => _DisplayMember.Value;
            set => _DisplayMember.OnNext(value);
        }

        /// <summary>
        /// Generates an observable sequence of values containing the currently
        /// selected item in the list control whenever the selection changes.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the values in the data source.
        /// </typeparam>
        /// <param name="source">
        /// A sequence of collections representing the data sources to bind to
        /// the list control. Only one collection is bound at any one time.
        /// </param>
        /// <returns>
        /// A sequence of values representing the currently selected item in
        /// the list control.
        /// </returns>
        protected override IObservable<TValue> Generate<TValue>(IObservable<IList<TValue>> source)
        {
            return Observable.Create<TValue>(observer =>
            {
                var sourceObserver = Observer.Create<IList<TValue>>(collection =>
                {
                    _SelectedItem.OnNext(null);
                    _DataSource.OnNext(collection);
                }, observer.OnError);
                var selectedItem = _SelectedItem.Where(value => value is TValue).Cast<TValue>();
                return new CompositeDisposable(
                    selectedItem.SubscribeSafe(observer),
                    source.SubscribeSafe(sourceObserver),
                    Disposable.Create(() =>
                    {
                        _DataSource.OnNext(null);
                        _SelectedItem.OnNext(null);
                    }));
            });
        }
    }
}
