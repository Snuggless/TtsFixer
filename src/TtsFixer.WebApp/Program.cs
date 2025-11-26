// <copyright file="Program.cs" company="TtsFixer">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TtsFixer.Core.Abstractions;
using TtsFixer.Core.Extensions;
using TtsFixer.WebApp.Components;
using TtsFixer.WebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddCoreServices();

builder.Services.AddScoped<IRuleRepository, CustomRuleRepository>();
builder.Services.AddScoped<DownloadService>();

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
