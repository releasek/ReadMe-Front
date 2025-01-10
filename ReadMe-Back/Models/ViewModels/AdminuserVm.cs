using System.ComponentModel.DataAnnotations;

namespace ReadMe_Back.Models.ViewModels
{
    public class AdminuserVm
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string GroupName { get; set; }

        public string RoleName { get; set; }

        public string PermissionName { get; set; }

    }
}
