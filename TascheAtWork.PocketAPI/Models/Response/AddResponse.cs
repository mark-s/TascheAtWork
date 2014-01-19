using Newtonsoft.Json;

namespace TascheAtWork.PocketAPI.Models.Response
{
  /// <summary>
  /// AddResponse Response
  /// </summary>
  [JsonObject]
  internal class AddResponse : ResponseBase
  {
    /// <summary>
    /// Gets or sets the item.
    /// </summary>
    /// <value>
    /// The item.
    /// </value>
    [JsonProperty]
    public PocketItem Item { get; set; }
  }
}
