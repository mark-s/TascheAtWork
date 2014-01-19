using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using TascheAtWork.Core.Infrastructure;

namespace TascheAtWork.Shell
{
    public class Bootstrapper : UnityBootstrapper
    {

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        private void RegisterServices(IUnityContainer container)
        {
            
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
//            moduleCatalog.AddModule(typeof(Modules.InitModules));
        }

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        /// <remarks>
        /// If the returned instance is a <see cref="DependencyObject"/>, the
        /// <see cref="Bootstrapper"/> will attach the default <seealso cref="Microsoft.Practices.Composite.Regions.IRegionManager"/> of
        /// the application in its <see cref="Microsoft.Practices.Composite.Presentation.Regions.RegionManager.RegionManagerProperty"/> attached property
        /// in order to be able to add regions by using the <seealso cref="Microsoft.Practices.Composite.Presentation.Regions.RegionManager.RegionNameProperty"/>
        /// attached property from XAML.
        /// </remarks>
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            ServiceLocator.Container = Container;

            RegisterControlsAndControllers(Container);
            RegisterServices(Container);
        }

        private void RegisterControlsAndControllers(IUnityContainer container)
        {
            
        }
    }
}
