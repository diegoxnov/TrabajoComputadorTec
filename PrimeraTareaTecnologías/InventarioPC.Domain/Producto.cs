namespace InventarioPC.Domain;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public Categoria Categoria { get; set; }
    public string Marca { get; set; } = "";
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}
