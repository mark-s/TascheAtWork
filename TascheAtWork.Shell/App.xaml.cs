using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace TascheAtWork.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // The boostrapper will create the Shell instance, so the App.xaml does not have a StartupUri
            var bootStrapper = new Bootstrapper();
            bootStrapper.Run();
        }

    }
}
