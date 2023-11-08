using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class ParseCommand : AsyncCommand<ParseCommand.Settings>
{
    public sealed class Settings : HiddenGlobalCommandSettings
    {
        [CommandArgument(0, "<file>")]
        [Description("The path to a .step-file to run.")]
        public string File { get; init; } = null!;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var scriptFile = new FileInfo(settings.File);

        var chars = await File.ReadAllTextAsync(scriptFile.FullName);
        var tokenizer = new Tokenizer();
        tokenizer.UpdateFile(scriptFile);
        tokenizer.Add(chars);
        var tokens = tokenizer.Tokenize();
        var parser = new Parser(tokens);
        var root = parser.ParseRoot();

        var tree = new Tree("Root");
        RenderNode(tree, root);

        AnsiConsole.Write(tree);

        return 0;
    }

    private static void RenderNode(IHasTreeNodes parent, INode parserNode)
    {
        // TODO: rewrite this using visitor pattern
        switch (parserNode)
        {
            case RootNode root:
                {
                    foreach (var node in root.Body)
                        RenderNode(parent, node);

                    break;
                }
            case NullableVariableDeclarationNode nvdn: // must come before VariableDeclarationNode
                {
                    var treeNode = parent.AddNode("NullableVariableDeclaration:");
                    treeNode.AddNode("Types: " + string.Join("|", nvdn.Types).EscapeMarkup());
                    treeNode.AddNode("Nullability: " + nvdn.NullabilityIndicator.ToString().EscapeMarkup());
                    treeNode.AddNode("Identifier: " + nvdn.Identifier.ToString().EscapeMarkup());
                    break;
                }
            case VariableDeclarationNode vdn:
                {
                    var treeNode = parent.AddNode("VariableDeclaration:");
                    treeNode.AddNode("Types: " + string.Join("|", vdn.Types).EscapeMarkup());
                    treeNode.AddNode("Identifier: " + vdn.Identifier.ToString().EscapeMarkup());
                    break;
                }
            case VariableInitializationNode vin:
                {
                    var treeNode = parent.AddNode("VariableInitialization:");
                    treeNode.AddNode("Types: " + string.Join("|", vin.Types).EscapeMarkup());
                    treeNode.AddNode("Identifier: " + vin.Identifier.ToString().EscapeMarkup());
                    var expressionNode = treeNode.AddNode("Expression:");
                    RenderNode(expressionNode, vin.Expression);
                    break;
                }
            case NullableVariableInitializationNode nvin:
                {
                    var treeNode = parent.AddNode("NullableVariableInitialization:");
                    treeNode.AddNode("Types: " + string.Join("|", nvin.Types).EscapeMarkup());
                    treeNode.AddNode("Nullability: " + nvin.NullabilityIndicator.ToString().EscapeMarkup());
                    treeNode.AddNode("Identifier: " + nvin.Identifier.ToString().EscapeMarkup());
                    var expressionNode = treeNode.AddNode("Expression:");
                    RenderNode(expressionNode, nvin.Expression);
                    break;
                }
            case LiteralExpressionNode len:
                {
                    parent.AddNode("LiteralExpression: " + len.Literal.ToString().EscapeMarkup());
                    break;
                }
            case CallExpressionNode cen:
                {
                    var treeNode = parent.AddNode("CallExpression:");
                    treeNode.AddNode("Identifier: " + cen.Identifier.ToString().EscapeMarkup());
                    if (cen.Arguments.Count > 0)
                    {
                        var argumentsNode = treeNode.AddNode("Arguments:");
                        foreach (var argument in cen.Arguments)
                            RenderNode(argumentsNode, argument);
                    }
                    else
                    {
                        treeNode.AddNode("No arguments");
                    }

                    break;
                }
            case IfElseStatementNode iesn:
                {
                    var treeNode = parent.AddNode("IfElseStatement:");
                    var conditionNode = treeNode.AddNode("Condition:");
                    RenderNode(conditionNode, iesn.Condition);

                    if (iesn.Body.Count > 0)
                    {
                        var ifBodyNode = treeNode.AddNode("IfBody:");
                        foreach (var statement in iesn.Body)
                            RenderNode(ifBodyNode, statement);
                    }
                    else
                    {
                        treeNode.AddNode("Empty if-body");
                    }

                    if (iesn.ElseBody.Count > 0)
                    {
                        var elseBodyNode = treeNode.AddNode("ElseBody:");
                        foreach (var statement in iesn.ElseBody)
                            RenderNode(elseBodyNode, statement);
                    }
                    else
                    {
                        treeNode.AddNode("Empty else-body");
                    }

                    break;
                }
            case IdentifierExpressionNode ien:
                {
                    parent.AddNode("IdentifierExpression: " + ien.Identifier.ToString().EscapeMarkup());
                    break;
                }
            case CallStatementNode csn:
                {
                    var treeNode = parent.AddNode("CallStatement:");
                    treeNode.AddNode("Identifier: " + csn.CallExpression.Identifier.ToString().EscapeMarkup());
                    if (csn.CallExpression.Arguments.Count > 0)
                    {
                        var argumentsNode = treeNode.AddNode("Arguments:");
                        foreach (var argument in csn.CallExpression.Arguments)
                            RenderNode(argumentsNode, argument);
                    }
                    else
                    {
                        treeNode.AddNode("No arguments");
                    }
                    break;
                }
            case WhileStatementNode wsn:
                {
                    var treeNode = parent.AddNode("WhileStatement:");
                    var conditionNode = treeNode.AddNode("Condition:");
                    RenderNode(conditionNode, wsn.Condition);

                    if (wsn.Body.Count > 0)
                    {
                        var bodyNode = treeNode.AddNode("Body:");
                        foreach (var statement in wsn.Body)
                            RenderNode(bodyNode, statement);
                    }
                    else
                    {
                        treeNode.AddNode("Empty body");
                    }

                    break;
                }
            case VariableAssignmentNode van:
                {
                    var treeNode = parent.AddNode("VariableAssignment:");
                    treeNode.AddNode("Identifier: " + van.Identifier.ToString().EscapeMarkup());
                    var expressionNode = treeNode.AddNode("Expression:");
                    RenderNode(expressionNode, van.Expression);
                    break;
                }
            case IfStatementNode isn:
                {
                    var treeNode = parent.AddNode("IfStatement:");
                    var conditionNode = treeNode.AddNode("Condition:");
                    RenderNode(conditionNode, isn.Condition);

                    if (isn.Body.Count > 0)
                    {
                        var bodyNode = treeNode.AddNode("Body:");
                        foreach (var statement in isn.Body)
                            RenderNode(bodyNode, statement);
                    }
                    else
                    {
                        treeNode.AddNode("Empty body");
                    }

                    break;
                }
            case ShorthandMathOperationStatementNode smosn:
                {
                    var treeNode = parent.AddNode("ShorthandMathOperationStatement:");
                    treeNode.AddNode("Identifier: " + smosn.Identifier.ToString().EscapeMarkup());
                    treeNode.AddNode("Operator: " + smosn.Operation.ToString().EscapeMarkup());
                    break;
                }
            case BreakStatementNode bsn:
                {
                    parent.AddNode("BreakStatement: " + bsn.Token.ToString().EscapeMarkup());
                    break;
                }
            case ContinueStatementNode csn:
                {
                    parent.AddNode("ContinueStatement: " + csn.Token.ToString().EscapeMarkup());
                    break;
                }
            case ReturnStatementNode rsn:
                {
                    var treeNode = parent.AddNode("ReturnStatement:");
                    RenderNode(treeNode, rsn.Expression);
                    break;
                }
            case FunctionDefinitionExpressionNode fden:
                {
                    var treeNode = parent.AddNode("FunctionDefinitionExpression:");
                    var argumentsNode = treeNode.AddNode("Parameters:");

                    if (fden.Parameters.Count > 0)
                    {
                        foreach (var argument in fden.Parameters)
                            RenderNode(argumentsNode, argument);
                    }
                    else
                    {
                        argumentsNode.AddNode("No arguments");
                    }

                    var bodyNode = treeNode.AddNode("Body:");
                    if (fden.Body.Count > 0)
                    {
                        foreach (var statement in fden.Body)
                            RenderNode(bodyNode, statement);
                    }
                    else
                    {
                        bodyNode.AddNode("Empty body");
                    }

                    break;
                }
            case ListExpressionNode len:
                {
                    var treeNode = parent.AddNode("ListExpression:");
                    if (len.Expressions.Count > 0)
                    {
                        foreach (var item in len.Expressions)
                            RenderNode(treeNode, item);
                    }
                    else
                    {
                        treeNode.AddNode("Empty list");
                    }
                    break;
                }
            case MapExpressionNode men:
                {
                    var treeNode = parent.AddNode("MapExpression:");
                    if (men.Expressions.Count > 0)
                    {
                        foreach (var kvp in men.Expressions)
                        {
                            var entryNode = treeNode.AddNode(kvp.Key.ToString().EscapeMarkup());
                            RenderNode(entryNode, kvp.Value);
                        }
                    }
                    else
                    {
                        treeNode.AddNode("Empty map");
                    }
                    break;
                }
            case IdentifierIndexAssignmentNode iian:
                {
                    var treeNode = parent.AddNode("IdentifierIndexAssignment:");
                    treeNode.AddNode("Identifier: " + iian.Identifier.ToString().EscapeMarkup());
                    var indexNode = treeNode.AddNode("Index:");
                    RenderNode(indexNode, iian.IndexExpression);
                    var expressionNode = treeNode.AddNode("Expression:");
                    RenderNode(expressionNode, iian.ValueExpression);
                    break;
                }
            case IBinaryExpressionNode iben:
                {
                    RenderBinaryNodes(parent, iben);
                    break;
                }
            case IUnaryExpressionNode iuen:
                {
                    RenderUnaryNodes(parent, iuen);
                    break;
                }
            default:
                {
                    _ = parent.AddNode("[red]! " + parserNode.GetType().Name + "[/]");
                    break;
                }
        }
    }

    private static void RenderBinaryNodes(IHasTreeNodes parent, IBinaryExpressionNode node)
    {
        var treeNode = parent.AddNode(node.GetType().Name + ":");
        treeNode.AddNode("Operator: " + node.Op.ToSymbol());
        var leftNode = treeNode.AddNode("Left:");
        RenderNode(leftNode, node.Left);
        var rightNode = treeNode.AddNode("Right:");
        RenderNode(rightNode, node.Right);
    }

    private static void RenderUnaryNodes(IHasTreeNodes parent, IUnaryExpressionNode node)
    {
        var treeNode = parent.AddNode(node.GetType().Name + ":");
        RenderNode(treeNode, node.Expression);
    }
}