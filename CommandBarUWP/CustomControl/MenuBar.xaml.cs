using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CommandBarUWP.CustomControl
{
    public sealed partial class MenuBar : UserControl
    {
        private Command _current = null;
        private Storyboard _slideOut = null;
        private Storyboard _slideIn = null;
        private SolidColorBrush _activeBrush;
        private SolidColorBrush _baseBrush;

        public MenuBar()
        {
            this.InitializeComponent();
            _activeBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
            _baseBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
        }

        private DoubleAnimationUsingKeyFrames CreateYAnimation(double value, double time)
        {
            DoubleAnimationUsingKeyFrames animationKeyFrames = new DoubleAnimationUsingKeyFrames();

            var keyFrameStart = new SplineDoubleKeyFrame();
            keyFrameStart.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(time));
            keyFrameStart.Value = value;

            animationKeyFrames.KeyFrames.Add(keyFrameStart);

            Storyboard.SetTargetProperty(animationKeyFrames, "Y");
            Storyboard.SetTarget(animationKeyFrames, SlideInTransform);
            return animationKeyFrames;
        }

        private DoubleAnimationUsingKeyFrames CreateOpacityAnimation(double value, double time)
        {
            DoubleAnimationUsingKeyFrames animationKeyFramesOpacity = new DoubleAnimationUsingKeyFrames();

            var keyFrameStartOpacity = new SplineDoubleKeyFrame();
            keyFrameStartOpacity.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(time));
            keyFrameStartOpacity.Value = value;

            animationKeyFramesOpacity.KeyFrames.Add(keyFrameStartOpacity);

            Storyboard.SetTargetProperty(animationKeyFramesOpacity, "Opacity");
            Storyboard.SetTarget(animationKeyFramesOpacity, SubMenuPanel);

            return animationKeyFramesOpacity;
        }

        private void InitSlideIn()
        {
            _slideIn = new Storyboard();

            _slideIn.Children.Add(CreateYAnimation(0, 150));
            _slideIn.Children.Add(CreateOpacityAnimation(1, 200));

            _slideIn.Completed += SlideIn_Completed;
        }

        private void InitSlideOut()
        {
            _slideOut = new Storyboard();

            _slideOut.Children.Add(CreateYAnimation(-SubMenuPanel.ActualHeight, 200));
            _slideOut.Children.Add(CreateOpacityAnimation(0, 150));

            _slideOut.Completed += SlideOut_Completed;
        }

        private void SlideIn_Completed(object sender, object e)
        {
        }

        private void SlideOut_Completed(object sender, object e)
        {
            SubMenuPanel.Visibility = Visibility.Collapsed;
        }

        public static readonly DependencyProperty MainCommandsProperty =
        DependencyProperty.Register("MainCommands", typeof(ObservableCollection<FrameworkElement>), typeof(MenuBar), new PropertyMetadata(new ObservableCollection<FrameworkElement>()));

        public ObservableCollection<FrameworkElement> MainCommands
        {
            get { return (ObservableCollection<FrameworkElement>)GetValue(MainCommandsProperty); }
            set {
                SetValue(MainCommandsProperty, value);
            }
        }

        public static readonly DependencyProperty CommanHeightProperty =
        DependencyProperty.Register("CommandHeight", typeof(int), typeof(MenuBar), new PropertyMetadata(Double.NaN));

        public int CommandHeight
        {
            get { return (int)GetValue(CommanHeightProperty); }
            set { SetValue(CommanHeightProperty, value);}
        }

        private void Expend()
        {
            if (_current.SubCommands.Count == 0)
            {
                Collapse();
                return;
            }
            CurrentSubCommands = _current.SubCommands;
            SubMenuPanel.Visibility = Visibility.Visible;
            if (_slideIn == null)
                InitSlideIn();
            _slideIn.Begin();
        }

        public static readonly DependencyProperty SubCommandsProperty =
            DependencyProperty.Register("CurrentSubCommands", typeof(ObservableCollection<FrameworkElement>), typeof(MenuBar),
                new PropertyMetadata(new ObservableCollection<FrameworkElement>()));

        public ObservableCollection<FrameworkElement> CurrentSubCommands
        {
            get { return (ObservableCollection<FrameworkElement>)GetValue(SubCommandsProperty); }
            set { SetValue(SubCommandsProperty, value); }
        }

        private void Collapse()
        {
            if (_slideOut == null)
                InitSlideOut();
            _slideOut.Begin();
        }

        public void SelectMenu(Command cmd)
        {
            if (cmd == _current || cmd == null)
                return;
            if (_current != null)
                _current.Background = _baseBrush;
            _current = cmd;
            _current.Background = _activeBrush;
           Expend();
        }
    }
}
