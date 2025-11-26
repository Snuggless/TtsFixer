// <copyright file="DownloadService.cs" company="TtsFixer">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.JSInterop;

namespace TtsFixer.WebApp.Services;

/// <summary>
/// Provides methods to initiate client-side downloads of text files using JavaScript interop in a Blazor application.
/// </summary>
/// <remarks>This service enables .NET code to trigger file downloads in the user's browser by leveraging the
/// JavaScript runtime. It is typically used in Blazor applications to allow users to download dynamically generated
/// content without requiring a server round-trip.</remarks>
internal sealed class DownloadService
{
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Initializes a new instance of the <see cref="DownloadService"/> class using the specified JavaScript runtime.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime interface used to invoke JavaScript functions from .NET code. Cannot be null.</param>
    public DownloadService(IJSRuntime jsRuntime)
    {
        this._jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Initiates a client-side download of a text file with the specified name and content.
    /// </summary>
    /// <remarks>This method uses JavaScript interop to trigger the download in the user's browser. The file
    /// will be saved with the specified name and content on the client side.</remarks>
    /// <param name="fileName">The name of the file to be downloaded. Cannot be null or empty.</param>
    /// <param name="content">The text content to include in the downloaded file. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous download operation.</returns>
    public async Task DownloadAsync(string fileName, string content)
    {
        await this._jsRuntime.InvokeVoidAsync("downloadTextFile", fileName, content);
    }
}
