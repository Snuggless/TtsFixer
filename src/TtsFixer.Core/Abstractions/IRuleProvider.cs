// <copyright file="IRuleProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TtsFixer.Core.Abstractions;

/// <summary>
/// Defines a contract for retrieving custom rules defined in the system.
/// </summary>
public interface IRuleProvider
{
    /// <summary>
    /// Retrieves a collection of custom rules currently defined in the system.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="CustomRule"/> objects representing the custom rules. The collection will
    /// be empty if no custom rules are defined.</returns>
    IEnumerable<CustomRule> GetCustomRules();
}
