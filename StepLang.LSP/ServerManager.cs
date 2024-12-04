using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Server;

namespace StepLang.LSP;

public class ServerManager
{
    public async Task RunAsync(LanguageServerOptions options, CancellationToken cancellationToken)
    {
        IObserver<WorkDoneProgressReport> workDone = null!;

        var server = await LanguageServer.From(o => o
                .ConfigureLogging(builder => builder
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
                .OnInitialize((server, request, _) =>
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

                        return Task.CompletedTask;
                    }
                )
                .OnInitialized((_, _, _, _) =>
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
                        return Task.CompletedTask;
                    }
                )
                .OnStarted(
                    async (languageServer, ct) =>
                    {
                        using var manager = await languageServer.WorkDoneManager.Create(new()
                            {
                                Title = "Doing some work...",
                            }, cancellationToken: ct)
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

                        var logger = languageServer.Services.GetRequiredService<ILogger<ServerManager>>();
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
            , cancellationToken).ConfigureAwait(false);

        await server.WaitForExit.ConfigureAwait(false);
    }
}