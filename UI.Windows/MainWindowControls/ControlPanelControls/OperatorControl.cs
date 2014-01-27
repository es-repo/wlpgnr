using System.Windows.Controls;
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.UI.Windows.MainWindowControls.ControlPanelControls
{
    public class OperatorControl : StackPanel
    {
        public readonly CheckBox _checkBox;
        private readonly Slider _probabilitySlider;

        public Operator Operator { get; private set; }

        public bool IsChecked
        {
            get { return _checkBox.IsChecked == true; }
            set { _checkBox.IsChecked = value; }
        }

        public double Probability 
        {
            get { return _probabilitySlider.Value / 100; }
            set { _probabilitySlider.Value = value*100; }
        }

        public OperatorControl(Operator @operator)
        {
            Orientation = Orientation.Horizontal;
            Operator = @operator;
            _checkBox = new CheckBox { Content = Operator.Name, Width = 50};
            SliderWithValueText sliderWithValueText = new SliderWithValueText(190, 0, 100, 50);
            _probabilitySlider = sliderWithValueText.Slider;
            Children.Add(_checkBox);
            Children.Add(sliderWithValueText);
        }
    }
}
