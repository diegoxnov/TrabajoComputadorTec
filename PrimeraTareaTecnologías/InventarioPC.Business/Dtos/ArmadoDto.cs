namespace InventarioPC.Business.Dtos;

public class ArmadoCrearDto
{
    public string Nombre { get; set; } = "";
    public List<int> ProductoIds { get; set; } = new();
}

public class ArmadoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public DateTime FechaCreacion { get; set; }
    public decimal PrecioTotal { get; set; }
    public List<ArmadoDetalleDto> Detalles { get; set; } = new();
}

public class ArmadoDetalleDto
{
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = "";
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
}
