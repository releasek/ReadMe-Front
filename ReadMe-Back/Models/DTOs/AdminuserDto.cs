using System.ComponentModel.DataAnnotations;

namespace ReadMe_Back.Models.DTOs
{
    public class AdminuserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }

        //public string RoleName { get; set; }

        public string PermissionName { get; set; }

    }
}
