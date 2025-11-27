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
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return this._jSObjectReference is not null ? this._jSObjectReference.DisposeAsync() : ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public IEnumerable<CustomRule> GetCustomRules()
    {
        return this._cache;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CustomRule>> GetAsync()
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

        return this._cache;
    }

    /// <inheritdoc />
    public async Task CreateAsync(CustomRule rule)
    {
        this._cache.Add(rule);

        try
        {
            await this.EnsureModuleAsync();
            await this._jSObjectReference!.InvokeVoidAsync("add", rule);
        }
        catch (JSException ex)
        {
            this._logger.LogError(ex, "Error adding rule");
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(CustomRule rule)
    {
        var existingIndex = this._cache.FindIndex(r => r.Identifier == rule.Identifier);
        this._cache [existingIndex] = rule;

        try
        {
            await this.EnsureModuleAsync();
            await this._jSObjectReference!.InvokeVoidAsync("update", rule);
        }
        catch (JSException ex)
        {
            this._logger.LogError(ex, "Error updating rule");
        }
    }

    /// <inheritdoc />
    public async Task RemoveAsync(Guid key)
    {
        try
        {
            this._cache.RemoveAll(r => r.Identifier == key);

            await this.EnsureModuleAsync();
            await this._jSObjectReference!.InvokeVoidAsync("remove", key);
        }
        catch (JSException ex)
        {
            this._logger.LogError(ex, "Error deleting rule");
        }
    }

    private async Task EnsureModuleAsync()
    {
        this._jSObjectReference ??= await this._jsRuntime.InvokeAsync<IJSObjectReference>("import", "./indexedDb.js");
    }
}
