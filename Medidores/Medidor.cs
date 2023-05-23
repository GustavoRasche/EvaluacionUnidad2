using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medidores
{
    public class Medidor
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

    public class Lectura
    {
        public int MedidorId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal ValorConsumo { get; set; }
    }
}
