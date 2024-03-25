using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive;
using Bonsai;
using Bonsai.Design;
using Bonsai.Gui.ZedGraph;
using Bonsai.Expressions;
using ZedGraph;

[assembly: TypeVisualizer(typeof(LineGraphOverlay), Target = typeof(MashupSource<GraphPanelVisualizer, LineGraphVisualizer>))]


namespace Bonsai.Gui.ZedGraph
{
    /// <summary>
    /// Provides a type visualizer used to overlay a sequence of points as a line graph.
    /// </summary>
    public class LineGraphOverlay : BufferedVisualizer, ILineGraphVisualizer
    {
        GraphPanelVisualizer visualizer;
        LineGraphBuilder.VisualizerController controller;
        BoundedPointPairList[] series;

        void ILineGraphVisualizer.AddValues(double index, params PointPair[] values)
        {
            for (int i = 0; i < series.Length; i++)
            {
                series[i].Add(values[i].X, values[i].Y, index, values[i].Tag);
            }
        }

        /// <inheritdoc/>
        public override void Load(IServiceProvider provider)
        {
            visualizer = (GraphPanelVisualizer)provider.GetService(typeof(MashupVisualizer));
            var context = (ITypeVisualizerContext)provider.GetService(typeof(ITypeVisualizerContext));
            var lineGraphBuilder = (LineGraphBuilder)ExpressionBuilder.GetVisualizerElement(context.Source).Builder;
            controller = lineGraphBuilder.Controller;
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
