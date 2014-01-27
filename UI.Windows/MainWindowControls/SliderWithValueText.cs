using System.Globalization;
using System.Windows.Controls;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Windows.MainWindowControls
{
    public class SliderWithValueText : StackPanel
    {
        private readonly TextBlock _textBlock;
        
        public Slider Slider { get; private set; }

        public SliderWithValueText(double width, double minimumValue, double maximumValue, double defaultValue)
        {
            Orientation = Orientation.Horizontal;
            Slider = new Slider
            {
                Width = width,
                Minimum = minimumValue,
                Maximum = maximumValue,
                Value = defaultValue,
                IsSnapToTickEnabled = true
            };

            _textBlock = new TextBlock {Text = Slider.Value.ToString(CultureInfo.InvariantCulture)};
            Slider.ValueChanged += (s, a) => _textBlock.Text = Slider.Value.ToInvariantString();

            Children.Add(Slider);
            Children.Add(_textBlock);
        }
    }
}
