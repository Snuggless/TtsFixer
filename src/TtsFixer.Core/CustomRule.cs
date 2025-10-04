// <copyright file="CustomRule.cs" company="TtsFixer">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace TtsFixer.Core;

/// <summary>
/// Represents a rule for processing words, including pattern matching and replacement functionality.
/// </summary>
/// <remarks>A <see cref="CustomRule"/> defines a configurable rule that can be used to match and optionally replace
/// words or text patterns. The rule supports both plain text and regular expression-based matching.</remarks>
public class CustomRule
{
    /// <summary>
    /// Gets or sets the unique identifier for the object.
    /// </summary>
    public Guid Identifier { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the priority level of the item.
    /// </summary>
    public int Prioity { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the name associated with the object.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the feature is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether regular expressions should be used for pattern matching.
    /// </summary>
    public bool UseRegex { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation is case-sensitive.
    /// </summary>
    public bool CaseSensetive { get; set; }

    /// <summary>
    /// Gets or sets the pattern used for matching or validation.
    /// </summary>
    [Required]
    public string Pattern { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the replacement string used to substitute matching patterns.
    /// </summary>
    [Required]
    public string Replacement { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
