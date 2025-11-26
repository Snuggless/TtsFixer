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
    /// <summary>
    /// Represents the repository used to manage and retrieve custom rules.
    /// </summary>
    /// <remarks>This field is read-only and is intended to store an instance of <see
    /// cref="IRuleRepository"/>  for use within the containing class. It provides access to custom rule data and
    /// operations.</remarks>
    private readonly IRuleRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomRuleProvider"/> class with the specified repository.
    /// </summary>
    /// <param name="repository">The repository used to retrieve and manage custom rules. Cannot be <see langword="null"/>.</param>
    public CustomRuleProvider(IRuleRepository repository)
    {
        this._repository = repository;
    }

    /// <inheritdoc/>
    public int Order => 1;

    /// <inheritdoc/>
    public string Apply(string inputText)
    {
        var rules = this._repository.GetAsync()
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
