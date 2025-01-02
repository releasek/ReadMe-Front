using ReadMe_Front.Models.DTOs;
using System.Collections.Generic;

namespace ReadMe_Front.Models.Interfaces
{
    public interface ICategoryRepo
    {
        List<CategoryDto> GetAll();

    }
}