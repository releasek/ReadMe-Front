using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.Repositories;
using System.Collections.Generic;

namespace ReadMe_Front.Models.ViewModels
{
    public class SearchVm
    {
        public List<CategoryDto> Data { get; set; } // 主要資料
        public PaginationInfo PaginationInfo { get; set; } // 分頁資訊
    }
}