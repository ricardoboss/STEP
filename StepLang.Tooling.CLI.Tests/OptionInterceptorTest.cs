using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;
using System.Runtime.InteropServices;

namespace StepLang.Tooling.CLI.Tests;

public class OptionInterceptorTest
{
	[Fact]
	public void TestDoesNothingForNonGlobalCommandSettings()
	{
		// Arrange
		var consoleMock = new Mock<IAnsiConsole>();
		var gitVersionProviderMock = new Mock<IGitVersionProvider>();
		var buildTimeProviderMock = new Mock<IBuildTimeProvider>();
		var remainingArgumentsMock = new Mock<IRemainingArguments>();

		var context = new CommandContext([], remainingArgumentsMock.Object, "test", null);

		var interceptor = new OptionInterceptor(consoleMock.Object, gitVersionProviderMock.Object, buildTimeProviderMock.Object);
		var settings = new EmptyCommandSettings();

		// Act
		interceptor.Intercept(context, settings);

		// Assert
		consoleMock.VerifyAll();
		gitVersionProviderMock.VerifyAll();
		buildTimeProviderMock.VerifyAll();
		remainingArgumentsMock.VerifyAll();
	}

	[Fact]
	public void TestDoesNothingIfNoOptionsAreSet()
	{
		// Arrange
		var consoleMock = new Mock<IAnsiConsole>();
		var gitVersionProviderMock = new Mock<IGitVersionProvider>();
		var buildTimeProviderMock = new Mock<IBuildTimeProvider>();
		var remainingArgumentsMock = new Mock<IRemainingArguments>();

		var context = new CommandContext([], remainingArgumentsMock.Object, "test", null);

		var interceptor = new OptionInterceptor(consoleMock.Object, gitVersionProviderMock.Object, buildTimeProviderMock.Object);
		var settings = new EmptyGlobalCommandSettings
		{
			Info = false,
			Version = false,
		};

		// Act
		interceptor.Intercept(context, settings);

		// Assert
		consoleMock.VerifyAll();
		gitVersionProviderMock.VerifyAll();
		buildTimeProviderMock.VerifyAll();
		remainingArgumentsMock.VerifyAll();
	}

	[Fact]
	public void TestPrintsVersionIfVersionOptionIsSet()
	{
		// Arrange
		const string fullSemVer = "fullSemVer";

		var consoleMock = new Mock<IAnsiConsole>();
		var gitVersionProviderMock = new Mock<IGitVersionProvider>();
		var buildTimeProviderMock = new Mock<IBuildTimeProvider>();
		var remainingArgumentsMock = new Mock<IRemainingArguments>();

		gitVersionProviderMock
			.Setup(g => g.FullSemVer)
			.Returns(fullSemVer)
			.Verifiable();

		IRenderable? writtenWidget = null;
		consoleMock
			.Setup(c => c.Write(It.IsAny<IRenderable>()))
			.Callback<IRenderable>(t => writtenWidget = t)
			.Verifiable();

		var context = new CommandContext([], remainingArgumentsMock.Object, "test", null);

		var interceptor = new OptionInterceptor(consoleMock.Object, gitVersionProviderMock.Object, buildTimeProviderMock.Object);
		var settings = new EmptyGlobalCommandSettings
		{
			Info = false,
			Version = true,
		};

		// Act
		interceptor.Intercept(context, settings);

		// Assert
		Assert.NotNull(writtenWidget);
		Assert.IsType<Text>(writtenWidget);
		Assert.Equal(fullSemVer, ((Text)writtenWidget).GetTextContent().TrimEnd(Environment.NewLine.ToCharArray()));

		consoleMock.VerifyAll();
		gitVersionProviderMock.VerifyAll();
		buildTimeProviderMock.VerifyAll();
		remainingArgumentsMock.VerifyAll();
	}

	[Fact]
	public void TestPrintsInfoGridIfInfoOptionIsSet()
	{
		// Arrange
		var buildTimeUtc = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
		const string sha = "sha";
		const string fullSemVer = "fullSemVer";
		const string branchName = "branchName";

		var consoleMock = new Mock<IAnsiConsole>();
		var gitVersionProviderMock = new Mock<IGitVersionProvider>();
		var buildTimeProviderMock = new Mock<IBuildTimeProvider>();
		var remainingArgumentsMock = new Mock<IRemainingArguments>();

		buildTimeProviderMock
			.SetupGet(g => g.BuildTimeUtc)
			.Returns(buildTimeUtc)
			.Verifiable();

		gitVersionProviderMock
			.Setup(g => g.Sha)
			.Returns(sha)
			.Verifiable();

		gitVersionProviderMock
			.Setup(g => g.FullSemVer)
			.Returns(fullSemVer)
			.Verifiable();

		gitVersionProviderMock
			.Setup(g => g.BranchName)
			.Returns(branchName)
			.Verifiable();

		buildTimeProviderMock
			.Setup(b => b.BuildTimeUtc)
			.Returns(buildTimeUtc)
			.Verifiable();

		IRenderable? writtenWidget = null;
		consoleMock
			.Setup(c => c.Write(It.IsAny<IRenderable>()))
			.Callback<IRenderable>(t => writtenWidget = t)
			.Verifiable();

		var context = new CommandContext([], remainingArgumentsMock.Object, "test", null);

		var interceptor = new OptionInterceptor(consoleMock.Object, gitVersionProviderMock.Object, buildTimeProviderMock.Object);
		var settings = new EmptyGlobalCommandSettings
		{
			Info = true,
			Version = false,
		};

		// Act
		interceptor.Intercept(context, settings);

		// Assert
		Assert.NotNull(writtenWidget);
		Assert.IsType<Grid>(writtenWidget);

		var grid = (Grid)writtenWidget;
		Assert.Equal(2, grid.Columns.Count);
		Assert.Equal(5, grid.Rows.Count);

		var buildDateRow = grid.Rows[0];
		var buildDateHeader = buildDateRow[0];
		var buildDateValue = buildDateRow[1];
		Assert.IsType<Text>(buildDateHeader);
		Assert.Equal("Build Date", ((Text)buildDateHeader).GetTextContent());
		Assert.IsType<Text>(buildDateValue);
		Assert.Equal("2025-01-01 00:00:00 UTC", ((Text)buildDateValue).GetTextContent());

		var versionRow = grid.Rows[1];
		var versionHeader = versionRow[0];
		var versionValue = versionRow[1];
		Assert.IsType<Text>(versionHeader);
		Assert.Equal("Version", ((Text)versionHeader).GetTextContent());
		Assert.IsType<Text>(versionValue);
		Assert.Equal($"{sha} ({fullSemVer})", ((Text)versionValue).GetTextContent());

		var branchRow = grid.Rows[2];
		var branchHeader = branchRow[0];
		var branchValue = branchRow[1];
		Assert.IsType<Text>(branchHeader);
		Assert.Equal("Branch", ((Text)branchHeader).GetTextContent());
		Assert.IsType<Text>(branchValue);
		Assert.Equal(branchName, ((Text)branchValue).GetTextContent());

		var clrVersionRow = grid.Rows[3];
		var clrVersionHeader = clrVersionRow[0];
		var clrVersionValue = clrVersionRow[1];
		Assert.IsType<Text>(clrVersionHeader);
		Assert.Equal("CLR Version", ((Text)clrVersionHeader).GetTextContent());
		Assert.IsType<Text>(clrVersionValue);
		Assert.Equal(Environment.Version.ToString(), ((Text)clrVersionValue).GetTextContent());

		var osVersionRow = grid.Rows[4];
		var osVersionHeader = osVersionRow[0];
		var osVersionValue = osVersionRow[1];
		Assert.IsType<Text>(osVersionHeader);
		Assert.Equal("OS Version", ((Text)osVersionHeader).GetTextContent());
		Assert.IsType<Text>(osVersionValue);
		Assert.Equal($"{RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})", ((Text)osVersionValue).GetTextContent());

		consoleMock.VerifyAll();
		gitVersionProviderMock.VerifyAll();
		buildTimeProviderMock.VerifyAll();
		remainingArgumentsMock.VerifyAll();
	}
}

file sealed class EmptyGlobalCommandSettings : CommandSettings, IGlobalCommandSettings
{
	public required bool Info { get; init; }

	public required bool Version { get; init; }
}
