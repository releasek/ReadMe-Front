namespace ReadMe_Back.Models.ViewModels
{
    public class OrderQueryParameters
    {
        public DateTime? StartDate { get; set; } // 查詢的起始日期
        public DateTime? EndDate { get; set; } // 查詢的結束日期
        public int? Amount { get; set; } // 查詢的最低金額
        public string? Keyword { get; set; } // 查詢的關鍵字 (適用於名稱或訂單編號)
        public string? AmountSort { get; set; } // 金額排序 ("asc" 或 "desc")

        // 新增的屬性
        public int PageNumber { get; set; } = 1; // 頁碼
        public int PageSize { get; set; } = 10; // 每頁數量
    }
}
