namespace TaskManagement.API.Interfaces;

public interface ITokenService
{
    public string CreateToken(Guid userId);
}