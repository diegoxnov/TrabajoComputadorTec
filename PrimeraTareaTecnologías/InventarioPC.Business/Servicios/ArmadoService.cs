using InventarioPC.Business.Dtos;
using InventarioPC.Business.Exceptions;
using InventarioPC.DataAccess;
using InventarioPC.Domain;

namespace InventarioPC.Business.Servicios;

public class ArmadoService : IArmadoService
{
    private readonly IArmadoRepository _armadoRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly InventarioPcDbContext _context;

    public ArmadoService(
        IArmadoRepository armadoRepository,
        IProductoRepository productoRepository,
        InventarioPcDbContext context)
    {
        _armadoRepository = armadoRepository;
        _productoRepository = productoRepository;
        _context = context;
    }

    public async Task<List<ArmadoDto>> ObtenerTodosAsync()
    {
        var armados = await _armadoRepository.ObtenerTodosAsync();
        return armados.Select(a => MapearADto(a)).ToList();
    }

    public async Task<ArmadoDto?> ObtenerPorIdAsync(int id)
    {
        var armado = await _armadoRepository.ObtenerPorIdAsync(id);
        return armado is null ? null : MapearADto(armado);
    }

    public async Task<ArmadoDto> CrearArmadoAsync(ArmadoCrearDto dto)
    {
        if (dto.ProductoIds is null || dto.ProductoIds.Count == 0)
            throw new StockInsuficienteException("Debe seleccionar al menos un producto.");

        var productos = await _productoRepository.ObtenerPorIdsAsync(dto.ProductoIds);

        if (productos.Count != dto.ProductoIds.Count)
            throw new StockInsuficienteException("Uno o más productos seleccionados no existen.");

        var sinStock = productos.Where(p => p.Stock < 1).ToList();
        if (sinStock.Count > 0)
        {
            var nombres = string.Join(", ", sinStock.Select(p => p.Nombre));
            throw new StockInsuficienteException($"Sin stock disponible para: {nombres}.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        var armado = new Armado
        {
            Nombre = dto.Nombre,
            FechaCreacion = DateTime.UtcNow,
            PrecioTotal = productos.Sum(p => p.Precio)
        };

        foreach (var producto in productos)
        {
            armado.Detalles.Add(new ArmadoDetalle
            {
                ProductoId = producto.Id,
                Cantidad = 1
            });
            producto.Stock -= 1;
        }

        await _armadoRepository.AgregarAsync(armado);
        await _armadoRepository.GuardarCambiosAsync();
        await transaction.CommitAsync();

        var armadoCreado = await _armadoRepository.ObtenerPorIdAsync(armado.Id);
        return MapearADto(armadoCreado!);
    }

    private static ArmadoDto MapearADto(Armado a) => new()
    {
        Id = a.Id,
        Nombre = a.Nombre,
        FechaCreacion = a.FechaCreacion,
        PrecioTotal = a.PrecioTotal,
        Detalles = a.Detalles.Select(d => new ArmadoDetalleDto
        {
            ProductoId = d.ProductoId,
            ProductoNombre = d.Producto?.Nombre ?? "",
            Precio = d.Producto?.Precio ?? 0,
            Cantidad = d.Cantidad
        }).ToList()
    };
}
