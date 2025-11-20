using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

_ = BenchmarkRunner
	.Run(
		assembly: typeof(Program).Assembly,
		config: DefaultConfig.Instance.AddJob(Job.Default.WithCustomBuildConfiguration("Test")),
		args: args
	);
