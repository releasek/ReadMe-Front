namespace ReadMe_Back.Models.DTOs
{
    public class UpdateRolesDto
    {
        public int UserId { get; set; }
        public List<int> AssignedRoleIds { get; set; }
    }
}
