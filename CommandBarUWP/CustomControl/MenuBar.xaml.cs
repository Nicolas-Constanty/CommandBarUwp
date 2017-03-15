using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CommandBarUWP.CustomControl
{
    public sealed partial class MenuBar : UserControl
    {
        private bool _expend = false;
        private Command _current = null;

        public MenuBar()
        {
            this.InitializeComponent();
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
        DependencyProperty.Register("CommandHeight", typeof(int), typeof(MenuBar), null);

        public int CommandHeight
        {
            get { return (int)GetValue(CommanHeightProperty); }
            set { SetValue(CommanHeightProperty, value); }
        }

        private void Expend()
        {
            SubCommands = _current.SubCommands;
            SubMenuPanel.Visibility = Visibility.Visible;
            _expend = true;
        }

        public static readonly DependencyProperty SubCommandsProperty =
            DependencyProperty.Register("SubCommands", typeof(ObservableCollection<FrameworkElement>), typeof(MenuBar), new PropertyMetadata(new ObservableCollection<FrameworkElement>()));

        public ObservableCollection<FrameworkElement> SubCommands
        {
            get { return (ObservableCollection<FrameworkElement>)GetValue(SubCommandsProperty); }
            set { SetValue(SubCommandsProperty, value); }
        }

        private void Collapse()
        {
            SubMenuPanel.Visibility = Visibility.Collapsed;
        }

        public void SelectMenu(Command cmd)
        {
            if (cmd == _current || cmd == null || _expend == true)
                return;
            _current = cmd;
           Expend();
        }
    }
}
