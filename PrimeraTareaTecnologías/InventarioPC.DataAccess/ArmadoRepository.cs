using InventarioPC.Domain;
using Microsoft.EntityFrameworkCore;

namespace InventarioPC.DataAccess;

public class ArmadoRepository : IArmadoRepository
{
    private readonly InventarioPcDbContext _context;

    public ArmadoRepository(InventarioPcDbContext context)
    {
        _context = context;
    }

    public Task<List<Armado>> ObtenerTodosAsync() =>
        _context.Armados
            .AsNoTracking()
            .Include(a => a.Detalles)
            .ThenInclude(d => d.Producto)
            .ToListAsync();

    public Task<Armado?> ObtenerPorIdAsync(int id) =>
        _context.Armados
            .Include(a => a.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task AgregarAsync(Armado armado) =>
        await _context.Armados.AddAsync(armado);

    public Task<int> GuardarCambiosAsync() =>
        _context.SaveChangesAsync();
}
