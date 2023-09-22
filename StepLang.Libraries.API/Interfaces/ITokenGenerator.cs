using StepLang.Libraries.API.DB.Entities;

namespace StepLang.Libraries.API.Interfaces;

public interface ITokenGenerator
{
    public string Create(Author owner);
}