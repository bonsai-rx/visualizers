using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Bonsai.Expressions;
using PropertyGrid = Bonsai.Design.PropertyGrid;

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer representing a property grid control.
    /// </summary>
    public class PropertyGridVisualizer : ControlVisualizerBase<PropertyGrid, PropertyGridBuilder>
    {
        /// <inheritdoc/>
        protected override PropertyGrid CreateControl(IServiceProvider provider, PropertyGridBuilder builder)
        {
            var handlerRegistered = false;
            var propertyGrid = new PropertyGrid();
            propertyGrid.Tag = builder;
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.Size = new Size(350, 450);
            propertyGrid.Site = new ServiceProviderContext(provider);
            propertyGrid.SubscribeTo(builder._HelpVisible, value => propertyGrid.HelpVisible = value);
            propertyGrid.SubscribeTo(builder._ToolbarVisible, value => propertyGrid.ToolbarVisible = value);
            propertyGrid.SubscribeTo(builder._RefreshProperties, value =>
            {
                if (value)
                {
                    propertyGrid.Refresh();
                    if (!handlerRegistered)
                    {
                        propertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
                        handlerRegistered = true;
                    }
                }
            });

            var workflowBuilder = (WorkflowBuilder)provider.GetService(typeof(WorkflowBuilder));
            propertyGrid.SelectedObject = GetExpressionContext(workflowBuilder.Workflow, builder);
            return propertyGrid;
        }

        static void PropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            var propertyGrid = (PropertyGrid)sender;
            var builder = (PropertyGridBuilder)propertyGrid.Tag;
            if (builder._RefreshProperties.Value)
            {
                propertyGrid.Refresh();
            }
        }

        static ExpressionBuilderGraph GetExpressionContext(ExpressionBuilderGraph source, ExpressionBuilder target)
        {
            foreach (var element in source.Elements())
            {
                if (element == target) return source;
                if (element is IWorkflowExpressionBuilder workflowBuilder)
                {
                    var nestedContext = GetExpressionContext(workflowBuilder.Workflow, target);
                    if (nestedContext != null)
                    {
                        return nestedContext;
                    }
                }
            }

            return null;
        }

        class ServiceProviderContext : ISite
        {
            readonly IServiceProvider parentProvider;

            public ServiceProviderContext(IServiceProvider provider)
            {
                parentProvider = provider;
            }

            public IComponent Component => null;

            public IContainer Container => null;

            public bool DesignMode => false;

            public string Name { get; set; }

            public object GetService(Type serviceType)
            {
                return parentProvider?.GetService(serviceType);
            }
        }
    }
}
