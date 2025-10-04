// <copyright file="IServiceCollectionExtensions.cs" company="TtsFixer">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using TtsFixer.Core.Services;
using TTSTextNormalization.DependencyInjection;
using TTSTextNormalization.Rules;

namespace TtsFixer.Core.Extensions;

/// <summary>
/// Provides extension methods for registering core services with an <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>This class contains methods that extend the functionality of <see cref="IServiceCollection"/>  to
/// simplify the registration of application-specific services.</remarks>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the core services required for the application to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the core services will be added. Cannot be <see langword="null"/>.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddAgashServices();

        return services;
    }

    /// <summary>
    /// Adds and configures services required for Agash functionality.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the Agash services will be added. Cannot be <see
    /// langword="null"/>.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
    private static IServiceCollection AddAgashServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.Configure<EmojiRuleOptions>(options =>
        {
            options.Suffix = "emoji";
        });

        services.AddTextNormalization(builder =>
        {
            builder.AddBasicSanitizationRule();
            builder.AddEmojiRule();
            builder.AddCurrencyRule();
            builder.AddAbbreviationNormalizationRule();
            builder.AddNumberNormalizationRule();
            builder.AddExcessivePunctuationRule();
            builder.AddLetterRepetitionRule();
            builder.AddUrlNormalizationRule();
            builder.AddWhitespaceNormalizationRule(orderOverride: 50);
            builder.AddRule<CustomRuleProvider>(ServiceLifetime.Singleton);
        });

        return services;
    }
}
