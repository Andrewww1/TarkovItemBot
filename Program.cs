﻿using Discord.Addons.Hosting;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Qmmands;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Threading.Tasks;
using TarkovItemBot.Options;
using TarkovItemBot.Services;
using TarkovItemBot.Services.Commands;
using TarkovItemBot.Services.TarkovDatabase;
using TarkovItemBot.Services.TarkovDatabaseSearch;
using TarkovItemBot.Services.TarkovTools;

namespace TarkovItemBot
{
    class Program
    {
        static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(theme: ConsoleTheme.None)
                .CreateLogger();

            var hostBuilder = Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddEnvironmentVariables("TarkovItemBot_");
                })
                .ConfigureDiscordHost((context, config) =>
                {
                    config.Token = context.Configuration["Bot:Token"];
                })
                .ConfigureServices((context, services) =>
                {
                    //Config
                    services.Configure<BotOptions>(context.Configuration.GetSection("Bot"));
                    services.Configure<TarkovDatabaseOptions>(context.Configuration.GetSection("TarkovDatabase"));
                    services.Configure<TarkovToolsOptions>(context.Configuration.GetSection("TarkovTools"));

                    // Cache
                    services.AddMemoryCache();

                    // Tarkov Database
                    services.AddHttpClient<TarkovDatabaseAuthClient>();

                    services.AddScoped<TarkovDatabaseTokenCache>();
                    services.AddTransient<TarkovDatabaseTokenHandler>();

                    // TODO: Ratelimit from config
                    services.AddHttpClient<TarkovDatabaseClient>().AddHttpMessageHandler(_ => new RateLimitHandler(500, TimeSpan.FromMinutes(1)))
                        .AddHttpMessageHandler<TarkovDatabaseTokenHandler>();

                    // Tarkov Database Search
                    services.AddHttpClient<TarkovSearchAuthClient>();

                    services.AddScoped<TarkovSearchTokenCache>();
                    services.AddTransient<TarkovSearchTokenHandler>();

                    // TODO: Ratelimit from config
                    services.AddHttpClient<TarkovSearchClient>().AddHttpMessageHandler(_ => new RateLimitHandler(100, TimeSpan.FromSeconds(10)))
                        .AddHttpMessageHandler<TarkovSearchTokenHandler>();

                    services.AddHttpClient<TarkovToolsClient>();

                    services.AddSingleton(new CommandService(new CommandServiceConfiguration()
                    {
                        DefaultRunMode = RunMode.Parallel,
                        CooldownBucketKeyGenerator = CooldownBucketKeyGenerators.DiscordDefault
                    }));

                    services.AddHostedService<CommandHandlingService>();
                    services.AddHostedService<PresenceService>();
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}
