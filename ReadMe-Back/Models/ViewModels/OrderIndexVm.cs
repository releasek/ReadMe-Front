namespace ReadMe_Back.Models.ViewModels
{
    public class OrderIndexVm
    {
        public int Year { get; set; }            //年分
        public int Quarter { get; set; }         // 季度，例如 1 (Q1), 2 (Q2)
        public decimal TotalAmount { get; set; } // 總銷售金額
        public int TotalQuantity { get; set; }   // 總銷售數量
    }
}
