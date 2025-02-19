using L01_2022MA653_2022MR655.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022MA653_2022MR655.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public ClienteController(restauranteContext restauranteContexto)
        {
            _restauranteContexto = restauranteContexto;
        }

        //Leer
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Cliente> listaCliente = (from c in _restauranteContexto.Clientes select c).ToList();

            if (listaCliente.Count == 0)
            {
                return NotFound();
            }

            return Ok(listaCliente);
        }

        //Crear
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarCliente([FromBody] Cliente cliente)
        {
            try
            {
                _restauranteContexto.Clientes.Add(cliente);
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
        public IActionResult ActualizarCliente(int id, [FromBody] Cliente clienteModificar)
        {
            Cliente? clienteActual = (from c in _restauranteContexto.Clientes where c.ClienteId == id select c).FirstOrDefault();

            if (clienteActual == null)
            {
                return NotFound();
            }

            clienteActual.NombreCliente = clienteModificar.NombreCliente;
            clienteActual.Direccion = clienteModificar.Direccion;

            _restauranteContexto.Entry(clienteActual).State = EntityState.Modified;
            _restauranteContexto.SaveChanges();

            return Ok();
        }

        //Eliminar
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarCliente(int id)
        {
            Cliente? cliente = (from c in _restauranteContexto.Clientes where c.ClienteId == id select c).FirstOrDefault();

            if (cliente == null)
            {
                return NotFound();
            }

            _restauranteContexto.Clientes.Attach(cliente);
            _restauranteContexto.Clientes.Remove(cliente);
            _restauranteContexto.SaveChanges();

            return Ok();
        }

        // Filtrar clientes por dirección
        [HttpGet]
        [Route("GetByAddress/{direccion}")]
        public IActionResult GetClientesByDireccion(string direccion)
        {
            var clientes = _restauranteContexto.Clientes
                .Where(c => c.Direccion.Contains(direccion))
                .ToList();

            if (clientes.Count == 0)
            {
                return NotFound("No se encontraron clientes con esa dirección.");
            }

            return Ok(clientes);
        }


    }
}
