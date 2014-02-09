using Microsoft.Practices.Prism.Events;
using TascheAtWork.Core.Services;

namespace TascheAtWork.Core.Infrastructure
{
    /// <summary>
    /// Provides messaging services for the application.
    /// </summary>
    public class MessageAggregator : EventAggregator, IMessageAggregator
    {
        // This is an extension point for the Prism EventAggregator.  It allows
        // us to enhance the EventAggregator later if necessary without having
        // to change any calling code.
    }
}
