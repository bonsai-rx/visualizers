using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides an abstract base class for interfacing with combo box and list box controls.
    /// </summary>
    public abstract class ListControlBuilderBase : ControlBuilderBase<string>
    {
        internal readonly ObservableCollection<string> _Items = new();
        internal readonly BehaviorSubject<string> _SelectedItem = new(string.Empty);

        /// <summary>
        /// Gets the collection of items contained in the list control.
        /// </summary>
        [Description("The collection of items contained in the list control.")]
        public Collection<string> Items
        {
            get => _Items;
        }

        /// <summary>
        /// Gets or sets the currently selected item in the list control.
        /// </summary>
        [Description("The currently selected item in the list control.")]
        public string SelectedItem
        {
            get => _SelectedItem.Value;
            set => _SelectedItem.OnNext(value);
        }

        /// <summary>
        /// Generates an observable sequence of values containing the currently
        /// selected item in the list control whenever the selection changes.
        /// </summary>
        /// <returns>
        /// A sequence of <see cref="string"/> values representing the currently
        /// selected item in the list control.
        /// </returns>
        protected override IObservable<string> Generate()
        {
            return _SelectedItem;
        }
    }
}
