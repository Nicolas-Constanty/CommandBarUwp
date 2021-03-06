﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CommandBarUWP.CustomControl
{
    public sealed partial class Command : UserControl
    {
        public Command()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(FrameworkElement), typeof(Command), null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public readonly DependencyProperty SubCommandsProperty =
        DependencyProperty.Register("SubCommands", typeof(ObservableCollection<FrameworkElement>), typeof(Command),
            new PropertyMetadata(new ObservableCollection<FrameworkElement>()));

        public ObservableCollection<FrameworkElement> SubCommands
        {
            get { return (ObservableCollection<FrameworkElement>)GetValue(SubCommandsProperty); }
            set { SetValue(SubCommandsProperty, value); }
        }

        private EventRegistrationTokenTable<EventHandler<RoutedEventArgs>> m_RoutedEvent = null;
        public event EventHandler<RoutedEventArgs> ClickCommand
        {
            add { EventRegistrationTokenTable<EventHandler<RoutedEventArgs>>.GetOrCreateEventRegistrationTokenTable(ref m_RoutedEvent).AddEventHandler(value); }
            remove { EventRegistrationTokenTable<EventHandler<RoutedEventArgs>>.GetOrCreateEventRegistrationTokenTable(ref m_RoutedEvent).RemoveEventHandler(value); }
        }
        internal void OnClickCommand() {
            EventRegistrationTokenTable<EventHandler<RoutedEventArgs>>.GetOrCreateEventRegistrationTokenTable(ref m_RoutedEvent).InvocationList?.Invoke(this, new RoutedEventArgs());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnClickCommand();
        }
    }
}
