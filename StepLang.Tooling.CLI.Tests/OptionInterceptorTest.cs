using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;
using StepLang.Tooling.CLI.Widgets;
using StepLang.Tooling.Meta;
using System.Runtime.InteropServices;

namespace StepLang.Tooling.CLI.Tests;

public class OptionInterceptorTest
{
	[Test]
	public void TestDoesNothingForNonGlobalCommandSettings()
	{
		// Arrange
		var consoleMock = new Mock<IAnsiConsole>();
		var metadataProviderMock = new Mock<IMetadataProvider>();
		var remainingArgumentsMock = new Mock<IRemainingArguments>();

		var context = new CommandContext([], remainingArgumentsMock.Object, "test", null);

		var interceptor = new OptionInterceptor(consoleMock.Object, metadataProviderMock.Object,
			new Dictionary<string, IMetadataProvider>());

		var settings = new EmptyCommandSettings();

		// Act
		interceptor.Intercept(context, settings);

		// Assert
		consoleMock.VerifyAll();
		metadataProviderMock.VerifyAll();
		remainingArgumentsMock.VerifyAll();
	}

	[Test]
	public void TestDoesNothingIfNoOptionsAreSet()
	{
		// Arrange
		var consoleMock = new Mock<IAnsiConsole>();
		var metadataProviderMock = new Mock<IMetadataProvider>();
		var remainingArgumentsMock = new Mock<IRemainingArguments>();

		var context = new CommandContext([], remainingArgumentsMock.Object, "test", null);

		var interceptor = new OptionInterceptor(consoleMock.Object, metadataProviderMock.Object,
			new Dictionary<string, IMetadataProvider>());

		var settings = new EmptyGlobalCommandSettings { Info = false, Version = false };

		// Act
		interceptor.Intercept(context, settings);

		// Assert
		Assert.That(settings.Handled, Is.False);

		consoleMock.VerifyAll();
		metadataProviderMock.VerifyAll();
		remainingArgumentsMock.VerifyAll();
	}

	[Test]
	public void TestPrintsVersionIfVersionOptionIsSet()
	{
		// Arrange
		const string fullSemVer = "fullSemVer";

		var consoleMock = new Mock<IAnsiConsole>();
		var metadataProviderMock = new Mock<IMetadataProvider>();
		var remainingArgumentsMock = new Mock<IRemainingArguments>();

		metadataProviderMock
			.Setup(g => g.FullSemVer)
			.Returns(fullSemVer)
			.Verifiable();

		IRenderable? writtenWidget = null;
		consoleMock
			.Setup(c => c.Write(It.IsAny<IRenderable>()))
			.Callback<IRenderable>(t => writtenWidget = t)
			.Verifiable();

		var context = new CommandContext([], remainingArgumentsMock.Object, "test", null);

		var interceptor = new OptionInterceptor(consoleMock.Object, metadataProviderMock.Object,
			new Dictionary<string, IMetadataProvider>());

		var settings = new EmptyGlobalCommandSettings { Info = false, Version = true };

		// Act
		interceptor.Intercept(context, settings);

		// Assert
		using (Assert.EnterMultipleScope())
		{
			Assert.That(settings.Handled, Is.True);
			Assert.That(writtenWidget, Is.Not.Null);
		}

		var text = AssertIsType<Text>(writtenWidget);
		Assert.That(text.GetTextContent().TrimEnd(Environment.NewLine.ToCharArray()), Is.EqualTo(fullSemVer));

		consoleMock.VerifyAll();
		metadataProviderMock.VerifyAll();
		remainingArgumentsMock.VerifyAll();
	}

	[Test]
	public void TestPrintsInfoGridIfInfoOptionIsSet()
	{
		// Arrange
		const string metadataComponentName = "Test";
		var buildTimeUtc = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
		const string sha = "sha";
		const string fullSemVer = "fullSemVer";
		const string branchName = "branchName";

		var consoleMock = new Mock<IAnsiConsole>();
		var metadataProviderMock = new Mock<IMetadataProvider>();
		var remainingArgumentsMock = new Mock<IRemainingArguments>();

		metadataProviderMock
			.SetupGet(g => g.BuildTime)
			.Returns(buildTimeUtc)
			.Verifiable();

		metadataProviderMock
			.Setup(g => g.Sha)
			.Returns(sha)
			.Verifiable();

		metadataProviderMock
			.Setup(g => g.FullSemVer)
			.Returns(fullSemVer)
			.Verifiable();

		metadataProviderMock
			.Setup(g => g.BranchName)
			.Returns(branchName)
			.Verifiable();

		metadataProviderMock
			.Setup(b => b.BuildTime)
			.Returns(buildTimeUtc)
			.Verifiable();

		IRenderable? writtenWidget = null;
		consoleMock
			.Setup(c => c.Write(It.IsAny<IRenderable>()))
			.Callback<IRenderable>(t => writtenWidget = t)
			.Verifiable();

		var context = new CommandContext([], remainingArgumentsMock.Object, "test", null);

		var interceptor = new OptionInterceptor(consoleMock.Object, metadataProviderMock.Object,
			new Dictionary<string, IMetadataProvider> { { metadataComponentName, metadataProviderMock.Object } });

		var settings = new EmptyGlobalCommandSettings { Info = true, Version = false };

		// Act
		interceptor.Intercept(context, settings);

		// Assert
		using (Assert.EnterMultipleScope())
		{
			Assert.That(settings.Handled, Is.True);
			Assert.That(writtenWidget, Is.Not.Null);
		}

		var definitionList = AssertIsType<DefinitionList>(writtenWidget);
		Assert.That(definitionList.Items, Has.Count.EqualTo(2));

		var metadataComponentLabel = AssertIsType<Markup>(definitionList.Items[0].Label);
		Assert.That(metadataComponentLabel.GetTextContent(), Is.EqualTo(metadataComponentName));

		var metadataGrid = AssertIsType<Grid>(definitionList.Items[0].Definition);
		using (Assert.EnterMultipleScope())
		{
			Assert.That(metadataGrid.Columns, Has.Count.EqualTo(2));
			Assert.That(metadataGrid.Rows, Has.Count.EqualTo(3));
		}

		var buildDateRow = metadataGrid.Rows[0];
		var buildDateHeader = buildDateRow[0];
		var buildDateValue = buildDateRow[1];
		var buildDateHeaderText = AssertIsType<Text>(buildDateHeader);
		Assert.That(buildDateHeaderText.GetTextContent(), Is.EqualTo("Build Date"));
		var buildDateValueText = AssertIsType<Text>(buildDateValue);
		Assert.That(buildDateValueText.GetTextContent(), Is.EqualTo("2025-01-01 00:00:00 UTC"));

		var versionRow = metadataGrid.Rows[1];
		var versionHeader = versionRow[0];
		var versionValue = versionRow[1];
		var versionHeaderText = AssertIsType<Text>(versionHeader);
		Assert.That(versionHeaderText.GetTextContent(), Is.EqualTo("Version"));
		var versionValueText = AssertIsType<Text>(versionValue);
		Assert.That(versionValueText.GetTextContent(), Is.EqualTo($"{sha} ({fullSemVer})"));

		var branchRow = metadataGrid.Rows[2];
		var branchHeader = branchRow[0];
		var branchValue = branchRow[1];
		var branchHeaderText = AssertIsType<Text>(branchHeader);
		Assert.That(branchHeaderText.GetTextContent(), Is.EqualTo("Branch"));
		var branchValueText = AssertIsType<Text>(branchValue);
		Assert.That(branchValueText.GetTextContent(), Is.EqualTo(branchName));

		var environmentLabel = AssertIsType<Markup>(definitionList.Items[1].Label);
		Assert.That(environmentLabel.GetTextContent(), Is.EqualTo("Environment"));

		var environmentGrid = AssertIsType<Grid>(definitionList.Items[1].Definition);
		using (Assert.EnterMultipleScope())
		{
			Assert.That(environmentGrid.Columns, Has.Count.EqualTo(2));
			Assert.That(environmentGrid.Rows, Has.Count.EqualTo(2));
		}

		var clrVersionRow = environmentGrid.Rows[0];
		var clrVersionHeader = clrVersionRow[0];
		var clrVersionValue = clrVersionRow[1];
		var clrVersionHeaderText = AssertIsType<Text>(clrVersionHeader);
		Assert.That(clrVersionHeaderText.GetTextContent(), Is.EqualTo("CLR Version"));
		var clrVersionValueText = AssertIsType<Text>(clrVersionValue);
		Assert.That(clrVersionValueText.GetTextContent(), Is.EqualTo(Environment.Version.ToString()));

		var osVersionRow = environmentGrid.Rows[1];
		var osVersionHeader = osVersionRow[0];
		var osVersionValue = osVersionRow[1];
		var osVersionHeaderText = AssertIsType<Text>(osVersionHeader);
		Assert.That(osVersionHeaderText.GetTextContent(), Is.EqualTo("OS Version"));
		var osVersionValueText = AssertIsType<Text>(osVersionValue);
		Assert.That(osVersionValueText.GetTextContent(),
			Is.EqualTo($"{RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})"));

		consoleMock.VerifyAll();
		metadataProviderMock.VerifyAll();
		remainingArgumentsMock.VerifyAll();
	}

	private static T AssertIsType<T>(object? value)
	{
		Assert.That(value, Is.TypeOf<T>());
		return (T)value!;
	}
}

file sealed class EmptyGlobalCommandSettings : CommandSettings, IGlobalCommandSettings
{
	public required bool Info { get; init; }

	public required bool Version { get; init; }

	public bool Handled { get; set; }
}
