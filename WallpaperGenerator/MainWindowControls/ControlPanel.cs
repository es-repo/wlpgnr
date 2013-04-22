using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.MainWindowControls
{
    public class ControlPanel : StackPanel
    {
        public Button GenerateFormulaButton { get; private set; }

        public Button RenderFormulaButton { get; private set; }

        #region Constructors

        public ControlPanel()
        {
            Orientation = Orientation.Vertical;
            VerticalAlignment = VerticalAlignment.Stretch;
            Width = 200;

            GenerateFormulaButton = new Button
            {
                Content = "Generate formula",
                Margin = new Thickness { Top = 10 },
            };

            RenderFormulaButton = new Button
            {
                Content = "Render",
                Margin = new Thickness { Top = 10 },
            };

            Children.Add(GenerateFormulaButton);
            Children.Add(RenderFormulaButton);

            IEnumerable<KeyValuePair<string, IEnumerable<CheckBox>>> checkBoxes = CreateOperatorCheckBoxes();
            foreach (KeyValuePair<string, IEnumerable<CheckBox>> entry in checkBoxes)
            {
                string category = entry.Key;
                TextBlock textBlock = new TextBlock
                    {
                        Text = category, 
                        FontWeight = FontWeight.FromOpenTypeWeight(999),
                        Margin = new Thickness { Top = 10 }
                    };
                Children.Add(textBlock);

                foreach (CheckBox checkBox in entry.Value)
                {
                    Children.Add(checkBox);
                }
            }
        }

        #endregion

        private static IEnumerable<KeyValuePair<string, IEnumerable<CheckBox>>> CreateOperatorCheckBoxes()
        {
            return OperatorsAndConstantsByCategories.Select(p =>
                new KeyValuePair<string, IEnumerable<CheckBox>>(
                    p.Key, 
                    p.Value.Select(op => 
                        new CheckBox
                            {
                                Content = FormulaTreeSerializer.OperatorToString(op)
                            })));
        }

        private static IEnumerable<KeyValuePair<string, IEnumerable<Operator>>> OperatorsAndConstantsByCategories
        {
            get
            {
                return ConstantsLibrary.AllByCategories.Concat(OperatorsLibrary.AllByCategories);
            }
        }
    }
}
