using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Bonsai.Design;
using Bonsai.Expressions;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides an abstract base class for visualizers representing UI elements which can
    /// be combined with other visualizers.
    /// </summary>
    /// <typeparam name="TControl">
    /// The type of the control exposed by this mashup visualizer.
    /// </typeparam>
    /// <typeparam name="TControlBuilder">
    /// The type of the workflow element used to create the mashup control.
    /// </typeparam>
    public abstract class MashupControlVisualizerBase<TControl, TControlBuilder>
        : MashupVisualizer
        where TControl : Control
        where TControlBuilder : ExpressionBuilder
    {
        /// <summary>
        /// Gets the active mashup control exposed by this type visualizer.
        /// </summary>
        [XmlIgnore]
        public TControl Control { get; private set; }

        /// <summary>
        /// Creates and configures the mashup control associated with
        /// the specified workflow operator.
        /// </summary>
        /// <param name="provider">
        /// A service provider object which can be used to obtain visualization,
        /// runtime inspection, or other editing services.
        /// </param>
        /// <param name="builder">
        /// The <see cref="ExpressionBuilder"/> object used to provide configuration
        /// properties to the mashup control.
        /// </param>
        /// <returns>
        /// A new instance of the mashup control class associated with the specified
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
            base.Load(provider);
        }

        /// <inheritdoc/>
        public override void LoadMashups(IServiceProvider provider)
        {
            for (int i = 0; i < MashupSources.Count; i++)
            {
                var mashupSource = MashupSources[i];
                LoadMashupSource(i, mashupSource, provider);
            }
        }

        /// <summary>
        /// Loads type visualizer resources for an individual mashup source in the
        /// mashup visualizer.
        /// </summary>
        /// <param name="index">The zero-based index of the mashup source.</param>
        /// <param name="mashupSource">The mashup source for which to load visualizer resources.</param>
        /// <param name="provider">
        /// A service provider object which can be used to obtain visualization,
        /// runtime inspection, or other editing services.
        /// </param>
        protected virtual void LoadMashupSource(int index, MashupSource mashupSource, IServiceProvider provider)
        {
            var mashupServiceProvider = new MashupControlServiceProvider(index, this, mashupSource.Source, provider);
            mashupSource.Visualizer.Load(mashupServiceProvider);
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

        internal class MashupControlServiceProvider : ITypeVisualizerContext, IServiceProvider
        {
            public MashupControlServiceProvider(
                int index,
                MashupVisualizer visualizer,
                InspectBuilder source,
                IServiceProvider provider)
            {
                Index = index;
                Visualizer = visualizer ?? throw new ArgumentNullException(nameof(visualizer));
                Source = source ?? throw new ArgumentNullException(nameof(source));
                Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            }

            public int Index { get; }

            public MashupVisualizer Visualizer { get; }

            public InspectBuilder Source { get; }

            public IServiceProvider Provider { get; }

            public virtual object GetService(Type serviceType)
            {
                if (serviceType == typeof(ITypeVisualizerContext))
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
