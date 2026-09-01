using InventarioPC.Business.Dtos;
using InventarioPC.Business.Exceptions;
using InventarioPC.Business.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace InventarioPC.Api.Controllers;

[ApiController]
[Route("api/armados")]
public class ArmadosController : ControllerBase
{
    private readonly IArmadoService _armadoService;

    public ArmadosController(IArmadoService armadoService)
    {
        _armadoService = armadoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ArmadoDto>>> ObtenerTodos() =>
        Ok(await _armadoService.ObtenerTodosAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ArmadoDto>> ObtenerPorId(int id)
    {
        var armado = await _armadoService.ObtenerPorIdAsync(id);
        return armado is null ? NotFound() : Ok(armado);
    }

    [HttpPost]
    public async Task<ActionResult<ArmadoDto>> Crear(ArmadoCrearDto dto)
    {
        try
        {
            var creado = await _armadoService.CrearArmadoAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
        }
        catch (StockInsuficienteException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
