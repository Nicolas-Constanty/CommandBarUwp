using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CommandBarUWP.CustomControl
{
    public sealed partial class MenuBar : UserControl
    {
        private bool _expend = false;
        private Command _current = null;
        private Storyboard SlideOut = null;
        private Storyboard SlideIn = null;

        public MenuBar()
        {
            this.InitializeComponent();
            
        }

        private void InitSlideIn()
        {
            SlideIn = new Storyboard();
            DoubleAnimationUsingKeyFrames animationKeyFrames = new DoubleAnimationUsingKeyFrames();

            var keyFrameStart = new SplineDoubleKeyFrame();
            keyFrameStart.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(150));
            keyFrameStart.Value = 0;

            animationKeyFrames.KeyFrames.Add(keyFrameStart);

            Storyboard.SetTargetProperty(animationKeyFrames, "Y");
            Storyboard.SetTarget(animationKeyFrames, SlideInTransform);

            DoubleAnimationUsingKeyFrames animationKeyFramesOpacity = new DoubleAnimationUsingKeyFrames();

            var keyFrameStartOpacity = new SplineDoubleKeyFrame();
            keyFrameStartOpacity.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200));
            keyFrameStartOpacity.Value = 1;

            animationKeyFramesOpacity.KeyFrames.Add(keyFrameStartOpacity);

            Storyboard.SetTargetProperty(animationKeyFramesOpacity, "Opacity");
            Storyboard.SetTarget(animationKeyFramesOpacity, SubMenuPanel);

            SlideIn.Children.Add(animationKeyFrames);
            SlideIn.Children.Add(animationKeyFramesOpacity);

            SlideIn.Completed += SlideIn_Completed;
        }

        private void InitSlideOut()
        {
            SlideOut = new Storyboard();
            DoubleAnimationUsingKeyFrames animationKeyFrames = new DoubleAnimationUsingKeyFrames();

            var keyFrameStart = new SplineDoubleKeyFrame();
            keyFrameStart.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200));
            keyFrameStart.Value = -SubMenuPanel.ActualHeight;

            animationKeyFrames.KeyFrames.Add(keyFrameStart);

            Storyboard.SetTargetProperty(animationKeyFrames, "Y");
            Storyboard.SetTarget(animationKeyFrames, SlideInTransform);

            DoubleAnimationUsingKeyFrames animationKeyFramesOpacity = new DoubleAnimationUsingKeyFrames();

            var keyFrameStartOpacity = new SplineDoubleKeyFrame();
            keyFrameStartOpacity.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(150));
            keyFrameStartOpacity.Value = 0;

            animationKeyFramesOpacity.KeyFrames.Add(keyFrameStartOpacity);

            Storyboard.SetTargetProperty(animationKeyFramesOpacity, "Opacity");
            Storyboard.SetTarget(animationKeyFramesOpacity, SubMenuPanel);

            SlideOut.Children.Add(animationKeyFrames);
            SlideOut.Children.Add(animationKeyFramesOpacity);

            SlideOut.Completed += SlideOut_Completed;
        }

        private void SlideIn_Completed(object sender, object e)
        {
            _expend = true;
        }

        private void SlideOut_Completed(object sender, object e)
        {
            SubMenuPanel.Visibility = Visibility.Collapsed;
            _expend = false;
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
            if (SlideIn == null)
                InitSlideIn();
            SlideIn.Begin();
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
            if (SlideOut == null)
                InitSlideOut();
            SlideOut.Begin();
        }

        public void SelectMenu(Command cmd)
        {
            if (cmd == _current || cmd == null)
                return;
            _current = cmd;
           Expend();
        }
    }
}
