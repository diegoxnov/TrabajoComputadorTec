namespace InventarioPC.Domain;

public class Armado
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public DateTime FechaCreacion { get; set; }
    public decimal PrecioTotal { get; set; }
    public List<ArmadoDetalle> Detalles { get; set; } = new();
}
