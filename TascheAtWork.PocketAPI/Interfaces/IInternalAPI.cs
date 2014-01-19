using System.Collections.Generic;
using TascheAtWork.PocketAPI.Models.Parameters;

namespace TascheAtWork.PocketAPI.Interfaces
{
    public interface IInternalAPI
    {
        T Request<T>(string method, Dictionary<string, string> parameters = null, bool requireAuth = true) where T : class, new();

        bool Send(List<ActionParameter> actionParameters);

        bool Send(ActionParameter actionParameter);

    }
}