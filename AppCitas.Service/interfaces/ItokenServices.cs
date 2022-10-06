using AppCitas.Service.Entities;

namespace AppCitas.Service.interfaces;

public interface ITokenServices
{
    string CreateToken(AppUser user);
}
