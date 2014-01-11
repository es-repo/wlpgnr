using System;
using System.Windows;
using System.Windows.Controls;

namespace WallpaperGenerator.UI.Windows.MainWindowControls
{
    public class StatusPanel : StackPanel
    {
        private TimeSpan _renderedTime;
        private readonly TextBlock _renderedTimeText;
        private readonly TextBlock _renderingProgressText;

        public TimeSpan RenderedTime
        {
            get
            {
                return _renderedTime;
            }
            set 
            { 
                _renderedTime = value;
                _renderedTimeText.Text = _renderedTime.ToString();
            }
        }

        public double RenderingProgress
        {
            set { _renderingProgressText.Text = value.ToString("P2"); }
        }

        public StatusPanel()
        {
            Orientation = Orientation.Vertical;
            _renderedTimeText = CreateTextBlockWithLabel("Rendered Time:");
            _renderingProgressText = CreateTextBlockWithLabel("Rendering:");
        }

        private TextBlock CreateTextBlockWithLabel(string label)
        {
            StackPanel panel = new StackPanel {Orientation = Orientation.Horizontal};
            TextBlock labelTextBlock = new TextBlock
            {
                Text = label,
                FontWeight = FontWeight.FromOpenTypeWeight(999)
            };
            panel.Children.Add(labelTextBlock);

            TextBlock textBlock = new TextBlock();
            panel.Children.Add(textBlock);
            Children.Add(panel);
            return textBlock;
        }
    }
}
