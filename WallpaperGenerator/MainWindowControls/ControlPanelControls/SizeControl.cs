using System.Windows.Controls;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Windows.MainWindowControls.ControlPanelControls
{
    public class SizeControl : StackPanel
    {
        private readonly TextBox _widthTextBox; 
        private readonly TextBox _heightTextBox;

        public Size Size
        {
            get { return new Size(int.Parse(_widthTextBox.Text), int.Parse(_heightTextBox.Text)); }
            set
            {
                _widthTextBox.Text = value.Width.ToInvariantString();
                _heightTextBox.Text = value.Height.ToInvariantString();
            }
        }

        public SizeControl()
        {
            Orientation = Orientation.Horizontal;
            _widthTextBox = new TextBox { Width = 50, Height = 20};
            Children.Add(_widthTextBox);
            _heightTextBox = new TextBox { Width = 50, Height = 20 };
            Children.Add(_heightTextBox);
            Size = new Size();
        }
    }
}
