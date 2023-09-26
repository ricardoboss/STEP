using PasswordRulesSharp.Builder;
using PasswordRulesSharp.Models;
using PasswordRulesSharp.Rules;
using PasswordRulesSharp.Validator;
using PasswordRulesSharp.Validator.Requirements;

namespace Leap.Common.API;

public record RegisterRequest(string Username, string Password)
{
    private static Lazy<IRule> PasswordRule { get; } = new(() =>
        new RuleBuilder()
            .RequireLower()
            .RequireUpper()
            .RequireDigit()
            .MinLength(6)
            .MaxConsecutive(3)
            .Build());

    private static Lazy<Validator> PasswordValidator { get; } = new(() => new(PasswordRule.Value));

    public void Validate()
    {
        if (!Library.UsernameRegex().IsMatch(Username))
            throw new ValidationException("User name must only contain lowercase characters and hyphens (-) and must start and end with a character with a minimum length of 2 characters.");

        if (PasswordValidator.Value.PasswordIsValid(Password, out var requirements))
            return;

        var firstFailedRequirement = requirements.First(t => !t.Success);

        switch (firstFailedRequirement.Requirement.Type)
        {
            case RequirementType.MinimumLength:
                throw new ValidationException($"Password must be at least {PasswordRule.Value.MinLength} characters long.");
            case RequirementType.MaximumLength:
                throw new ValidationException($"Password must be at most {PasswordRule.Value.MaxLength} characters long.");
            case RequirementType.MaxConsecutive:
                throw new ValidationException($"Password must not contain more than {PasswordRule.Value.MaxConsecutive} consecutive characters.");
            case RequirementType.RequiredChars:
                var requiredChars = ((CharacterClassRequirement)firstFailedRequirement.Requirement).CharacterClass;
                if (requiredChars == CharacterClass.Upper)
                    throw new ValidationException("Password must contain at least one uppercase character.");

                if (requiredChars == CharacterClass.Lower)
                    throw new ValidationException("Password must contain at least one lowercase character.");

                if (requiredChars == CharacterClass.AsciiPrintable)
                    throw new ValidationException("Password must contain at least one ASCII printable character.");

                if (requiredChars == CharacterClass.Digit)
                    throw new ValidationException("Password must contain at least one digit.");

                if (requiredChars == CharacterClass.Unicode)
                    throw new ValidationException("Password must contain at least one character.");

                if (requiredChars is SpecificCharacterClass specific)
                    throw new ValidationException($"Password must contain at least one of {string.Join(' ', specific.Chars)}.");

                throw new ValidationException("Password does not meet requirements.");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
