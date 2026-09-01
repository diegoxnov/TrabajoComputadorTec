using InventarioPC.Business.Dtos;
using InventarioPC.Business.Servicios;
using InventarioPC.Domain;
using Microsoft.AspNetCore.Mvc;

namespace InventarioPC.Api.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> ObtenerTodos() =>
        Ok(await _productoService.ObtenerTodosAsync());

    [HttpGet("categorias")]
    public ActionResult<IEnumerable<string>> ObtenerCategorias() =>
        Ok(Enum.GetNames<Categoria>());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoDto>> ObtenerPorId(int id)
    {
        var producto = await _productoService.ObtenerPorIdAsync(id);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Crear(ProductoCrearDto dto)
    {
        try
        {
            var creado = await _productoService.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, ProductoCrearDto dto)
    {
        try
        {
            var actualizado = await _productoService.ActualizarAsync(id, dto);
            return actualizado ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _productoService.EliminarAsync(id);
        return eliminado ? NoContent() : NotFound();
    }
}
