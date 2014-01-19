using Microsoft.Practices.Prism.Events;

namespace TascheAtWork.Core.Services
{
    /// <summary>
    /// Provides messaging services for the application.
    /// </summary>
    public interface IMessageAggregator : IEventAggregator
    {
        // This is an extension point for the Prism EventAggregator.  It allows
        // us to enhance the EventAggregator later if necessary without having
        // to change any calling code.
    }
}
