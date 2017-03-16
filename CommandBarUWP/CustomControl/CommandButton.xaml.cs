using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CommandBarUWP.CustomControl
{
    public sealed partial class CommandButton : UserControl
    {
        public CommandButton()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(FrameworkElement), typeof(CommandButton), null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public readonly DependencyProperty IconeProperty =
        DependencyProperty.Register("Icone", typeof(FrameworkElement), typeof(CommandButton), null);

        public string Icone
        {
            get { return (string)GetValue(IconeProperty); }
            set { SetValue(IconeProperty, value); }
        }

        private EventRegistrationTokenTable<EventHandler<RoutedEventArgs>> m_RoutedEvent = null;
        public event EventHandler<RoutedEventArgs> Click
        {
            add { EventRegistrationTokenTable<EventHandler<RoutedEventArgs>>.GetOrCreateEventRegistrationTokenTable(ref m_RoutedEvent).AddEventHandler(value); }
            remove { EventRegistrationTokenTable<EventHandler<RoutedEventArgs>>.GetOrCreateEventRegistrationTokenTable(ref m_RoutedEvent).RemoveEventHandler(value); }
        }
        internal void OnClick()
        {
            EventRegistrationTokenTable<EventHandler<RoutedEventArgs>>.GetOrCreateEventRegistrationTokenTable(ref m_RoutedEvent).InvocationList?.Invoke(this, new RoutedEventArgs());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnClick();
        }
    }
}
