namespace ReadMe_Back.Models.ViewModels
{
    public class UserVm
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public List<string> Roles { get; set; }

    }
}
