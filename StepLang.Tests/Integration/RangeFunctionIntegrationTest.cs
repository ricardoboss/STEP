using StepLang.Diagnostics;
using StepLang.Expressions;
using StepLang.Interpreting;
using StepLang.Tests;

namespace StepLang.Tests.Integration;

public class RangeFunctionIntegrationTest
{
        [Fact]
        public void RangeProducesAscendingSequence()
        {
                const string code = "println(range(0, 3))";
                var diagnostics = new DiagnosticCollection();
                var root = code.AsParsed(diagnostics);

                using var output = new StringWriter();
                var interpreter = new Interpreter(output, diagnostics: diagnostics);

                root.Accept(interpreter);

                Assert.Multiple(() =>
                {
                        Assert.Equal("[0, 1, 2, 3]" + Environment.NewLine, output.ToString(), ignoreLineEndingDifferences: true);
                        Assert.Empty(diagnostics);
                });
        }

        [Fact]
        public void RangeProducesFractionalSequence()
        {
                const string code = "println(range(0, 1, 0.1))";
                var diagnostics = new DiagnosticCollection();
                var root = code.AsParsed(diagnostics);

                using var output = new StringWriter();
                var interpreter = new Interpreter(output, diagnostics: diagnostics);

                root.Accept(interpreter);

                Assert.Multiple(() =>
                {
                        Assert.Equal("[0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1]" + Environment.NewLine,
                                output.ToString(), ignoreLineEndingDifferences: true);
                        Assert.Empty(diagnostics);
                });
        }

        [Fact]
        public void RangeProducesDescendingSequence()
        {
                const string code = "println(range(3, 0, -1))";
                var diagnostics = new DiagnosticCollection();
                var root = code.AsParsed(diagnostics);

                using var output = new StringWriter();
                var interpreter = new Interpreter(output, diagnostics: diagnostics);

                root.Accept(interpreter);

                Assert.Multiple(() =>
                {
                        Assert.Equal("[3, 2, 1, 0]" + Environment.NewLine, output.ToString(), ignoreLineEndingDifferences: true);
                        Assert.Empty(diagnostics);
                });
        }

        [Fact]
        public void RangeRejectsZeroStep()
        {
                const string code = "list invalid = range(0, 1, 0)";
                var diagnostics = new DiagnosticCollection();
                var root = code.AsParsed(diagnostics);

                var interpreter = new Interpreter(diagnostics: diagnostics);

                _ = Assert.Throws<InvalidArgumentValueException>(() => root.Accept(interpreter));
                Assert.Empty(diagnostics);
        }
}
