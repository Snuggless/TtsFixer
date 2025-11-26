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
internal sealed class CustomRuleRepository : IRuleRepository
{
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Local cache for rules to satisfy the synchronous interface.
    /// </summary>
    private readonly List<CustomRule> _cache = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomRuleRepository"/> class.
    /// Loads existing rules from IndexedDB into the local cache.
    /// </summary>
    /// <param name="jsRuntime">The JS runtime.</param>
    public CustomRuleRepository(IJSRuntime jsRuntime)
    {
        this._jsRuntime = jsRuntime;

        // Initialize cache from IndexedDB (fire-and-forget)
        _ = this.LoadAllAsync();
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

    private async Task LoadAllAsync()
    {
        try
        {
            var module = await this._jsRuntime.InvokeAsync<IJSObjectReference>("import", "./indexedDb.js");
            var items = await module.InvokeAsync<CustomRule []>("getAll");
            this._cache.Clear();
            if (items is not null && items.Length > 0)
            {
                this._cache.AddRange(items);
            }
        }
        catch (JSException)
        {
            // Ignore failures; cache stays as-is.
        }
    }

    private async Task UpsertAsync(CustomRule rule)
    {
        try
        {
            var module = await this._jsRuntime.InvokeAsync<IJSObjectReference>("import", "./indexedDb.js");
            await module.InvokeVoidAsync("upsert", rule);
        }
        catch (JSException)
        {
            // Swallow exceptions to keep sync API simple
        }
    }

    private async Task DeleteAsync(Guid id)
    {
        try
        {
            var module = await this._jsRuntime.InvokeAsync<IJSObjectReference>("import", "./indexedDb.js");
            await module.InvokeVoidAsync("remove", id);
        }
        catch (JSException)
        {
        }
    }
}
