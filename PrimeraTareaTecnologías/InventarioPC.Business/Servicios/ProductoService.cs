using InventarioPC.Business.Dtos;
using InventarioPC.DataAccess;
using InventarioPC.Domain;

namespace InventarioPC.Business.Servicios;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductoDto>> ObtenerTodosAsync()
    {
        var productos = await _repository.ObtenerTodosAsync();
        return productos.Select(MapearADto).ToList();
    }

    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        var producto = await _repository.ObtenerPorIdAsync(id);
        return producto is null ? null : MapearADto(producto);
    }

    public async Task<ProductoDto> CrearAsync(ProductoCrearDto dto)
    {
        ValidarPrecioYStock(dto.Precio, dto.Stock);

        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Categoria = dto.Categoria,
            Marca = dto.Marca,
            Precio = dto.Precio,
            Stock = dto.Stock
        };

        await _repository.AgregarAsync(producto);
        await _repository.GuardarCambiosAsync();

        return MapearADto(producto);
    }

    public async Task<bool> ActualizarAsync(int id, ProductoCrearDto dto)
    {
        ValidarPrecioYStock(dto.Precio, dto.Stock);

        var producto = await _repository.ObtenerPorIdAsync(id);
        if (producto is null) return false;

        producto.Nombre = dto.Nombre;
        producto.Categoria = dto.Categoria;
        producto.Marca = dto.Marca;
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;

        await _repository.GuardarCambiosAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var producto = await _repository.ObtenerPorIdAsync(id);
        if (producto is null) return false;

        _repository.Eliminar(producto);
        await _repository.GuardarCambiosAsync();
        return true;
    }

    private static void ValidarPrecioYStock(decimal precio, int stock)
    {
        if (precio < 0) throw new ArgumentException("El precio no puede ser negativo.");
        if (stock < 0) throw new ArgumentException("El stock no puede ser negativo.");
    }

    private static ProductoDto MapearADto(Producto p) => new()
    {
        Id = p.Id,
        Nombre = p.Nombre,
        Categoria = p.Categoria,
        Marca = p.Marca,
        Precio = p.Precio,
        Stock = p.Stock
    };
}
