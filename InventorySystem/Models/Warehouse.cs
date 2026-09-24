namespace InventorySystem.Models
{
    public class Warehouse
    {
        public int WarehouseID { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
    }
}