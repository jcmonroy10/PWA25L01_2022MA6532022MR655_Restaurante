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

        //Eliminar
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPlato(int id)
        {
            Plato? plato = (from p in _restauranteContexto.Platos where p.PlatoId ==id select p).FirstOrDefault();

            if (plato == null)
            {
                return NotFound();
            }

            _restauranteContexto.Platos.Attach(plato);
            _restauranteContexto.Platos.Remove(plato);
            _restauranteContexto.SaveChanges();

            return Ok();
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
