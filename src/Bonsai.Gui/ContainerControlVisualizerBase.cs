using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using Bonsai.Design;
using Bonsai.Expressions;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides an abstract base class for visualizers representing UI elements which can
    /// contain other nested UI elements.
    /// </summary>
    /// <typeparam name="TControl">
    /// The type of the container control exposed by this type visualizer.
    /// </typeparam>
    /// <typeparam name="TControlBuilder">
    /// The type of the workflow element used to create the container control.
    /// </typeparam>
    public abstract class ContainerControlVisualizerBase<TControl, TControlBuilder>
        : MashupVisualizer
        where TControl : Control
        where TControlBuilder : ExpressionBuilder
    {
        /// <summary>
        /// Gets the active container control exposed by this type visualizer.
        /// </summary>
        protected TControl Control { get; private set; }

        /// <summary>
        /// Creates and configures the container control associated with
        /// the specified workflow operator.
        /// </summary>
        /// <param name="provider">
        /// A service provider object which can be used to obtain visualization,
        /// runtime inspection, or other editing services.
        /// </param>
        /// <param name="builder">
        /// The <see cref="ExpressionBuilder"/> object used to provide configuration
        /// properties to the container control.
        /// </param>
        /// <returns>
        /// A new instance of the container control class associated with the specified
        /// workflow operator.
        /// </returns>
        protected abstract TControl CreateControl(IServiceProvider provider, TControlBuilder builder);

        /// <summary>
        /// Adds a control to the container at the specified index.
        /// </summary>
        /// <param name="index">The index of the control, following the order of visualizer sources.</param>
        /// <param name="control">The control to add to the container.</param>
        protected abstract void AddControl(int index, Control control);

        /// <inheritdoc/>
        public override void Load(IServiceProvider provider)
        {
            var context = (ITypeVisualizerContext)provider.GetService(typeof(ITypeVisualizerContext));
            var controlBuilder = (TControlBuilder)ExpressionBuilder.GetVisualizerElement(context.Source).Builder;
            Control = CreateControl(provider, controlBuilder);

            var visualizerService = (IDialogTypeVisualizerService)provider.GetService(typeof(IDialogTypeVisualizerService));
            visualizerService?.AddControl(Control);
            base.Load(provider);
        }

        /// <inheritdoc/>
        public override void LoadMashups(IServiceProvider provider)
        {
            for (int i = 0; i < MashupSources.Count; i++)
            {
                var mashupSource = MashupSources[i];
                var containerProvider = new ContainerControlServiceProvider(i, this, mashupSource.Source, provider);
                mashupSource.Visualizer.Load(containerProvider);
            }
        }

        /// <inheritdoc/>
        public override MashupSource GetMashupSource(int x, int y)
        {
            if (Control == null) return null;
            var panelPoint = Control.PointToClient(new Point(x, y));
            var childControl = Control.GetChildAtPoint(panelPoint);
            if (childControl != null)
            {
                var index = Control.Controls.GetChildIndex(childControl);
                return MashupSources[index];
            }

            return null;
        }

        /// <inheritdoc/>
        public override void Show(object value)
        {
        }

        /// <inheritdoc/>
        public override IObservable<object> Visualize(IObservable<IObservable<object>> source, IServiceProvider provider)
        {
            return MashupSources.Select(xs => xs.Visualizer.Visualize(xs.Source.Output, provider)).Merge();
        }

        /// <inheritdoc/>
        public override void Unload()
        {
            base.Unload();
            Control.Dispose();
            Control = null;
        }

        class ContainerControlServiceProvider : IDialogTypeVisualizerService, ITypeVisualizerContext, IServiceProvider
        {
            public ContainerControlServiceProvider(
                int index,
                ContainerControlVisualizerBase<TControl, TControlBuilder> visualizer,
                InspectBuilder source,
                IServiceProvider provider)
            {
                Index = index;
                Visualizer = visualizer ?? throw new ArgumentNullException(nameof(visualizer));
                Source = source ?? throw new ArgumentNullException(nameof(source));
                Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            }

            public int Index { get; }

            public ContainerControlVisualizerBase<TControl, TControlBuilder> Visualizer { get; }

            public InspectBuilder Source { get; }

            public IServiceProvider Provider { get; }

            public void AddControl(Control control)
            {
                Visualizer.AddControl(Index, control);
            }

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IDialogTypeVisualizerService) ||
                    serviceType == typeof(ITypeVisualizerContext))
                {
                    return this;
                }

                if (serviceType == typeof(MashupVisualizer))
                {
                    return Visualizer;
                }

                return Provider.GetService(serviceType);
            }
        }
    }
}
