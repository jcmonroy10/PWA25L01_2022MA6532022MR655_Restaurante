namespace L01_2022MA653_2022MR655.Models
{
    public class Pedido
    {
        public int PedidoId { get; set; }

        public int? MotoristaId { get; set; }

        public int? ClienteId { get; set; }

        public int? PlatoId { get; set; }

        public int? Cantidad { get; set; }

        public decimal? Precio { get; set; }
    }
}
