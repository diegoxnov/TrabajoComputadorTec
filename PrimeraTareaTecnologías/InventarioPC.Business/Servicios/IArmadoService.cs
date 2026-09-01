using InventarioPC.Business.Dtos;

namespace InventarioPC.Business.Servicios;

public interface IArmadoService
{
    Task<List<ArmadoDto>> ObtenerTodosAsync();
    Task<ArmadoDto?> ObtenerPorIdAsync(int id);
    Task<ArmadoDto> CrearArmadoAsync(ArmadoCrearDto dto);
}
