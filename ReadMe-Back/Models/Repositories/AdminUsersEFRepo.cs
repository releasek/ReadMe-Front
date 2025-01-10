using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.ViewModels;

namespace ReadMe_Back.Models.Repositories
{
    public class AdminUsersEFRepo
    {
        private readonly AppDbContext _db;

        public AdminUsersEFRepo(AppDbContext db)
        {
            _db = db;
        }
        public List<AdminUser> GetAllAdminUsers()
        {
          
        }
        public List<AdminuserDto> GetAdminUsers()
        {
            using (var db = new AppDbContext())
            {
                var result = from adminUser in db.AdminUsers
                             join group in db.Groups
                             on AdminuserVm.GroupId equals group.Id
                             join role in db.Roles
                             on adminUser.RoleId equals role.Id
                             join permission in db.Permissions
                             on adminUser.PermissionId equals permission.Id
                             select new AdminuserDto
                             {
                                 Id = adminUser.Id,
                                 Name = adminUser.Name,
                                 GroupName = group.GroupName,
                                 RoleName = role.RoleName,
                                 PermissionName = permission.PermissionName
                             };
                return result.ToList();
            }
        }

    }
}
