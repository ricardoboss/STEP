using System.Reflection;
using HILFE.LSP.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Server;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File("lsp.log", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Verbose()
    .CreateLogger();

Log.Logger.Information("Starting up...");

IObserver<WorkDoneProgressReport> workDone = null!;

var server = await LanguageServer.From(options => options
    .ConfigureLogging(builder => builder
        .AddSerilog(Log.Logger)
        .AddLanguageProtocolLogging()
        .SetMinimumLevel(LogLevel.Trace))
    .WithInput(Console.OpenStandardInput())
    .WithOutput(Console.OpenStandardOutput())
    .WithServices(x => x.AddLogging(b => b.SetMinimumLevel(LogLevel.Trace)))
    .WithHandler<TextDocumentHandler>()
    .WithHandler<DidChangeWatchedFilesHandler>()
    .WithHandler<FoldingRangeHandler>()
    .WithHandler<MyWorkspaceSymbolsHandler>()
    .WithHandler<MyDocumentSymbolHandler>()
    .WithHandler<SemanticTokensHandler>()
    .WithServerInfo(new()
    {
        Name = "HILFE LSP Server",
        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
    })
    .OnInitialize(
        async (server, request, cancellationToken) =>
        {
            var manager = server.WorkDoneManager.For(
                request, new()
                {
                    Title = "Server is starting...",
                    Percentage = 10,
                }
            );

            workDone = manager;

            // await Task.Delay(200, cancellationToken).ConfigureAwait(false);

            manager.OnNext(
                new WorkDoneProgressReport
                {
                    Percentage = 20,
                    Message = "loading in progress",
                }
            );
        }
    )
    .OnInitialized(
        async (server, request, response, cancellationToken) =>
        {
            workDone.OnNext(
                new()
                {
                    Percentage = 40,
                    Message = "loading almost done",
                }
            );

            // await Task.Delay(200, cancellationToken).ConfigureAwait(false);

            workDone.OnNext(
                new()
                {
                    Message = "loading done",
                    Percentage = 100,
                }
            );

            workDone.OnCompleted();
        }
    )
    .OnStarted(
        async (languageServer, cancellationToken) =>
        {
            using var manager = await languageServer.WorkDoneManager.Create(new()
                {
                    Title = "Doing some work..."
                }, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            manager.OnNext(new WorkDoneProgressReport
            {
                Message = "doing things...",
            });

            // await Task.Delay(100).ConfigureAwait(false);
            manager.OnNext(new WorkDoneProgressReport
            {
                Message = "doing things... 1234",
            });

            // await Task.Delay(100, cancellationToken).ConfigureAwait(false);
            manager.OnNext(new WorkDoneProgressReport
            {
                Message = "doing things... 56789",
            });

            var logger = languageServer.Services.GetRequiredService<ILogger<Program>>();
            var configuration = await languageServer.Configuration.GetConfiguration(
                new ConfigurationItem
                {
                    Section = "typescript",
                }, new ConfigurationItem
                {
                    Section = "terminal",
                }
            ).ConfigureAwait(false);

            var baseConfig = new JObject();
            foreach (var config in languageServer.Configuration.AsEnumerable())
            {
                baseConfig.Add(config.Key, config.Value);
            }

            logger.LogInformation("Base Config: {@Config}", baseConfig);

            var scopedConfig = new JObject();
            foreach (var config in configuration.AsEnumerable())
            {
                scopedConfig.Add(config.Key, config.Value);
            }

            logger.LogInformation("Scoped Config: {@Config}", scopedConfig);
        }
    )
).ConfigureAwait(false);

Log.Logger.Information("Waiting for exit...");

await server.WaitForExit.ConfigureAwait(false);

Log.Logger.Information("Exiting...");
