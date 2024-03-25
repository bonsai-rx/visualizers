using System.ComponentModel;
using System.Reactive.Subjects;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a property grid control for
    /// browsing the properties of the workflow in which it is inserted.
    /// </summary>
    [TypeVisualizer(typeof(PropertyGridVisualizer))]
    [Description("Interfaces with a property grid control for browsing the properties of the workflow in which it is inserted.")]
    public class PropertyGridBuilder : ControlBuilderBase
    {
        internal readonly BehaviorSubject<bool> _HelpVisible = new(true);
        internal readonly BehaviorSubject<bool> _ToolbarVisible = new(true);
        internal readonly BehaviorSubject<bool> _AutoRefresh = new(false);

        /// <summary>
        /// Gets or sets a value specifying whether the help text box is visible.
        /// </summary>
        [Category(nameof(CategoryAttribute.Appearance))]
        [Description("Specifies whether the help text box is visible.")]
        public bool HelpVisible
        {
            get => _HelpVisible.Value;
            set => _HelpVisible.OnNext(value);
        }

        /// <summary>
        /// Gets or sets a value specifying whether the toolbar is visible.
        /// </summary>
        [Category(nameof(CategoryAttribute.Appearance))]
        [Description("Specifies whether the toolbar is visible.")]
        public bool ToolbarVisible
        {
            get => _ToolbarVisible.Value;
            set => _ToolbarVisible.OnNext(value);
        }

        /// <summary>
        /// Gets or sets a value specifying whether the property grid should refresh
        /// whenever the value of any property changes.
        /// </summary>
        [Category(nameof(CategoryAttribute.Behavior))]
        [Description("Specifies whether the property grid should refresh whenever the value of any property changes.")]
        public bool AutoRefresh
        {
            get => _AutoRefresh.Value;
            set => _AutoRefresh.OnNext(value);
        }
    }
}
