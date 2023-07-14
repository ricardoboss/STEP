namespace HILFE.Parsing;

public enum StatementType
{
    EmptyLine,
    VariableDeclaration,
    IfStatement,
    ElseStatement,
    IfElseStatement,
    WhileStatement,
    FunctionCall,
    CodeBlockStart,
    CodeBlockEnd,
}