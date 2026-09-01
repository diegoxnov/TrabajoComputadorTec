using InventarioPC.Business.Dtos;

namespace InventarioPC.Business.Servicios;

public interface IProductoService
{
    Task<List<ProductoDto>> ObtenerTodosAsync();
    Task<ProductoDto?> ObtenerPorIdAsync(int id);
    Task<ProductoDto> CrearAsync(ProductoCrearDto dto);
    Task<bool> ActualizarAsync(int id, ProductoCrearDto dto);
    Task<bool> EliminarAsync(int id);
}
