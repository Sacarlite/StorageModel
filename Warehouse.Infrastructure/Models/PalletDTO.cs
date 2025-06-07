namespace Warehouse.Infrastructure.Models
{
    public class PalletDTO
    {
        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public List<BoxDTO> Boxes { get; set; } = new();
    }
}
