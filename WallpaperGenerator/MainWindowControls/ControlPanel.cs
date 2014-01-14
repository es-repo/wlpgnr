using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.UI.Core;
using WallpaperGenerator.UI.Windows.MainWindowControls.ControlPanelControls;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Windows.MainWindowControls
{
    public class ControlPanel : StackPanel
    {
        #region Porperties

        public Button GenerateFormulaButton { get; private set; }

        public Button ChangeColorButton { get; private set; }

        public Button TransformButton { get; private set; }

        public Button RenderFormulaButton { get; private set; }

        public Button StartStopSmoothAnimationButton { get; private set; }

        public Button SaveButton { get; private set; }

        public CheckBox RandomizeCheckBox { get; private set; }

        public Slider DimensionsCountSlider { get; private set; }

        public Slider MinimalDepthSlider { get; private set; }

        public Slider LeafProbabilitySlider { get; private set; }

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

            GenerateFormulaButton = CreateButton(panel, "Generate");
            TransformButton = CreateButton(panel, "Transform");
            ChangeColorButton = CreateButton(panel, "Change colors");
            RenderFormulaButton = CreateButton(panel, "Render");

            const string stopAnimationText = "Stop animation"; 
            const string animateSmoothlyText = "Animate smoothly";

            StartStopSmoothAnimationButton = CreateButton(panel, animateSmoothlyText);
            StartStopSmoothAnimationButton.Click +=
                (s, a) => StartStopSmoothAnimationButton.Content = StartStopSmoothAnimationButton.Content.ToString() == animateSmoothlyText ? stopAnimationText : animateSmoothlyText;

            SaveButton = CreateButton(panel, "Save");

            RandomizeCheckBox = new CheckBox { Content = "Randomize", Margin = new Thickness { Top = 10, Bottom = 5} };
            panel.Children.Add(RandomizeCheckBox);

            DimensionsCountSlider = CreateSliderControlsBlock(panel,1, 100, 8, "Dimensions");

            MinimalDepthSlider = CreateSliderControlsBlock(panel,1, 100, 14, "Minimal depth");

            LeafProbabilitySlider = CreateSliderControlsBlock(panel, 0, 100, 20, "Leaf probability");

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

        private Button CreateButton(Panel panel, string text)
        {
            Button button = new Button
            {
                Content = text,
                Margin = new Thickness { Top = 10 },
            };
            panel.Children.Add(button);
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
                    p.Value.Select(op => new OperatorControl(op) )));
        }

        public void LoadState(FormulaRenderArgumentsGenerationParams generationParams)
        {
            DimensionsCountSlider.Value = generationParams.DimensionCountBounds.Low;
            MinimalDepthSlider.Value = generationParams.MinimalDepthBounds.Low;
            ConstantProbabilitySlider.Value = generationParams.ConstantProbabilityBounds.Low * 100;
            LeafProbabilitySlider.Value = generationParams.LeafProbabilityBounds.Low*100;

            foreach (OperatorControl opCtrl in OperatorControls)
            {
                if (generationParams.OperatorAndMaxProbabilityMap.ContainsKey(opCtrl.Operator))
                {
                    opCtrl.Probability = generationParams.OperatorAndMaxProbabilityMap[opCtrl.Operator];
                    opCtrl.IsChecked = true;
                }
            }
        }

        public void SaveState(FormulaRenderArgumentsGenerationParams generationParams)
        {
            int dimensionsCount = (int)DimensionsCountSlider.Value;
            generationParams.DimensionCountBounds = new Bounds<int>(dimensionsCount, dimensionsCount);

            int minimalDepth = (int)MinimalDepthSlider.Value;
            generationParams.MinimalDepthBounds = new Bounds<int>(minimalDepth, minimalDepth);

            double constantProbability = ConstantProbabilitySlider.Value / 100;
            generationParams.ConstantProbabilityBounds = new Bounds(constantProbability, constantProbability);

            double leafProbability = LeafProbabilitySlider.Value / 100;
            generationParams.LeafProbabilityBounds = new Bounds(leafProbability, leafProbability);

            generationParams.OperatorAndMaxProbabilityMap = OperatorControls.Where(cb => cb.IsChecked).ToDictionary(c => c.Operator, c => c.Probability);
        }
    }
}
