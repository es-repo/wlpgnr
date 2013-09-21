using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.MainWindowControls.ControlPanelControls
{
    public class OperatorControl : StackPanel
    {
        public readonly CheckBox _checkBox;
        private readonly Slider _probabilitySlider;

        public Operator Operator { get; private set; }

        public bool IsChecked
        {
            get { return _checkBox.IsChecked == true; }
            set { _checkBox.IsChecked = true; }
        }

        public double Probability 
        {
            get { return _probabilitySlider.Value / 100; }
        }

        public OperatorControl(Operator @operator)
        {
            Orientation = Orientation.Horizontal;
            Operator = @operator;
            _checkBox = new CheckBox { Content = Operator.Name, Width = 50};

            _probabilitySlider = new Slider
            {
                Minimum = 0,
                Maximum = 100,
                Value = 50,
                TickPlacement = TickPlacement.BottomRight,
                IsSnapToTickEnabled = true,
                Width =  150
            };

            Children.Add(_checkBox);
            Children.Add(_probabilitySlider);
        }
    }
}
