using L01_2022MA653_2022MR655.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L01_2022MA653_2022MR655.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContext;

        public PedidosController(restauranteContext contextRestaurante)
        {
            _restauranteContext = contextRestaurante;
        }

        // GET: api/Pedidos/GetAll
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Pedido> listadoPedidos = _restauranteContext.Pedidos.ToList();
            if (listadoPedidos.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoPedidos);
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var pedido = _restauranteContext.Pedidos.FirstOrDefault(p => p.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        // POST: api/Pedidos
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPedido([FromBody] Plato plato)
        {
            try
            {
                _restauranteContext.Platos.Add(plato);
                _restauranteContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Pedidos/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Pedido pedido)
        {
            Pedido? pedidoExistente = (from p in _restauranteContext.Pedidos where p.PedidoId == id select p).FirstOrDefault();

            if (pedidoExistente == null)
            {
                return NotFound();
            }

            pedidoExistente.MotoristaId = pedido.MotoristaId;
            pedidoExistente.ClienteId = pedido.ClienteId;
            pedidoExistente.PlatoId = pedido.PlatoId;
            pedidoExistente.Cantidad = pedido.Cantidad;
            pedidoExistente.Precio = pedido.Precio;

            _restauranteContext.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Pedidos/5
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPlato(int id)
        {
            Pedido? pedido = (from p in _restauranteContext.Pedidos where p.PedidoId == id select p).FirstOrDefault();

            if (pedido == null)
            {
                return NotFound();
            }

            _restauranteContext.Pedidos.Remove(pedido);
            _restauranteContext.SaveChanges();

            return NoContent();
        }
    }
}
