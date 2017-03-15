using System;
using System.Collections.ObjectModel;
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
            SubMenuPanel.Children.Clear();
            foreach (var cmd in _current.SubCommands)
            {
                Button btn = new Button();
                btn.Content = cmd;
                SubMenuPanel.Children.Add(btn);
            }
            SubMenuPanel.Visibility = Visibility.Visible;
            _expend = true;
        }

        private void Collapse()
        {
            SubMenuPanel.Visibility = Visibility.Collapsed;
        }

        class CommandHandler : ICommand
        {
            public event EventHandler CanExecuteChanged;

            private Action _action;

            public CommandHandler(Action action)
            {
                this._action = action;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                this._action();
            }
        }

        public ICommand SelectMenuCommand
        {
            get
            {
                return new CommandHandler(() => this.SelectMenu());
            }
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
