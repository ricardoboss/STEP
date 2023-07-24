namespace HILFE.Parsing.Statements;

public enum StatementType
{
    EmptyStatement,
    VariableDeclaration,
    VariableAssignment,
    FunctionCall,
    IfStatement,
    IfElseStatement,
    WhileStatement,
    AnonymousCodeBlock,
    ReturnStatement,
    DiscardAssignment,
}