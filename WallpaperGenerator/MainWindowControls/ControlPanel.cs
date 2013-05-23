using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.MainWindowControls.ControlPanelControls;

namespace WallpaperGenerator.MainWindowControls
{
    public class ControlPanel : StackPanel
    {
        #region Porperties

        public Button GenerateFormulaButton { get; private set; }

        public Button RenderFormulaButton { get; private set; }

        public Slider DimensionsCountSlider { get; private set; }

        public Slider VariablesCountSlider { get; private set; }

        public Slider ConstantsCountSlider { get; private set; }

        public Slider UnaryOperatorsCountSlider { get; private set; }

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

            DimensionsCountSlider = CreateSliderControlsBlock(1, 100, 8, "Dimensions");
            DimensionsCountSlider.ValueChanged += (s, a) =>
                VariablesCountSlider.Value = Math.Max(VariablesCountSlider.Value, DimensionsCountSlider.Value);
            
            VariablesCountSlider = CreateSliderControlsBlock(1, 500, 20, "Variables");
            
            ConstantsCountSlider = CreateSliderControlsBlock(0, 200, 4, "Constants");
            
            UnaryOperatorsCountSlider = CreateSliderControlsBlock(0, 1000, 40, "Unary Operators");
            
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

        private Slider CreateSliderControlsBlock(int minimumValue, int maximumValue, int defaultValue, string label)
        {
            TextBlock labelTextBlock = new TextBlock 
            { 
                Text = label, 
                FontWeight = FontWeight.FromOpenTypeWeight(999),
                Margin = new Thickness { Right = 10 }               
            };
            TextBlock valueTextBlock = new TextBlock();
            Slider slider = CreatSlider(minimumValue, maximumValue, defaultValue, valueTextBlock);
            StackPanel stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(labelTextBlock);
            stackPanel.Children.Add(valueTextBlock); 
            Children.Add(stackPanel);
            Children.Add(slider);
            return slider;
        }
        
        private static Slider CreatSlider(int minimumValue, int maximumValue, int defaultValue, TextBlock sliderValueLabel)
        {
            Slider slider = new Slider
            {
                Minimum = minimumValue,
                Maximum = maximumValue,
                Value = defaultValue,
                TickPlacement = TickPlacement.BottomRight,
                IsSnapToTickEnabled = true
            };

            sliderValueLabel.Text = slider.Value.ToString(CultureInfo.InvariantCulture);
            slider.ValueChanged += (s, a) => sliderValueLabel.Text = slider.Value.ToString(CultureInfo.InvariantCulture);

            return slider;
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
