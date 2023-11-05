using StepLang.Interpreting;

namespace StepLang.Parsing;

public interface IVariableDeclarationEvaluator
{
    Variable Execute(VariableDeclarationNode variableDeclarationNode);
    Variable Execute(NullableVariableDeclarationNode variableDeclarationNode);
    Variable Execute(VariableInitializationNode variableDeclarationNode);
    Variable Execute(NullableVariableInitializationNode variableDeclarationNode);
}