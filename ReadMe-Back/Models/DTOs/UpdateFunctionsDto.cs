namespace ReadMe_Back.Models.DTOs
{
    public class UpdateFunctionsDto
    {
        public int RoleId { get; set; }
        public List<int> AssignedFunctionIds { get; set; }
    }
}
