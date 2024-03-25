using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive;
using Bonsai;
using Bonsai.Design;
using Bonsai.Gui.Visualizers;
using Bonsai.Expressions;
using ZedGraph;

[assembly: TypeVisualizer(typeof(BarGraphOverlay), Target = typeof(MashupSource<RollingGraphPanelVisualizer, BarGraphVisualizer>))]


namespace Bonsai.Gui.Visualizers
{
    /// <summary>
    /// Provides a type visualizer used to overlay a sequence of values as a bar graph.
    /// </summary>
    public class BarGraphOverlay : BufferedVisualizer, IBarGraphVisualizer
    {
        RollingGraphPanelVisualizer visualizer;
        BarGraphBuilder.VisualizerController controller;
        BoundedPointPairList[] series;

        void IBarGraphVisualizer.AddValues(string index, params double[] values) => AddValues(0, index, values);

        void IBarGraphVisualizer.AddValues(double index, params double[] values) => AddValues(index, null, values);

        void IBarGraphVisualizer.AddValues(double index, string tag, params double[] values) => AddValues(index, tag, values);

        static int FindIndex(IPointListEdit series, string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                for (int i = 0; i < series.Count; i++)
                {
                    if (EqualityComparer<string>.Default.Equals(tag, (string)series[i].Tag))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        internal void AddValues(double index, string tag, params double[] values)
        {
            if (values.Length > 0)
            {
                var updateIndex = FindIndex(series[0], tag);
                if (updateIndex >= 0 && controller.BaseAxis <= BarBase.X2) UpdateLastBaseX();
                else if (updateIndex >= 0) UpdateLastBaseY();
                else if (controller.BaseAxis <= BarBase.X2) AddBaseX();
                else AddBaseY();

                void UpdateLastBaseX()
                {
                    for (int i = 0; i < series.Length; i++)
                        series[i][updateIndex].Y = values[i];
                }

                void UpdateLastBaseY()
                {
                    for (int i = 0; i < series.Length; i++)
                        series[i][updateIndex].X = values[i];
                }

                void AddBaseX()
                {
                    for (int i = 0; i < series.Length; i++)
                        series[i].Add(index, values[i], index, tag);
                }

                void AddBaseY()
                {
                    for (int i = 0; i < series.Length; i++)
                        series[i].Add(values[i], index, index, tag);
                }
            }
        }

        /// <inheritdoc/>
        public override void Load(IServiceProvider provider)
        {
            visualizer = (RollingGraphPanelVisualizer)provider.GetService(typeof(MashupVisualizer));
            var context = (ITypeVisualizerContext)provider.GetService(typeof(ITypeVisualizerContext));
            var barGraphBuilder = (BarGraphBuilder)ExpressionBuilder.GetVisualizerElement(context.Source).Builder;
            controller = barGraphBuilder.Controller;
            visualizer.EnsureBarSettings(new BarSettings(visualizer.Control.GraphPane)
            {
                Base = controller.BaseAxis,
                Type = controller.BarType
            });
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
            var curve = new BarItem(label, points, color);
            curve.Label.IsVisible = !string.IsNullOrEmpty(label);
            curve.Bar.Fill.Type = FillType.Solid;
            curve.Bar.Border.IsVisible = false;
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
