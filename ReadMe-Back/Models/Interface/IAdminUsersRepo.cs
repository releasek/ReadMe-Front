using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.EFModels;

namespace ReadMe_Back.Models.Interface
{
    public interface IAdminUsersRepo
    {
        List<AdminUser> GetAllAdminUsers();
        List<AdminuserDto> GetAdminUsers();
    }
}
