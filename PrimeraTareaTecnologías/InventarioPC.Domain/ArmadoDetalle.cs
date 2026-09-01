namespace InventarioPC.Domain;

public class ArmadoDetalle
{
    public int Id { get; set; }
    public int ArmadoId { get; set; }
    public Armado Armado { get; set; } = null!;
    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;
    public int Cantidad { get; set; } = 1;
}
