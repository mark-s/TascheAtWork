﻿using System;

namespace TascheAtWork.PocketAPI
{
  /// <summary>
  /// custom Pocket API Exceptions
  /// </summary>
  public class PocketAPIException : Exception
  {
    /// <summary>
    /// Gets or sets the pocket error code.
    /// </summary>
    /// <value>
    /// The pocket error code.
    /// </value>
    public int? PocketErrorCode { get; set; }

    /// <summary>
    /// Gets or sets the pocket error.
    /// </summary>
    /// <value>
    /// The pocket error.
    /// </value>
    public string PocketError { get; set; }


    /// <summary>
    /// Initializes a new instance of the <see cref="PocketAPIException"/> class.
    /// </summary>
    public PocketAPIException() { }


    /// <summary>
    /// Initializes a new instance of the <see cref="PocketAPIException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PocketAPIException(string message): base(message) { }


    /// <summary>
    /// Initializes a new instance of the <see cref="PocketAPIException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public PocketAPIException(string message, Exception innerException) : base(message, innerException) { }
  }
}
