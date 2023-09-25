using Leap.API.DB.Entities;

namespace Leap.API.Interfaces;

public interface ITokenGenerator
{
    public string Create(Author owner);
}