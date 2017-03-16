using CommandBarUWP.CustomControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CommandBarUWP
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Init();
        }

        private void Init()
        {
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = (Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush).Color;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = (Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush).Color;
            titleBar.ButtonForegroundColor = Colors.White;

            titleBar.InactiveBackgroundColor = (Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush).Color;
            titleBar.InactiveForegroundColor = Colors.White;

            titleBar.ButtonInactiveBackgroundColor = (Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush).Color;
        }

        private void Command_ClickCommand(object sender, RoutedEventArgs e)
        {
            CommandBar.SelectMenu((Command)sender);
        }

        private void ChangeLanguage(object sender, RoutedEventArgs e)
        {
            if (ApplicationLanguages.PrimaryLanguageOverride == "en")
                ApplicationLanguages.PrimaryLanguageOverride = "fr";
            else
                ApplicationLanguages.PrimaryLanguageOverride = "en";

            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();

            Reload();
        }

        private bool Reload(object param = null)
        {
            var type = Frame.CurrentSourcePageType;

            try
            {
                return Frame.Navigate(type, param);
            }
            finally
            {
                Frame.BackStack.Remove(Frame.BackStack.Last());
            }

        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
    }
}
