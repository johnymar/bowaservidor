using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaSystem.DTE.WS.EstadoDTE
{
    public class EstadoDTEAvanzadoResult
    {
        public string ResponseXml { get; set; }
        
        public int Estado { get; set; }

        public bool Ok { get { return Estado == 0; } }
        public string Glosa { get; set; }

        public bool IsRecibido { get; set; }
        public string EstadoBody { get; set; }
        public string GlosaBody { get; set; }
        public int TrackId { get; set; }

        public int NumeroAtencion { get; set; }
        public DateTime FechaAtencion { get; set; }

        public EstadoDTEAvanzadoResult()
        {
            Estado = -9999999;
        }
    }
}
