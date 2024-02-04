using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Bonsai.Design;
using Bonsai.Expressions;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides an abstract base class for visualizers representing individual UI elements.
    /// </summary>
    /// <typeparam name="TControl">The type of the control exposed by this type visualizer.</typeparam>
    /// <typeparam name="TControlBuilder">The type of the workflow element used to create the control.</typeparam>
    public abstract class ControlVisualizerBase<TControl, TControlBuilder>
        : DialogTypeVisualizer
        where TControl : Control
        where TControlBuilder : ExpressionBuilder
    {
        /// <summary>
        /// Gets the active control exposed by this type visualizer.
        /// </summary>
        [XmlIgnore]
        public TControl Control { get; private set; }

        /// <summary>
        /// Creates and configures the visual control associated with
        /// the specified workflow operator.
        /// </summary>
        /// <param name="provider">
        /// A service provider object which can be used to obtain visualization,
        /// runtime inspection, or other editing services.
        /// </param>
        /// <param name="builder">
        /// The <see cref="ExpressionBuilder"/> object used to provide configuration
        /// properties to the control.
        /// </param>
        /// <returns>
        /// A new instance of the control class associated with the specified
        /// workflow operator.
        /// </returns>
        protected abstract TControl CreateControl(IServiceProvider provider, TControlBuilder builder);

        /// <inheritdoc/>
        public override void Load(IServiceProvider provider)
        {
            var context = (ITypeVisualizerContext)provider.GetService(typeof(ITypeVisualizerContext));
            var controlBuilder = (TControlBuilder)ExpressionBuilder.GetVisualizerElement(context.Source).Builder;
            Control = CreateControl(provider, controlBuilder);
            Control.SubscribeTo(controlBuilder);

            var visualizerService = (IDialogTypeVisualizerService)provider.GetService(typeof(IDialogTypeVisualizerService));
            visualizerService?.AddControl(Control);
        }

        /// <inheritdoc/>
        public override void Show(object value)
        {
        }

        /// <inheritdoc/>
        public override void Unload()
        {
            Control.Dispose();
            Control = null;
        }
    }
}
