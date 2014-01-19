﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TascheAtWork.PocketAPI.Models.Parameters
{
  /// <summary>
  /// All parameters which can be passed to modify an item
  /// </summary>
  [DataContract]
  internal class ModifyParameters : Parameters
  {
    /// <summary>
    /// Gets or sets the actions.
    /// </summary>
    /// <value>
    /// The actions.
    /// </value>
    [DataMember(Name = "actions")]
    public List<Dictionary<string, string>> Actions { get; set; }
  }
}
