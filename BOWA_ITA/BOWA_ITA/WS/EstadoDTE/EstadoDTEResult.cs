using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaSystem.DTE.WS.EstadoDTE
{
    public class EstadoDTEResult
    {
        public string ResponseXml { get; set; }
        public string Estado { get; set; }

        public bool Ok { get { return Estado == "DOK"; } }

        public string GlosaEstado { get; set; }
        public string ERR_CODE { get; set; }
        public string GLosa_ERR_CODE { get; set; }
        
        public int NumeroAtencion { get; set; }
        public DateTime FechaAtencion { get; set; }
    }
}
