namespace ReadMe_Back.Models.DTOs
{
    public class CreateRoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<int> AssignedFunctionIds { get; set; }

    }
}
