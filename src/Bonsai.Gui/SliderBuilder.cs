using System;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace Bonsai.Gui
{
    /// <summary>
    /// Represents an operator that interfaces with a slider control and generates
    /// a sequence of notifications whenever the slider value changes.
    /// </summary>
    [DefaultProperty(nameof(Value))]
    [TypeVisualizer(typeof(SliderVisualizer))]
    [Description("Interfaces with a slider control and generates a sequence of notifications whenever the slider value changes.")]
    public class SliderBuilder : ControlBuilderBase<double>
    {
        internal readonly BehaviorSubject<double> _Minimum = new(0);
        internal readonly BehaviorSubject<double> _Maximum = new(100);
        internal readonly BehaviorSubject<int?> _DecimalPlaces = new(null);
        internal readonly BehaviorSubject<double> _Value = new(0);

        /// <summary>
        /// Gets or sets the lower limit of values in the slider.
        /// </summary>
        [Description("The lower limit of values in the slider.")]
        public double Minimum
        {
            get => _Minimum.Value;
            set => _Minimum.OnNext(value);
        }

        /// <summary>
        /// Gets or sets the upper limit of values in the slider.
        /// </summary>
        [Description("The upper limit of values in the slider.")]
        public double Maximum
        {
            get => _Maximum.Value;
            set => _Maximum.OnNext(value);
        }

        /// <summary>
        /// Gets or sets the optional maximum number of decimal places
        /// used when formatting the numeric display of the slider.
        /// </summary>
        [Category(nameof(CategoryAttribute.Appearance))]
        [Description("The optional maximum number of decimal places used when formatting the numeric display of the slider.")]
        public int? DecimalPlaces
        {
            get => _DecimalPlaces.Value;
            set => _DecimalPlaces.OnNext(value);
        }

        /// <summary>
        /// Gets or sets a numeric value which represents the position of the slider.
        /// </summary>
        [Description("The numeric value which represents the position of the slider.")]
        public double Value
        {
            get { return _Value.Value; }
            set { _Value.OnNext(value); }
        }

        /// <inheritdoc/>
        protected override IObservable<double> Generate()
        {
            return _Value;
        }
    }
}
