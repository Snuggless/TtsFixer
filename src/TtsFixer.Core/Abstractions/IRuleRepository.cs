// <copyright file="IRuleRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TtsFixer.Core.Abstractions;

/// <summary>
/// Defines a repository for managing <see cref="CustomRule"/> objects, providing methods to retrieve, add, update, and
/// remove rules.
/// </summary>
/// <remarks>This interface is designed to support asynchronous operations for managing rules. Implementations of
/// this interface should ensure thread safety and proper handling of concurrent access.</remarks>
public interface IRuleRepository
{
    /// <summary>
    /// Retrieves a collection of custom rules.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, with a result of an <see cref="IEnumerable{T}"/> of <see cref="CustomRule"/> objects representing the custom rules.</returns>
    Task<IEnumerable<CustomRule>> GetAsync();

    /// <summary>
    /// Adds a custom rule to the current configuration.
    /// </summary>
    /// <param name="rule">The custom rule to add. Cannot be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CreateAsync(CustomRule rule);

    /// <summary>
    /// Asynchronously sets a custom rule for processing.
    /// </summary>
    /// <param name="rule">The custom rule to be applied. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(CustomRule rule);

    /// <summary>
    /// Removes the rule associated with the specified key.
    /// </summary>
    /// <remarks>If no rule is found with the specified key, the method completes without throwing an
    /// exception.</remarks>
    /// <param name="key">The unique identifier of the rule to be removed.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveAsync(Guid key);
}
