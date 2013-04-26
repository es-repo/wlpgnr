using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.MainWindowControls.ControlPanelControls;

namespace WallpaperGenerator.MainWindowControls
{
    public class ControlPanel : StackPanel
    {
        #region Porperties

        public Button GenerateFormulaButton { get; private set; }

        public Button RenderFormulaButton { get; private set; }

        public IEnumerable<OperatorCheckBox> OperatorCheckBoxes { get; private set; }
    
        #endregion

        #region Constructors

        public ControlPanel()
        {
            Orientation = Orientation.Vertical;
            VerticalAlignment = VerticalAlignment.Stretch;
            Width = 200;

            GenerateFormulaButton = CreateGenerateFormulaButton();

            RenderFormulaButton = CreateRenderFormulaButton();

            Children.Add(GenerateFormulaButton);
            Children.Add(RenderFormulaButton);

            IEnumerable<KeyValuePair<string, IEnumerable<OperatorCheckBox>>> operatorCheckBoxesByCategories = CreateOperatorCheckBoxesByCategories().ToArray();
            foreach (KeyValuePair<string, IEnumerable<OperatorCheckBox>> entry in operatorCheckBoxesByCategories)
            {
                string category = entry.Key;
                TextBlock textBlock = new TextBlock
                    {
                        Text = category, 
                        FontWeight = FontWeight.FromOpenTypeWeight(999),
                        Margin = new Thickness { Top = 10 }
                    };
                Children.Add(textBlock);

                foreach (OperatorCheckBox checkBox in entry.Value)
                {
                    Children.Add(checkBox);
                }
            }

            OperatorCheckBoxes = Children.Cast<object>().OfType<OperatorCheckBox>();
        }

        #endregion

        private Button CreateGenerateFormulaButton()
        {
            Button button = new Button
            {
                Content = "Generate",
                Margin = new Thickness { Top = 10 },
            };

            return button;
        }

        private Button CreateRenderFormulaButton()
        {
            Button button = new Button
            {
                Content = "Render",
                Margin = new Thickness { Top = 10 },
            };
            
            return button;
        }

        private static IEnumerable<KeyValuePair<string, IEnumerable<OperatorCheckBox>>> CreateOperatorCheckBoxesByCategories()
        {
            return OperatorsLibrary.AllByCategories.Select(p =>
                new KeyValuePair<string, IEnumerable<OperatorCheckBox>>(
                    p.Key,
                    p.Value.Select(op => new OperatorCheckBox(op) {IsChecked = true})));
        }
    }
}
