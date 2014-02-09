using Newtonsoft.Json;

namespace TascheAtWork.PocketAPI.Models.Response
{
  /// <summary>
  /// Modify Response
  /// </summary>
  [JsonObject]
  internal class ModifyResponse : ResponseBase
  {
    /// <summary>
    /// Gets or sets the action results.
    /// </summary>
    /// <value>
    /// The action results.
    /// </value>
    [JsonProperty("action_results")]
    public bool[] ActionResults { get; set; }
  }
}
