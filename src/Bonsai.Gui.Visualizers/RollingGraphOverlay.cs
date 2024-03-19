using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive;
using Bonsai;
using Bonsai.Design;
using Bonsai.Gui.Visualizers;
using Bonsai.Expressions;
using ZedGraph;

[assembly: TypeVisualizer(typeof(RollingGraphOverlay), Target = typeof(MashupSource<GraphPanelVisualizer, RollingGraphVisualizer>))]


namespace Bonsai.Gui.Visualizers
{
    /// <summary>
    /// Provides a type visualizer used to overlay a sequence of values as a rolling graph.
    /// </summary>
    public class RollingGraphOverlay : BufferedVisualizer, IRollingGraphVisualizer
    {
        GraphPanelVisualizer visualizer;
        RollingGraphBuilder.VisualizerController controller;
        BoundedPointPairList[] series;

        void IRollingGraphVisualizer.AddValues(string index, params double[] values) => AddValues(0, index, values);

        void IRollingGraphVisualizer.AddValues(double index, params double[] values) => AddValues(index, null, values);

        void IRollingGraphVisualizer.AddValues(double index, string tag, params double[] values) => AddValues(index, null, values);

        internal void AddValues(double index, string tag, params double[] values)
        {
            for (int i = 0; i < series.Length; i++)
            {
                series[i].Add(index, values[i], index, tag);
            }
        }

        /// <inheritdoc/>
        public override void Load(IServiceProvider provider)
        {
            visualizer = (GraphPanelVisualizer)provider.GetService(typeof(MashupVisualizer));
            var context = (ITypeVisualizerContext)provider.GetService(typeof(ITypeVisualizerContext));
            var rollingGraphBuilder = (RollingGraphBuilder)ExpressionBuilder.GetVisualizerElement(context.Source).Builder;
            controller = rollingGraphBuilder.Controller;
            visualizer.EnsureIndex(controller.IndexType);

            var hasLabels = controller.ValueLabels != null;
            if (hasLabels)
            {
                series = new BoundedPointPairList[controller.ValueLabels.Length];
                for (int i = 0; i < series.Length; i++)
                {
                    series[i] = new BoundedPointPairList();
                    var curveSettings = controller.CurveSettings.Length > 0
                        ? controller.CurveSettings[i % controller.CurveSettings.Length]
                        : null;
                    var color = curveSettings?.Color.IsEmpty == false
                        ? curveSettings.Color
                        : visualizer.Control.GetNextColor();
                    var curve = CreateSeries(curveSettings?.Label ?? controller.ValueLabels[i], series[i], color);
                    visualizer.Control.GraphPane.CurveList.Add(curve);
                }
            }
        }

        private CurveItem CreateSeries(string label, IPointListEdit points, Color color)
        {
            var curve = new LineItem(label, points, color, controller.SymbolType, controller.LineWidth);
            curve.Line.IsAntiAlias = true;
            curve.Line.IsOptimizedDraw = true;
            curve.Label.IsVisible = !string.IsNullOrEmpty(label);
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Symbol.IsAntiAlias = true;
            return curve;
        }

        /// <inheritdoc/>
        protected override void ShowBuffer(IList<Timestamped<object>> values)
        {
            base.ShowBuffer(values);
            if (values.Count > 0)
            {
                visualizer.Control.Invalidate();
            }
        }

        /// <inheritdoc/>
        public override void Show(object value)
        {
            Show(DateTime.Now, value);
        }

        /// <inheritdoc/>
        protected override void Show(DateTime time, object value)
        {
            controller.AddValues(time, value, this);
        }

        /// <inheritdoc/>
        public override void Unload()
        {
            visualizer = null;
        }
    }
}
