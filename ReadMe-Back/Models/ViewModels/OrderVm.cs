namespace ReadMe_Back.Models.ViewModels
{
    public class OrderVm
    {
        public int OrderId { get; set; } // 訂單 ID
        public string OrderName { get; set; } // 訂單名稱
        public decimal TotalAmount { get; set; } // 訂單總金額
        public DateTime? OrderDate { get; set; } // 訂單日期
        public string UserName { get; set; } // 用戶名稱 (根據 UserID 對應)
        public int TotalQuantity { get; set; } // 訂單中所有產品數量的總和
        public string Keyword { get; set; } // 關鍵字 (用於篩選)
    }
}
