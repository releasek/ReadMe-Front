namespace ReadMe_Back.Models.DTOs
{
    public class CreateUserDto
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<int> AssignedRoleIds { get; set; }
    }
}
