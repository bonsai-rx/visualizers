using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;

namespace Bonsai.Gui.ZedGraph
{
    /// <summary>
    /// Represents common configuration properties for individual curves in a graph.
    /// </summary>
    public class CurveConfiguration
    {
        /// <summary>
        /// Gets or sets the label that will appear in the legend.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the color of the curve.
        /// </summary>
        [XmlIgnore]
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets an HTML representation of the curve color value for serialization.
        /// </summary>
        [Browsable(false)]
        [XmlElement(nameof(Color))]
        public string ColorHtml
        {
            get { return ColorTranslator.ToHtml(Color); }
            set { Color = ColorTranslator.FromHtml(value); }
        }
    }
}
