using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

_ = BenchmarkRunner
	.Run(
		assembly: typeof(Program).Assembly,
		config: DefaultConfig.Instance.AddJob(
			Job.ShortRun.WithCustomBuildConfiguration("Test")
		),
		args: args
	);
