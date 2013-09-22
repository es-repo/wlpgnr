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

        public Button ChangeColorButton { get; private set; }

        public Button ChangeRangesButton { get; private set; }

        public Button RenderFormulaButton { get; private set; }

        public Slider DimensionsCountSlider { get; private set; }

        public Slider MinimalDepthSlider { get; private set; }

        public Slider VarOrConstantProbabilitySlider { get; private set; }

        public Slider ConstantProbabilitySlider { get; private set; }

        public IEnumerable<OperatorControl> OperatorControls { get; private set; }
    
        #endregion

        #region Constructors

        public ControlPanel()
        {
            Orientation = Orientation.Vertical;
            Children.Add(CreateButtonsAndSlidersPanel());
            Children.Add(CreateOperatorsPanel());
        }

        #endregion

        private Panel CreateButtonsAndSlidersPanel()
        {
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = 280
           };

            GenerateFormulaButton = CreateGenerateFormulaButton();
            panel.Children.Add(GenerateFormulaButton);

            ChangeRangesButton = CreateChangeRangesButton();
            panel.Children.Add(ChangeRangesButton);

            ChangeColorButton = CreateChangeColorButton();
            panel.Children.Add(ChangeColorButton);

            RenderFormulaButton = CreateRenderFormulaButton();
            panel.Children.Add(RenderFormulaButton);

            DimensionsCountSlider = CreateSliderControlsBlock(panel,1, 100, 8, "Dimensions");

            MinimalDepthSlider = CreateSliderControlsBlock(panel,1, 100, 14, "Minimal depth");

            VarOrConstantProbabilitySlider = CreateSliderControlsBlock(panel, 0, 100, 20, "Var or constant probability");

            ConstantProbabilitySlider = CreateSliderControlsBlock(panel, 0, 100, 20, "Constant probability");

            return panel;
        }

        private Panel CreateOperatorsPanel()
        {
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = 280,
                Margin = new Thickness { Left = 20 }  
            };
            
            IEnumerable<KeyValuePair<string, IEnumerable<OperatorControl>>> categoryAndOperatorControlsMap = CreateOperatorControls().ToArray();
            List<OperatorControl> operatorControls = new List<OperatorControl>();
            foreach (KeyValuePair<string, IEnumerable<OperatorControl>> entry in categoryAndOperatorControlsMap)
            {
                string category = entry.Key;
                TextBlock textBlock = new TextBlock
                {
                    Text = category,
                    FontWeight = FontWeight.FromOpenTypeWeight(999),
                    Margin = new Thickness { Top = 10 }
                };
                panel.Children.Add(textBlock);

                foreach (OperatorControl operatorControl in entry.Value)
                {
                    panel.Children.Add(operatorControl);
                    operatorControls.Add(operatorControl);
                }
            }

            OperatorControls = operatorControls;

            return panel;
        }

        private Button CreateGenerateFormulaButton()
        {
            Button button = new Button
            {
                Content = "Generate",
                Margin = new Thickness { Top = 10 },
            };

            return button;
        }

        private Button CreateChangeColorButton()
        {
            Button button = new Button
            {
                Content = "Change Colors",
                Margin = new Thickness { Top = 10 },
            };

            return button;
        }

        private Button CreateChangeRangesButton()
        {
            Button button = new Button
            {
                Content = "Change Ranges",
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

        private Slider CreateSliderControlsBlock(Panel parentPanel, int minimumValue, int maximumValue, int defaultValue, string label)
        {
            TextBlock labelTextBlock = new TextBlock 
            { 
                Text = label, 
                FontWeight = FontWeight.FromOpenTypeWeight(999),
                Margin = new Thickness { Right = 10 }               
            };
            SliderWithValueText sliderWithValueText = new SliderWithValueText(250, minimumValue, maximumValue, defaultValue);
            parentPanel.Children.Add(labelTextBlock);
            parentPanel.Children.Add(sliderWithValueText);
            return sliderWithValueText.Slider;
        }

        private static IEnumerable<KeyValuePair<string, IEnumerable<OperatorControl>>> CreateOperatorControls()
        {
            return OperatorsLibrary.AllByCategories.Select(p =>
                new KeyValuePair<string, IEnumerable<OperatorControl>>(
                    p.Key,
                    p.Value.Select(op => new OperatorControl(op) /*{ IsChecked = true }*/)));
        }
    }
}
