using InventarioPC.Domain;

namespace InventarioPC.Business.Dtos;

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public Categoria Categoria { get; set; }
    public string Marca { get; set; } = "";
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}

public class ProductoCrearDto
{
    public string Nombre { get; set; } = "";
    public Categoria Categoria { get; set; }
    public string Marca { get; set; } = "";
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}
