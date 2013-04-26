using System;
using System.Windows.Controls;
using System.Windows;

namespace WallpaperGenerator.MainWindowControls
{
    public class StatusPanel : StackPanel
    {
        private TimeSpan _renderedTime;
        private readonly TextBlock _renderedTimeText;

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

        public StatusPanel()
        {
            Orientation = Orientation.Horizontal;

            TextBlock renderedTimeLabel = new TextBlock
            {
                Text = "Rendered Time:",
                FontWeight = FontWeight.FromOpenTypeWeight(999)
            };
            Children.Add(renderedTimeLabel);

            _renderedTimeText = new TextBlock();
            Children.Add(_renderedTimeText);
        }
    }
}
