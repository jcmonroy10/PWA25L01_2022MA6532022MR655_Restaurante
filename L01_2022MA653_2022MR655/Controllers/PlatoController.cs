using L01_2022MA653_2022MR655.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022MA653_2022MR655.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatoController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public PlatoController(restauranteContext restauranteContexto)
        {
            _restauranteContexto = restauranteContexto;
        }

        //Leer
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Plato> listaPlato = (from p in _restauranteContexto.Platos select p).ToList();

            if(listaPlato.Count == 0)
            {
                return NotFound();
            }

            return Ok(listaPlato);
        }

        //Leer por ID
        [HttpGet("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var pedido = _restauranteContexto.Platos.FirstOrDefault(p => p.PlatoId == id);
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        //Crear
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPlato([FromBody] Plato plato)
        {
            try
            {
                _restauranteContexto.Platos.Add(plato);
                _restauranteContexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        //Actualizar
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPlato(int id, [FromBody] Plato platoModificar)
        {
            Plato? platoActual = (from p in _restauranteContexto.Platos where p.PlatoId == id select p).FirstOrDefault();

            if(platoActual == null)
            {
                return NotFound();
            }

            platoActual.NombrePlato = platoModificar.NombrePlato;
            platoActual.Precio = platoModificar.Precio;

            _restauranteContexto.Entry(platoActual).State = EntityState.Modified;
            _restauranteContexto.SaveChanges();

            return Ok();    
        }

        //Eliminar cascada
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPlato(int id)
        {
            using var transaction = _restauranteContexto.Database.BeginTransaction();

            try
            {
                Plato? plato = _restauranteContexto.Platos.FirstOrDefault(p => p.PlatoId == id);

                if (plato == null)
                {
                    return NotFound();
                }

                var pedidosAsociados = _restauranteContexto.Pedidos.Where(p => p.PlatoId == id).ToList();
                if (pedidosAsociados.Any())
                {
                    _restauranteContexto.Pedidos.RemoveRange(pedidosAsociados);
                }

                _restauranteContexto.Platos.Remove(plato);

                _restauranteContexto.SaveChanges();

                transaction.Commit();

                return Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Error al eliminar el plato: {ex.Message}");
            }
        }

        // Filtrar platos por nombre
        [HttpGet]
        [Route("GetByName/{nombre}")]
        public IActionResult GetPlatoPorNombre(string nombre)
        {
            var plato = _restauranteContexto.Platos
                .Where(p => p.NombrePlato.Contains(nombre))
                .ToList();

            if (plato.Count == 0)
            {
                return NotFound("No se encontraron platos con ese nombre.");
            }

            return Ok(plato);
        }

    }
}
