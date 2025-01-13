namespace ReadMe_Back.Models.DTOs
{
    public class CreateUserDto
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public List<int> AssignedRoleIds { get; set; }
    }
}
