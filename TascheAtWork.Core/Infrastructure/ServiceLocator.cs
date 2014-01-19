using System;
using Microsoft.Practices.Unity;

namespace TascheAtWork.Core.Infrastructure
{
    /// <summary>
    /// Provides locator services for client code that 
    /// needs access to those services.
    /// </summary>
    public class ServiceLocator
    {
        /// <summary>
        /// Unity Container that can resolve required items.  
        /// This is set in the Bootstrapper
        /// </summary>
        /// <remarks>
        /// Make getter private so others don't try to use it 
        /// to register stuff.  Force them to resolve using 
        /// the Resolve method.
        /// Getter is internal to give access to testing code
        /// </remarks>
        public static IUnityContainer Container { internal get; set; }

        /// <summary>
        /// Create and return an instance of the class registered to T
        /// </summary>
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// Create and return an instance of the class registered to T
        /// that was registered with the given name
        /// </summary>
        public static T Resolve<T>(string pName)
        {
            return Container.Resolve<T>(pName);
        }

        /// <summary>
        /// Create and return an instance of the class registered to T
        /// </summary>
        public static object Resolve(Type T)
        {
            return Container.Resolve(T);
        }

        /// <summary>
        /// Register the given object with the proxy object T
        /// </summary>
        public static void RegisterInstance<T>(T pObject)
        {
            Container.RegisterInstance<T>(pObject);
        }

        /// <summary>
        /// Register the given object with the proxy object T
        /// and label it with the given occurrance name
        /// </summary>
        public static void RegisterInstance<T>(
          string pOccurranceName, T pObject)
        {
            Container.RegisterInstance<T>(pOccurranceName, pObject);
        }

        /// <summary>
        /// Register the given proxy type to create and return a class
        /// of the given type.
        /// </summary>
        public static void RegisterType<T, U>() where U : T
        {
            Container.RegisterType<T, U>();
        }

        /// <summary>
        /// Register the given proxy type to create and return a class
        /// of the given type.  Name this instance so that this particular
        /// one can be accessed by callers.
        /// </summary>
        public static void RegisterType<T, U>(LifetimeManager pLifetimeManager)
          where U : T
        {
            Container.RegisterType<T, U>(pLifetimeManager);
        }
    }
}
