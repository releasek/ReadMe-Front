namespace ReadMe_Back.Models.DTOs
{
    public class CreateFunctionDto
    {
        public string FunctionName { get; set; }
        public List<int> AssignedRoleIds { get; set; }
    }
}
