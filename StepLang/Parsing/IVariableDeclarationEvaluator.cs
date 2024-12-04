using StepLang.Interpreting;

namespace StepLang.Parsing;

public interface IVariableDeclarationEvaluator
{
    Variable Evaluate(VariableDeclarationNode variableDeclarationNode);
    Variable Evaluate(NullableVariableDeclarationNode variableDeclarationNode);
    Variable Evaluate(VariableInitializationNode variableDeclarationNode);
    Variable Evaluate(NullableVariableInitializationNode variableDeclarationNode);
}