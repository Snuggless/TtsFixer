// <copyright file="CustomRuleRepository.cs" company="TtsFixer">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using TtsFixer.Core;
using TtsFixer.Core.Abstractions;

namespace TtsFixer.WebApp.Services;

/// <summary>
/// Provides functionality to manage a repository of custom rules, including retrieving, saving, and deleting rules.
/// </summary>
/// <remarks>This class is designed to handle operations related to custom rules, such as retrieving all rules,
/// saving new rules, and deleting existing rules by their unique identifier. It ensures encapsulation  of the rule
/// storage and provides a simple API for interacting with the rules.</remarks>
internal sealed class CustomRuleRepository : IRuleRepository
{
    /// <summary>
    /// Gets or sets the collection of custom rules used to define additional processing logic.
    /// </summary>
    private List<CustomRule> CustomRules { get; set; } = [];

    /// <inheritdoc />
    public IEnumerable<CustomRule> GetRules()
    {
        return this.CustomRules;
    }

    /// <inheritdoc />
    public void RemoveRule(Guid key)
    {
        this.CustomRules.RemoveAll(r => r.Identifier == key);
    }

    /// <inheritdoc />
    public void SetRule(CustomRule rule)
    {
        this.CustomRules.Add(rule);
    }
}
