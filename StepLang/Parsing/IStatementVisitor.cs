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
    void Execute(ReturnExpressionStatementNode statementNode);
    void Execute(VariableAssignmentNode statementNode);
    void Execute(WhileStatementNode statementNode);
    void Execute(IncrementStatementNode statementNode);
    void Execute(DecrementStatementNode statementNode);
    void Execute(VariableDeclarationStatementNode statementNode);
    void Execute(ReturnStatementNode statementNode);
    void Execute(DiscardStatementNode discardStatementNode);
}