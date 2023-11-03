namespace StepLang.Parsing;

public interface IVariableDeclarationVisitor
{
    void Execute(VariableDeclarationNode variableDeclarationNode);
    void Execute(NullableVariableDeclarationNode variableDeclarationNode);
    void Execute(VariableInitializationNode variableDeclarationNode);
    void Execute(NullableVariableInitializationNode variableDeclarationNode);
}