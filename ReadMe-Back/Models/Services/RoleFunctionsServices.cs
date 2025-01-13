using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.Repositories;

namespace ReadMe_Back.Models.Services
{
    public class RoleFunctionsServices
    {
        private readonly RoleFunctionsEFRepo _repo;

        public RoleFunctionsServices(RoleFunctionsEFRepo repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<RoleFunctionDto>> GetAllRolesAsync()
        {
            return await _repo.GetAllRoles();
        }



    }
}
