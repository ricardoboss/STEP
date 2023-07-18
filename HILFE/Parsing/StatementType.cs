namespace HILFE.Parsing;

public enum StatementType
{
    EmptyStatement,
    VariableDeclaration,
    IfStatement,
    IfElseStatement,
    WhileStatement,
    FunctionCall,
    CodeBlockStart,
    CodeBlockEnd,
}