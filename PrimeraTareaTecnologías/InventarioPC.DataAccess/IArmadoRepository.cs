using InventarioPC.Domain;

namespace InventarioPC.DataAccess;

public interface IArmadoRepository
{
    Task<List<Armado>> ObtenerTodosAsync();
    Task<Armado?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Armado armado);
    Task<int> GuardarCambiosAsync();
}
