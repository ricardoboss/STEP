using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
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
        var source = await CharacterSource.FromFileAsync(scriptFile);
        var tokenizer = new Tokenizer(source);
        var tokens = tokenizer.Tokenize();
        var parser = new Parser(tokens);
        var root = parser.ParseRoot();

        var tree = new Tree(scriptFile.Name);
        var treeBuilder = new RootTreeBuilder(tree);
        root.Accept(treeBuilder);

        AnsiConsole.Write(tree);

        return 0;
    }

    private sealed class RootTreeBuilder : IRootNodeVisitor
    {
        private readonly IHasTreeNodes root;

        public RootTreeBuilder(IHasTreeNodes root)
        {
            this.root = root;
        }

        public void Run(RootNode node)
        {
            IHasTreeNodes statementsRoot;
            if (node.Imports.Count > 0)
            {
                var importsNode = root.AddNode("Imports:");
                var importTreeBuilder = new ImportTreeBuilder(importsNode);
                foreach (var import in node.Imports)
                    import.Accept(importTreeBuilder);

                statementsRoot = root.AddNode("Statements:");
            }
            else
                statementsRoot = root;

            var statementTreeBuilder = new StatementTreeBuilder(statementsRoot);
            foreach (var statement in node.Body)
                statement.Accept(statementTreeBuilder);
        }
    }

    private sealed class ImportTreeBuilder : IImportNodeVisitor
    {
        private readonly IHasTreeNodes parent;

        public ImportTreeBuilder(IHasTreeNodes parent)
        {
            this.parent = parent;
        }

        public void Visit(ImportNode importNode)
        {
            parent.AddNode("Import: " + importNode.PathToken.StringValue);
        }
    }

    private sealed class StatementTreeBuilder : IStatementVisitor
    {
        private readonly IHasTreeNodes root;

        public StatementTreeBuilder(IHasTreeNodes root)
        {
            this.root = root;
        }

        public void Execute(CallStatementNode statementNode)
        {
            var node = root.AddNode("CallStatement:");
            var evaluator = new ExpressionTreeBuilder(node);
            var expression = statementNode.CallExpression;
            _ = expression.EvaluateUsing(evaluator);
        }

        public void Execute(CodeBlockStatementNode statementNode)
        {
            var node = root.AddNode("CodeBlockStatement:");
            var statementTreeBuilder = new StatementTreeBuilder(node);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);
        }

        public void Execute(ContinueStatementNode statementNode)
        {
            root.AddNode("ContinueStatement");
        }

        public void Execute(ForeachDeclareKeyDeclareValueStatementNode statementNode)
        {
            var node = root.AddNode("ForeachStatement:");

            var keyDeclarationNode = node.AddNode("KeyDeclaration:");
            var keyExpressionBuilder = new VariableDeclarationTreeBuilder(keyDeclarationNode);
            statementNode.KeyDeclaration.EvaluateUsing(keyExpressionBuilder);

            var valueDeclarationNode = node.AddNode("ValueDeclaration:");
            var valueExpressionBuilder = new VariableDeclarationTreeBuilder(valueDeclarationNode);
            statementNode.ValueDeclaration.EvaluateUsing(valueExpressionBuilder);

            var expressionNode = node.AddNode("Expression:");
            var expressionBuilder = new ExpressionTreeBuilder(expressionNode);
            statementNode.Collection.EvaluateUsing(expressionBuilder);

            var bodyNode = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(bodyNode);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);
        }

        public void Execute(BreakStatementNode statementNode)
        {
            root.AddNode("BreakStatement");
        }

        public void Execute(ForeachDeclareKeyValueStatementNode statementNode)
        {
            var node = root.AddNode("ForeachStatement:");

            var keyDeclarationNode = node.AddNode("KeyDeclaration:");
            var keyExpressionBuilder = new VariableDeclarationTreeBuilder(keyDeclarationNode);
            statementNode.KeyDeclaration.EvaluateUsing(keyExpressionBuilder);

            node.AddNode("ValueIdentifier: " + statementNode.ValueIdentifier.ToString().EscapeMarkup());

            var expressionNode = node.AddNode("Expression:");
            var expressionBuilder = new ExpressionTreeBuilder(expressionNode);
            statementNode.Collection.EvaluateUsing(expressionBuilder);

            var bodyNode = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(bodyNode);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);
        }

        public void Execute(ForeachDeclareValueStatementNode statementNode)
        {
            var node = root.AddNode("ForeachStatement:");

            var valueDeclarationNode = node.AddNode("ValueDeclaration:");
            var valueExpressionBuilder = new VariableDeclarationTreeBuilder(valueDeclarationNode);
            statementNode.ValueDeclaration.EvaluateUsing(valueExpressionBuilder);

            var expressionNode = node.AddNode("Expression:");
            var expressionBuilder = new ExpressionTreeBuilder(expressionNode);
            statementNode.Collection.EvaluateUsing(expressionBuilder);

            var bodyNode = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(bodyNode);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);
        }

        public void Execute(ForeachKeyValueStatementNode statementNode)
        {
            var node = root.AddNode("ForeachStatement:");

            node.AddNode("KeyIdentifier: " + statementNode.KeyIdentifier.ToString().EscapeMarkup());
            node.AddNode("ValueIdentifier: " + statementNode.ValueIdentifier.ToString().EscapeMarkup());

            var expressionNode = node.AddNode("Expression:");
            var expressionBuilder = new ExpressionTreeBuilder(expressionNode);
            statementNode.Collection.EvaluateUsing(expressionBuilder);

            var bodyNode = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(bodyNode);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);
        }

        public void Execute(ForeachKeyDeclareValueStatementNode statementNode)
        {
            var node = root.AddNode("ForeachStatement:");

            node.AddNode("KeyIdentifier: " + statementNode.KeyIdentifier.ToString().EscapeMarkup());

            var valueDeclarationNode = node.AddNode("ValueDeclaration:");
            var valueExpressionBuilder = new VariableDeclarationTreeBuilder(valueDeclarationNode);
            statementNode.ValueDeclaration.EvaluateUsing(valueExpressionBuilder);

            var expressionNode = node.AddNode("Expression:");
            var expressionBuilder = new ExpressionTreeBuilder(expressionNode);
            statementNode.Collection.EvaluateUsing(expressionBuilder);

            var bodyNode = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(bodyNode);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);
        }

        public void Execute(ForeachValueStatementNode statementNode)
        {
            var node = root.AddNode("ForeachStatement:");

            node.AddNode("ValueIdentifier: " + statementNode.Identifier.ToString().EscapeMarkup());

            var expressionNode = node.AddNode("Expression:");
            var expressionBuilder = new ExpressionTreeBuilder(expressionNode);
            statementNode.Collection.EvaluateUsing(expressionBuilder);

            var bodyNode = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(bodyNode);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);
        }

        public void Execute(IdentifierIndexAssignmentNode statementNode)
        {
            var node = root.AddNode("IdentifierIndexAssignment:");
            node.AddNode("Identifier: " + statementNode.Identifier.ToString().EscapeMarkup());

            var indexNode = node.AddNode("Index:");
            var indexTreeBuilder = new ExpressionTreeBuilder(indexNode);
            statementNode.IndexExpression.EvaluateUsing(indexTreeBuilder);

            var expressionNode = node.AddNode("Expression:");
            var expressionTreeBuilder = new ExpressionTreeBuilder(expressionNode);
            statementNode.ValueExpression.EvaluateUsing(expressionTreeBuilder);
        }

        public void Execute(IfElseIfStatementNode statementNode)
        {
            var node = root.AddNode("IfElseIfStatement:");

            var conditionNode = node.AddNode("Condition:");
            var expressionTreeBuilder = new ExpressionTreeBuilder(conditionNode);
            statementNode.Condition.EvaluateUsing(expressionTreeBuilder);

            var body = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(body);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);

            var elseIfConditionNode = node.AddNode("ElseIfCondition:");
            var elseIfExpressionTreeBuilder = new ExpressionTreeBuilder(elseIfConditionNode);
            statementNode.ElseCondition.EvaluateUsing(elseIfExpressionTreeBuilder);

            var elseIfBody = node.AddNode("ElseBody:");
            var elseIfStatementTreeBuilder = new StatementTreeBuilder(elseIfBody);
            foreach (var statement in statementNode.ElseBody)
                statement.Accept(elseIfStatementTreeBuilder);
        }

        public void Execute(IfElseStatementNode statementNode)
        {
            var node = root.AddNode("IfElseStatement:");

            var conditionNode = node.AddNode("Condition:");
            var expressionTreeBuilder = new ExpressionTreeBuilder(conditionNode);
            statementNode.Condition.EvaluateUsing(expressionTreeBuilder);

            var body = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(body);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);

            var elseBody = node.AddNode("ElseBody:");
            var elseStatementTreeBuilder = new StatementTreeBuilder(elseBody);
            foreach (var statement in statementNode.ElseBody)
                statement.Accept(elseStatementTreeBuilder);
        }

        public void Execute(IfStatementNode statementNode)
        {
            var node = root.AddNode("IfStatement:");

            var conditionNode = node.AddNode("Condition:");
            var expressionTreeBuilder = new ExpressionTreeBuilder(conditionNode);
            statementNode.Condition.EvaluateUsing(expressionTreeBuilder);

            var body = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(body);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);
        }

        public void Execute(ReturnExpressionStatementNode statementNode)
        {
            var node = root.AddNode("ReturnExpressionStatement:");
            var expressionTreeBuilder = new ExpressionTreeBuilder(node);
            statementNode.Expression.EvaluateUsing(expressionTreeBuilder);
        }

        public void Execute(VariableAssignmentNode statementNode)
        {
            var node = root.AddNode("VariableAssignment:");
            node.AddNode("Identifier: " + statementNode.Identifier.ToString().EscapeMarkup());

            var expressionNode = node.AddNode("Expression:");
            var expressionTreeBuilder = new ExpressionTreeBuilder(expressionNode);
            statementNode.Expression.EvaluateUsing(expressionTreeBuilder);
        }

        public void Execute(WhileStatementNode statementNode)
        {
            var node = root.AddNode("WhileStatement:");

            var conditionNode = node.AddNode("Condition:");
            var expressionTreeBuilder = new ExpressionTreeBuilder(conditionNode);
            statementNode.Condition.EvaluateUsing(expressionTreeBuilder);

            var body = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(body);
            foreach (var statement in statementNode.Body)
                statement.Accept(statementTreeBuilder);
        }

        public void Execute(IncrementStatementNode statementNode)
        {
            var node = root.AddNode("IncrementStatement:");

            _ = node.AddNode("Operator: ++");
            _ = node.AddNode("Identifier: " + statementNode.Identifier.ToString().EscapeMarkup());
        }

        public void Execute(DecrementStatementNode statementNode)
        {
            var node = root.AddNode("DecrementStatement:");

            _ = node.AddNode("Operator: --");
            _ = node.AddNode("Identifier: " + statementNode.Identifier.ToString().EscapeMarkup());
        }

        public void Execute(VariableDeclarationStatement statementNode)
        {
            var variableDeclarationTreeBuilder = new VariableDeclarationTreeBuilder(root);
            statementNode.Declaration.EvaluateUsing(variableDeclarationTreeBuilder);
        }

        public void Execute(ReturnStatementNode statementNode)
        {
            root.AddNode("ReturnStatement");
        }

        public void Execute(DiscardStatementNode discardStatementNode)
        {
            var node = root.AddNode("DiscardStatement");

            var expressionNode = node.AddNode("Expression:");
            var expressionTreeBuilder = new ExpressionTreeBuilder(expressionNode);
            discardStatementNode.Expression.EvaluateUsing(expressionTreeBuilder);
        }
    }

    private sealed class VariableDeclarationTreeBuilder : IVariableDeclarationEvaluator
    {
        private readonly IHasTreeNodes root;

        public VariableDeclarationTreeBuilder(IHasTreeNodes root)
        {
            this.root = root;
        }

        public Variable Execute(VariableDeclarationNode variableDeclarationNode)
        {
            var node = root.AddNode("VariableDeclaration:");
            node.AddNode("Types: " + string.Join("|", variableDeclarationNode.Types).EscapeMarkup());
            node.AddNode("Identifier: " + variableDeclarationNode.Identifier.ToString().EscapeMarkup());

            return DummyVariable;
        }

        public Variable Execute(NullableVariableDeclarationNode variableDeclarationNode)
        {
            var node = root.AddNode("NullableVariableDeclaration:");
            node.AddNode("Types: " + string.Join("|", variableDeclarationNode.Types).EscapeMarkup());
            node.AddNode("Nullability: " + variableDeclarationNode.NullabilityIndicator.ToString().EscapeMarkup());
            node.AddNode("Identifier: " + variableDeclarationNode.Identifier.ToString().EscapeMarkup());

            return DummyVariable;
        }

        public Variable Execute(VariableInitializationNode variableDeclarationNode)
        {
            var node = root.AddNode("VariableInitialization:");
            node.AddNode("Types: " + string.Join("|", variableDeclarationNode.Types).EscapeMarkup());
            node.AddNode("Identifier: " + variableDeclarationNode.Identifier.ToString().EscapeMarkup());

            var expressionNode = node.AddNode("Expression:");
            var expressionTreeBuilder = new ExpressionTreeBuilder(expressionNode);
            variableDeclarationNode.Expression.EvaluateUsing(expressionTreeBuilder);

            return DummyVariable;
        }

        public Variable Execute(NullableVariableInitializationNode variableDeclarationNode)
        {
            var node = root.AddNode("NullableVariableInitialization:");
            node.AddNode("Types: " + string.Join("|", variableDeclarationNode.Types).EscapeMarkup());
            node.AddNode("Nullability: " + variableDeclarationNode.NullabilityIndicator.ToString().EscapeMarkup());
            node.AddNode("Identifier: " + variableDeclarationNode.Identifier.ToString().EscapeMarkup());

            var expressionNode = node.AddNode("Expression:");
            var expressionTreeBuilder = new ExpressionTreeBuilder(expressionNode);
            variableDeclarationNode.Expression.EvaluateUsing(expressionTreeBuilder);

            return DummyVariable;
        }

        private static readonly Variable DummyVariable = new("dummy", Array.Empty<ResultType>(), false);
    }

    private sealed class ExpressionTreeBuilder : IExpressionEvaluator
    {
        private readonly IHasTreeNodes root;

        public ExpressionTreeBuilder(IHasTreeNodes root)
        {
            this.root = root;
        }

        public ExpressionResult Evaluate(CallExpressionNode expressionNode)
        {
            var node = root.AddNode("CallExpression:");
            node.AddNode("Identifier: " + expressionNode.Identifier.ToString().EscapeMarkup());
            if (expressionNode.Arguments.Count > 0)
            {
                var argumentsNode = node.AddNode("Arguments:");
                var evaluator = new ExpressionTreeBuilder(argumentsNode);
                foreach (var argument in expressionNode.Arguments)
                    argument.EvaluateUsing(evaluator);
            }
            else
                node.AddNode("No arguments");

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(FunctionDefinitionExpressionNode expressionNode)
        {
            var node = root.AddNode("FunctionDefinitionExpression:");

            var argumentsNode = node.AddNode("Parameters:");
            var variableDeclarationTreeBuilder = new VariableDeclarationTreeBuilder(argumentsNode);
            foreach (var argument in expressionNode.Parameters)
                argument.EvaluateUsing(variableDeclarationTreeBuilder);

            var bodyNode = node.AddNode("Body:");
            var statementTreeBuilder = new StatementTreeBuilder(bodyNode);
            foreach (var statement in expressionNode.Body)
                statement.Accept(statementTreeBuilder);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(IdentifierExpressionNode expressionNode)
        {
            root.AddNode("IdentifierExpression: " + expressionNode.Identifier.ToString().EscapeMarkup());

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(ListExpressionNode expressionNode)
        {
            var node = root.AddNode("ListExpression:");

            var entriesNode = node.AddNode("Entries:");
            var entryTreeBuilder = new ExpressionTreeBuilder(entriesNode);
            foreach (var entry in expressionNode.Expressions)
            {
                entry.EvaluateUsing(entryTreeBuilder);
            }

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(LiteralExpressionNode expressionNode)
        {
            root.AddNode("LiteralExpression: " + expressionNode.Literal.ToString().EscapeMarkup());

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(MapExpressionNode expressionNode)
        {
            var node = root.AddNode("MapExpression:");

            var entriesNode = node.AddNode("Entries:");
            foreach (var kvp in expressionNode.Expressions)
            {
                var entryNode = entriesNode.AddNode(kvp.Key.ToString().EscapeMarkup());
                var entriesTreeBuilder = new ExpressionTreeBuilder(entryNode);
                kvp.Value.EvaluateUsing(entriesTreeBuilder);
            }

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(AddExpressionNode expressionNode)
        {
            var node = root.AddNode("AddExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(CoalesceExpressionNode expressionNode)
        {
            var node = root.AddNode("CoalesceExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(NotEqualsExpressionNode expressionNode)
        {
            var node = root.AddNode("NotEqualsExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(EqualsExpressionNode expressionNode)
        {
            var node = root.AddNode("EqualsExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(NegateExpressionNode expressionNode)
        {
            var node = root.AddNode("NegateExpression:");

            _ = node.AddNode("Operator: -");

            var expressionNodeTreeBuilder = new ExpressionTreeBuilder(node);
            expressionNode.Expression.EvaluateUsing(expressionNodeTreeBuilder);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(SubtractExpressionNode expressionNode)
        {
            var node = root.AddNode("SubtractExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(MultiplyExpressionNode expressionNode)
        {
            var node = root.AddNode("MultiplyExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(DivideExpressionNode expressionNode)
        {
            var node = root.AddNode("DivideExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(ModuloExpressionNode expressionNode)
        {
            var node = root.AddNode("ModuloExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(PowerExpressionNode expressionNode)
        {
            var node = root.AddNode("PowerExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(GreaterThanExpressionNode expressionNode)
        {
            var node = root.AddNode("GreaterThanExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(LessThanExpressionNode expressionNode)
        {
            var node = root.AddNode("LessThanExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(GreaterThanOrEqualExpressionNode expressionNode)
        {
            var node = root.AddNode("GreaterThanOrEqualExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(LessThanOrEqualExpressionNode expressionNode)
        {
            var node = root.AddNode("LessThanOrEqualExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(LogicalAndExpressionNode expressionNode)
        {
            var node = root.AddNode("LogicalAndExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(LogicalOrExpressionNode expressionNode)
        {
            var node = root.AddNode("LogicalOrExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(BitwiseXorExpressionNode expressionNode)
        {
            var node = root.AddNode("BitwiseXorExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(BitwiseAndExpressionNode expressionNode)
        {
            var node = root.AddNode("BitwiseAndExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(BitwiseOrExpressionNode expressionNode)
        {
            var node = root.AddNode("BitwiseOrExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(BitwiseShiftLeftExpressionNode expressionNode)
        {
            var node = root.AddNode("BitwiseShiftLeftExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(BitwiseShiftRightExpressionNode expressionNode)
        {
            var node = root.AddNode("BitwiseShiftRightExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(BitwiseRotateLeftExpressionNode expressionNode)
        {
            var node = root.AddNode("BitwiseRotateLeftExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(BitwiseRotateRightExpressionNode expressionNode)
        {
            var node = root.AddNode("BitwiseRotateRightExpression:");

            EvaluateBinary(node, expressionNode);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(NotExpressionNode expressionNode)
        {
            var node = root.AddNode("NotExpression:");

            _ = node.AddNode("Operator: !");

            var expressionNodeTreeBuilder = new ExpressionTreeBuilder(node);
            expressionNode.Expression.EvaluateUsing(expressionNodeTreeBuilder);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(FunctionDefinitionCallExpressionNode expressionNode)
        {
            var node = root.AddNode("FunctionDefinitionCallExpression:");

            var definitionNode = node.AddNode("Definition:");
            var definitionParameters = definitionNode.AddNode("Parameters:");
            var definitionParametersTreeBuilder = new VariableDeclarationTreeBuilder(definitionParameters);
            foreach (var parameter in expressionNode.Parameters)
                parameter.EvaluateUsing(definitionParametersTreeBuilder);

            var definitionBody = definitionNode.AddNode("Body:");
            var definitionBodyTreeBuilder = new StatementTreeBuilder(definitionBody);
            foreach (var statement in expressionNode.Body)
                statement.Accept(definitionBodyTreeBuilder);

            var argumentsNode = node.AddNode("Arguments:");
            var argumentsTreeBuilder = new ExpressionTreeBuilder(argumentsNode);
            foreach (var argument in expressionNode.CallArguments)
                argument.EvaluateUsing(argumentsTreeBuilder);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(NativeFunctionDefinitionExpressionNode expressionNode)
        {
            var node = root.AddNode("NativeFunctionDefinitionExpression:");

            var argumentsNode = node.AddNode("Parameters:");
            var variableDeclarationTreeBuilder = new VariableDeclarationTreeBuilder(argumentsNode);
            foreach (var argument in expressionNode.Definition.Parameters)
                argument.EvaluateUsing(variableDeclarationTreeBuilder);

            return VoidResult.Instance;
        }

        public ExpressionResult Evaluate(IndexAccessExpressionNode expressionNode)
        {
            var node = root.AddNode("IndexAccessExpression:");

            var expression = node.AddNode("Expression:");
            var expressionNodeTreeBuilder = new ExpressionTreeBuilder(expression);
            expressionNode.Left.EvaluateUsing(expressionNodeTreeBuilder);

            var index = node.AddNode("Index:");
            var indexNodeTreeBuilder = new ExpressionTreeBuilder(index);
            expressionNode.Index.EvaluateUsing(indexNodeTreeBuilder);

            return VoidResult.Instance;
        }

        private static void EvaluateBinary(IHasTreeNodes parent, IBinaryExpressionNode node)
        {
            _ = parent.AddNode("Operator: " + node.Op.ToSymbol().EscapeMarkup());

            var leftNode = parent.AddNode("Left:");
            var leftExpressionTreeBuilder = new ExpressionTreeBuilder(leftNode);
            node.Left.EvaluateUsing(leftExpressionTreeBuilder);

            var rightNode = parent.AddNode("Right:");
            var rightExpressionTreeBuilder = new ExpressionTreeBuilder(rightNode);
            node.Right.EvaluateUsing(rightExpressionTreeBuilder);
        }
    }
}