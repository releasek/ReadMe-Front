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

    public class FilterVm
    {
        //以下是篩選結果
        public string CategoryName { get; set; }

        public string Publisher { get; set; }

        public string Authoor { get; set; }

        //以下是查詢結果
        public List<SearchVm> Data { get; set; }
    }
}