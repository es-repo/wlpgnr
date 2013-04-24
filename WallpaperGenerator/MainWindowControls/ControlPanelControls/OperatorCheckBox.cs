using System.Windows.Controls;
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.MainWindowControls.ControlPanelControls
{
    public class OperatorCheckBox : CheckBox
    {
        public Operator Operator { get; private set; }

        public OperatorCheckBox(Operator @operator)
        {
            Operator = @operator;
            Content = Operator.Name;
        }
    }
}
