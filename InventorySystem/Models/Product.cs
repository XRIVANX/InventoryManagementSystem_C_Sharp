namespace InventorySystem.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public int UomID { get; set; }
        public int? SupplierID { get; set; }
        public decimal ReorderLevel { get; set; }
        public decimal ReorderQty { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsBatchTracked { get; set; }
        public bool IsActive { get; set; }
    }
}