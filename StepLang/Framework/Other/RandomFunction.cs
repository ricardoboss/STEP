using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

/// <summary>
/// Returns an insecure, random number between 0 and 1.
/// </summary>
/// <seealso cref="SeedFunction"/>
public class RandomFunction : GenericFunction
{
    /// <summary>
    /// The identifier of the <see cref="RandomFunction"/>.
    /// </summary>
    public const string Identifier = "random";

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyNumber;

    /// <inheritdoc />
    [SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "This is not for security purposes")]
    protected override NumberResult Invoke(TokenLocation callLocation, Interpreter interpreter) => interpreter.Random.NextDouble();
}