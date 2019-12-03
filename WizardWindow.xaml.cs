using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Path = System.IO.Path;

namespace WpfApp
{


    public class PaintWizard : Wizard {
        public PaintWizard()
            :base()
        {
            PageChanged += PaintWizard_PageChangedAsync;
            Finish += PaintWizard_Finish;
            Help += PaintWizard_Help;
        }

        private void PaintWizard_Help(object sender, RoutedEventArgs e)
        {

            Xceed.Wpf.Toolkit.MessageBox.Show("Догадайтесь сами, хехе.", "Paint Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        private void PaintWizard_Finish(object sender, Xceed.Wpf.Toolkit.Core.CancelRoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();

        }

        private async void PaintWizard_PageChangedAsync(object sender, RoutedEventArgs e)
        {
            PaintWizard paintWizard = (PaintWizard)sender;

            WizardPage wizardPage = paintWizard.CurrentPage;
            Debug.WriteLine(wizardPage.Name);
            if (wizardPage.Name == "MiddlePage")
            {
                BusyIndicator busyIndicator = (BusyIndicator)wizardPage.Content;
                await EnableBusyIndicator(wizardPage, busyIndicator);
            }
        }

        private async Task EnableBusyIndicator(WizardPage page, BusyIndicator busyIndicator)
        {

            busyIndicator.IsBusy = true;
            TimeSpan timeSpan = new TimeSpan(0, 0, 10);
            await Task.Run(() => Thread.Sleep(timeSpan));
            busyIndicator.IsBusy = false;
            page.CanSelectNextPage = true;
            page.Title = "Потерпите еще чуть-чуть";
            page.Description = "Для окончания установки нажмите кнопку далее";
        }

    }

    /// <summary>
    /// Interaction logic for WizardWindow.xaml
    /// </summary>
    public partial class WizardWindow : Window
    {
        public WizardWindow()
        {
            InitializeComponent();
        }
    }
}
