// <copyright file="CustomRuleRepository.cs" company="TtsFixer">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.JSInterop;
using TtsFixer.Core;
using TtsFixer.Core.Abstractions;

namespace TtsFixer.WebApp.Services;

/// <summary>
/// IndexedDB-backed implementation of <see cref="IRuleRepository"/> for Blazor WebAssembly using JS interop.
/// </summary>
internal sealed class CustomRuleRepository : IRuleRepository, IAsyncDisposable
{
    /// <summary>
    /// The JS runtime for interop calls.
    /// </summary>
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// The logger instance.
    /// </summary>
    private readonly ILogger<CustomRuleRepository> _logger;

    /// <summary>
    /// Local cache for rules to satisfy the synchronous interface.
    /// </summary>
    private readonly List<CustomRule> _cache = [];

    /// <summary>
    /// The JS object reference for IndexedDB operations.
    /// </summary>
    private IJSObjectReference? _jSObjectReference;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomRuleRepository"/> class.
    /// Loads existing rules from IndexedDB into the local cache.
    /// </summary>
    /// <param name="jsRuntime">The JS runtime.</param>
    /// <param name="logger">The logger instance.</param>
    public CustomRuleRepository(IJSRuntime jsRuntime, ILogger<CustomRuleRepository> logger)
    {
        this._jsRuntime = jsRuntime;
        this._logger = logger;

        _ = this.LoadAllAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return this._jSObjectReference is not null ? this._jSObjectReference.DisposeAsync() : ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public IEnumerable<CustomRule> GetRules()
    {
        return this._cache;
    }

    /// <inheritdoc />
    public void SetRule(CustomRule rule)
    {
        var existingIndex = this._cache.FindIndex(r => r.Identifier == rule.Identifier);
        if (existingIndex >= 0)
        {
            this._cache [existingIndex] = rule;
        }
        else
        {
            this._cache.Add(rule);
        }

        _ = this.UpsertAsync(rule);
    }

    /// <inheritdoc />
    public void RemoveRule(Guid key)
    {
        this._cache.RemoveAll(r => r.Identifier == key);
        _ = this.DeleteAsync(key);
    }

    private async Task EnsureModuleAsync()
    {
        this._jSObjectReference ??= await this._jsRuntime.InvokeAsync<IJSObjectReference>("import", "./indexedDb.js");
    }

    private async Task LoadAllAsync()
    {
        try
        {
            await this.EnsureModuleAsync();
            var items = await this._jSObjectReference!.InvokeAsync<CustomRule []>("getAll");
            this._cache.Clear();
            if (items is not null && items.Length > 0)
            {
                this._cache.AddRange(items);
            }
        }
        catch (JSException ex)
        {
            this._logger.LogError(ex, "Error loading rules");
        }
    }

    private async Task UpsertAsync(CustomRule rule)
    {
        try
        {
            await this._jSObjectReference!.InvokeVoidAsync("upsert", rule);
        }
        catch (JSException ex)
        {
            this._logger.LogError(ex, "Error upserting rule");
        }
    }

    private async Task DeleteAsync(Guid id)
    {
        try
        {
            await this._jSObjectReference!.InvokeVoidAsync("remove", id);
        }
        catch (JSException ex)
        {
            this._logger.LogError(ex, "Error deleting rule");
        }
    }
}
