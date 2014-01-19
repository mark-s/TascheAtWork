using Newtonsoft.Json;

namespace TascheAtWork.PocketAPI.Models
{
  /// <summary>
  /// Tag
  /// </summary>
  [JsonObject]
  public class PocketTag
  {
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    [JsonProperty("tag")]
    public string Name { get; set; }
  }
}
