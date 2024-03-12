using System;
using System.Drawing;
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
        : MashupControlVisualizerBase<TControl, TControlBuilder>
        where TControl : Control
        where TControlBuilder : ExpressionBuilder
    {
        /// <summary>
        /// Adds a control to the container at the specified index.
        /// </summary>
        /// <param name="index">The index of the control, following the order of visualizer sources.</param>
        /// <param name="control">The control to add to the container.</param>
        protected abstract void AddControl(int index, Control control);

        /// <inheritdoc/>
        protected override void LoadMashupSource(int index, MashupSource mashupSource, IServiceProvider provider)
        {
            var containerProvider = new ContainerControlServiceProvider(index, this, mashupSource.Source, provider);
            mashupSource.Visualizer.Load(containerProvider);
        }

        /// <inheritdoc/>
        public override void UnloadMashups()
        {
            base.UnloadMashups();
            Control.Controls.Clear();
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

        class ContainerControlServiceProvider : MashupControlServiceProvider, IDialogTypeVisualizerService
        {
            public ContainerControlServiceProvider(
                int index,
                ContainerControlVisualizerBase<TControl, TControlBuilder> visualizer,
                InspectBuilder source,
                IServiceProvider provider)
                : base(index, visualizer, source, provider)
            {
            }

            public void AddControl(Control control)
            {
                var container = (ContainerControlVisualizerBase<TControl, TControlBuilder>)Visualizer;
                container.AddControl(Index, control);
            }

            public override object GetService(Type serviceType)
            {
                if (serviceType == typeof(IDialogTypeVisualizerService))
                {
                    return this;
                }

                return base.GetService(serviceType);
            }
        }
    }
}
