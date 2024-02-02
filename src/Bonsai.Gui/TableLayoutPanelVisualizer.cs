using Bonsai;
using Bonsai.Design;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Bonsai.Gui;

[assembly: TypeVisualizer(typeof(DialogTypeVisualizer), Target = typeof(MashupSource<TableLayoutPanelVisualizer>))]

namespace Bonsai.Gui
{
    /// <summary>
    /// Provides a type visualizer that can be used to arrange other visualizers in a grid.
    /// </summary>
    public class TableLayoutPanelVisualizer : ContainerControlVisualizerBase<TableLayoutPanel, TableLayoutPanelBuilder>
    {
        TableLayoutPanelBuilder panelBuilder;

        static void SetStyles(TableLayoutStyleCollection styles, IReadOnlyList<TableLayoutStyle> builderStyles, int count, Func<TableLayoutStyle> defaultStyle)
        {
            if (builderStyles.Count > 0)
            {
                foreach (var style in builderStyles)
                {
                    styles.Add(style);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    styles.Add(defaultStyle());
                }
            }
        }

        void UpdateLayoutPanel()
        {
            var panel = Control;
            var columnCount = panelBuilder.ColumnCount;
            var rowCount = panelBuilder.RowCount;
            if (columnCount == 0 && rowCount == 0)
            {
                throw new InvalidOperationException("The table layout must have at least one non-zero dimension.");
            }
            if (columnCount == 0) columnCount = MashupSources.Count / rowCount;
            if (rowCount == 0) rowCount = MashupSources.Count / columnCount;

            panel.ColumnCount = columnCount;
            panel.RowCount = rowCount;
            SetStyles(panel.ColumnStyles, panelBuilder.ColumnStyles, columnCount, () => new ColumnStyle(SizeType.Percent, 100f / columnCount));
            SetStyles(panel.RowStyles, panelBuilder.RowStyles, rowCount, () => new RowStyle(SizeType.Percent, 100f / rowCount));
        }

        /// <inheritdoc/>
        protected override TableLayoutPanel CreateControl(IServiceProvider provider, TableLayoutPanelBuilder builder)
        {
            var panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.Size = new Size(320, 240);
            panel.Name = builder.Name;
            panelBuilder = builder;
            return panel;
        }

        /// <inheritdoc/>
        protected override void AddControl(int index, Control control)
        {
            int column, row;
            var panel = Control;
            if (panel.ColumnCount == 0)
            {
                column = index / panel.RowCount;
                row = index % panel.RowCount;
            }
            else
            {
                column = index % panel.ColumnCount;
                row = index / panel.ColumnCount;
            }
            panel.Controls.Add(control, column, row);
            if (index < panelBuilder.CellSpans.Count)
            {
                var cellSpan = panelBuilder.CellSpans[index];
                panel.SetColumnSpan(control, cellSpan.ColumnSpan);
                panel.SetRowSpan(control, cellSpan.RowSpan);
            }
        }

        /// <inheritdoc/>
        public override void LoadMashups(IServiceProvider provider)
        {
            UpdateLayoutPanel();
            base.LoadMashups(provider);
        }

        /// <inheritdoc/>
        public override void UnloadMashups()
        {
            base.UnloadMashups();
            Control.Controls.Clear();
            Control.RowStyles.Clear();
            Control.ColumnStyles.Clear();
        }

        /// <inheritdoc/>
        public override void Unload()
        {
            base.Unload();
            panelBuilder = null;
        }
    }
}
