// <copyright file="CustomRuleProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using TtsFixer.Core.Abstractions;
using TTSTextNormalization.Abstractions;

namespace TtsFixer.Core.Services;

/// <summary>
/// Provides a custom implementation of the <see cref="ITextNormalizationRule"/> interface,  defining a specific text
/// normalization rule to be applied in a text processing pipeline.
/// </summary>
/// <remarks>This class implements the <see cref="ITextNormalizationRule"/> interface, which requires  defining
/// the order of the rule and the logic for applying the rule to a given input text.</remarks>
public class CustomRuleProvider : ITextNormalizationRule
{
    /// <inheritdoc/>
    public int Order => 1;

    /// <summary>
    /// Gets or sets the repository used to manage and retrieve rules.
    /// </summary>
    private IRuleRepository? RuleRepository { get; set; }

    /// <summary>
    /// Initializes the instance with the specified rule repository.
    /// </summary>
    /// <param name="ruleRepository">The rule repository to associate with this instance. Cannot be null.</param>
    public void Initialize(IRuleRepository ruleRepository)
    {
        this.RuleRepository = ruleRepository;
    }

    /// <inheritdoc/>
    public string Apply(string inputText)
    {
        if (this.RuleRepository is null)
        {
            throw new InvalidOperationException("RuleRepository is not initialized.");
        }

        var rules = this.RuleRepository.GetAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult()
            .OrderBy(x => x.Prioity);

        var output = inputText;

        foreach (var rule in rules)
        {
            if (output is null)
            {
                break;
            }

            output = output.Replace(rule.Pattern, rule.Replacement, rule.CaseSensetive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
        }

        return output ?? string.Empty;
    }
}
