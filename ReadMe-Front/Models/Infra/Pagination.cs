using System;

namespace ReadMe_Front.Models.Infra
{
    public class Pagination
    {
        public Pagination(int pageNumber, int pageSize, int totalRecords, string selectedOption)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            this.selectedOption = selectedOption;
        }
        public object RouteValues { get; set; } // 傳遞篩選條件的路由參數
        public string ActionName { get; set; } // Action 名稱
        public string ControllerName { get; set; } // Controller 名稱

        public string selectedOption { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalRecords { get; }

        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);

        /// <summary>
        /// 是否顯示第一頁
        /// </summary>
        public bool ShowFirstPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        /// <summary>
        /// 是否顯示上一頁
        /// </summary>
        public bool ShowPrevPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        /// <summary>
        /// 是否顯示最末頁
        /// </summary>
        public bool ShowLastPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }

        /// <summary>
        /// 是否顯示下一頁
        /// </summary>
        public bool ShowNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }
    }
}