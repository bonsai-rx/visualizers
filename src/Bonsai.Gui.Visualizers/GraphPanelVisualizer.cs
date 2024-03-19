using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bonsai.Design;
using ZedGraph;

namespace Bonsai.Gui.Visualizers
{
    /// <summary>
    /// Provides a type visualizer that can be used to overlay multiple plots sharing
    /// the same axes in a single graph panel.
    /// </summary>
    public class GraphPanelVisualizer : MashupControlVisualizerBase<GraphControl, GraphPanelBuilder>
    {
        Type indexType;
        BarSettings barSettings;
        GraphPanelBuilder graphBuilder;

        static void ThrowIfNotEquals<T>(T left, T right, string message)
        {
            if (!EqualityComparer<T>.Default.Equals(left, right))
            {
                throw new InvalidOperationException(message);
            }
        }

        internal Axis BarBaseAxis()
        {
            return graphBuilder.BaseAxis switch
            {
                BarBase.Y => Control.GraphPane.YAxis,
                BarBase.Y2 => Control.GraphPane.Y2Axis,
                BarBase.X2 => Control.GraphPane.X2Axis,
                _ => Control.GraphPane.XAxis
            };
        }

        internal void EnsureIndex(Type type)
        {
            if (indexType == null)
            {
                indexType = type;
                var baseAxis = BarBaseAxis();
                if (type == typeof(string))
                {
                    GraphHelper.FormatOrdinalAxis(baseAxis, indexType);
                }
                if (type == typeof(XDate))
                {
                    GraphHelper.FormatLinearDateAxis(baseAxis);
                }
            }
            else ThrowIfNotEquals(indexType, type, "Only overlays with identical axis are allowed.");
        }

        internal void EnsureBarSettings(BarSettings settings)
        {
            if (barSettings == null)
            {
                barSettings = Control.GraphPane.BarSettings;
                barSettings.Base = settings.Base;
                barSettings.ClusterScaleWidth = settings.ClusterScaleWidth;
                barSettings.ClusterScaleWidthAuto = settings.ClusterScaleWidthAuto;
                barSettings.MinBarGap = settings.MinBarGap;
                barSettings.MinClusterGap = settings.MinClusterGap;
                barSettings.Type = settings.Type;
            }
            else
            {
                const string ErrorMessage = "All bar graph overlays must have identical settings.";
                ThrowIfNotEquals(barSettings.Base, settings.Base, ErrorMessage);
                ThrowIfNotEquals(barSettings.ClusterScaleWidth, settings.ClusterScaleWidth, ErrorMessage);
                ThrowIfNotEquals(barSettings.ClusterScaleWidthAuto, settings.ClusterScaleWidthAuto, ErrorMessage);
                ThrowIfNotEquals(barSettings.MinBarGap, settings.MinBarGap, ErrorMessage);
                ThrowIfNotEquals(barSettings.MinClusterGap, settings.MinClusterGap, ErrorMessage);
                ThrowIfNotEquals(barSettings.Type, settings.Type, ErrorMessage);
            }
        }

        /// <inheritdoc/>
        protected override GraphControl CreateControl(IServiceProvider provider, GraphPanelBuilder builder)
        {
            var graph = new GraphControl();
            graph.Dock = DockStyle.Fill;
            graphBuilder = builder;
            return graph;
        }

        /// <inheritdoc/>
        protected override void LoadMashupSource(int index, MashupSource mashupSource, IServiceProvider provider)
        {
            Control.ResetColorCycle();
            base.LoadMashupSource(index, mashupSource, provider);
        }

        /// <inheritdoc/>
        public override void Unload()
        {
            base.Unload();
            indexType = null;
            barSettings = null;
            graphBuilder = null;
        }
    }
}
