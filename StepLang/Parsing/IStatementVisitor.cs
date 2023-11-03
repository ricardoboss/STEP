namespace StepLang.Parsing;

public interface IStatementVisitor
{
    void Execute(CallStatementNode statementNode);
    void Execute(CodeBlockStatementNode statementNode);
    void Execute(ContinueStatementNode statementNode);
    void Execute(ForeachDeclareKeyDeclareValueStatementNode statementNode);
    void Execute(BreakStatementNode statementNode);
    void Execute(ForeachDeclareKeyValueStatementNode statementNode);
    void Execute(ForeachDeclareValueStatementNode statementNode);
    void Execute(ForeachKeyValueStatementNode statementNode);
    void Execute(ForeachKeyDeclareValueStatementNode statementNode);
    void Execute(ForeachValueStatementNode statementNode);
    void Execute(IdentifierIndexAssignmentNode statementNode);
    void Execute(IfElseIfStatementNode statementNode);
    void Execute(IfElseStatementNode statementNode);
    void Execute(IfStatementNode statementNode);
    void Execute(NullableVariableDeclarationNode statementNode);
    void Execute(NullableVariableInitializationNode statementNode);
    void Execute(ReturnStatementNode statementNode);
    void Execute(ShorthandMathOperationExpressionStatementNode statementNode);
    void Execute(ShorthandMathOperationStatementNode statementNode);
    void Execute(VariableAssignmentNode statementNode);
    void Execute(WhileStatementNode statementNode);
    void Execute(VariableInitializationNode statementNode);
    void Execute(VariableDeclarationNode statementNode);
}