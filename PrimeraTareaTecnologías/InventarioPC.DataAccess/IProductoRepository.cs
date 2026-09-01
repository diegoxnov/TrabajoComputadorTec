using InventarioPC.Domain;

namespace InventarioPC.DataAccess;

public interface IProductoRepository
{
    Task<List<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<List<Producto>> ObtenerPorIdsAsync(List<int> ids);
    Task AgregarAsync(Producto producto);
    void Eliminar(Producto producto);
    Task<int> GuardarCambiosAsync();
}
