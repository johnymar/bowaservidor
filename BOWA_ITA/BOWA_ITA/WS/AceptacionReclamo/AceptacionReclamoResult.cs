using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITA_CHILE.WS.AceptacionReclamo
{
    public class AceptacionReclamoResult
    {
        public string CodRespuesta { get; set; }
        public string Descripcion { get; set; }
        public DateTime fechaRecepcion { get; set; }
        public List<DetalleEvento> detalles { get; set; }
    }

    public class DetalleEvento
    {
        public string codEvento { get; set; }
        public string descripcionEvento { get; set; }
        public string rutResponsable { get; set; }
        public string dvResponsable { get; set; }
        public DateTime FechaEvento { get; set; }
    }
}
