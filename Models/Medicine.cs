namespace MedicineStoreAPI.Models
{
    public class Medicine
    {
        public int MedicineId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
