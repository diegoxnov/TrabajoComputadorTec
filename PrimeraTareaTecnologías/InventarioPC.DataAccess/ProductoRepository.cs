using InventarioPC.Domain;
using Microsoft.EntityFrameworkCore;

namespace InventarioPC.DataAccess;

public class ProductoRepository : IProductoRepository
{
    private readonly InventarioPcDbContext _context;

    public ProductoRepository(InventarioPcDbContext context)
    {
        _context = context;
    }

    public Task<List<Producto>> ObtenerTodosAsync() =>
        _context.Productos.AsNoTracking().ToListAsync();

    public Task<Producto?> ObtenerPorIdAsync(int id) =>
        _context.Productos.FirstOrDefaultAsync(p => p.Id == id);

    public Task<List<Producto>> ObtenerPorIdsAsync(List<int> ids) =>
        _context.Productos.Where(p => ids.Contains(p.Id)).ToListAsync();

    public async Task AgregarAsync(Producto producto) =>
        await _context.Productos.AddAsync(producto);

    public void Eliminar(Producto producto) =>
        _context.Productos.Remove(producto);

    public Task<int> GuardarCambiosAsync() =>
        _context.SaveChangesAsync();
}
