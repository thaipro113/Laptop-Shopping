namespace CNWEB22CT1.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public DateTime? ApprovedDate { get; set; } // Thêm trường thời gian duyệt
    }
}
