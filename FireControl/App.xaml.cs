using FireControl.Infrastructure;
using System;
using System.Windows;
using FireControl.Infrastructure.Implementation;

namespace FireControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var diConfiguration = new FireControlUnityConfiguration();
            FireControlMappingConfiguration.CreateMappings();
            var windowDisplayService = diConfiguration.WindowDisplayService();

            try
            {
                windowDisplayService.ShowMainWindow();
            }
            catch (Exception ex)
            {
                windowDisplayService.ShowError(ex);
            }
        }
    }
}
