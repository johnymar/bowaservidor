using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaSystem.DTE.WS.EstadoEnvio
{
    public class EstadoEnvioBoletaResult
    {
        [JsonProperty("rut_emisor")]
        public string RutEmisor { get; set; }
        [JsonProperty("rut_envia")]
        public string RutEnvia { get; set; }
        public long TrackId { get; set; }
        [JsonProperty("fecha_recepcion")]
        public string FechaRecepcion { get; set; }
        public string Estado { get; set; }

        public string Response { get; set; }
        public List<EstadoEnvioBoletaEstadistica> Estadistica { get; set; }

        [JsonProperty("detalle_rep_rech")]
        public List<EstadoEnvioBoletaDetalleReparosYRechazos> Detalles { get; set; }

        //      "rut_envia": "8315495-0",
        //"trackid": 1014,
        //"fecha_recepcion": "30/07/2020 07:57:42",
        //"estado": "EPR",
    }

    public class EstadoEnvioBoletaEstadistica
    {
        public int ? Tipo { get; set; }
        public int ? Informados { get; set; }
        public int ? Aceptados { get; set; }
        public int ?  Rechazados { get; set; }
        public int ? Reparos { get; set; }
    }

    public class EstadoEnvioBoletaDetalleReparosYRechazos
    {
        public int ? Tipo { get; set; }
        public int ? Folio { get; set; }
        public string Estado { get; set; }
        public string Descripcion { get; set; }
        [JsonProperty("error")]
        public List<EstadoEnvioBoletaError> Errores { get; set; }
    }

    public class EstadoEnvioBoletaError
    {
        [JsonProperty("seccion")]
        public string Seccion { get; set; }
        public int ? Linea { get; set; }
        public int ? Nivel { get; set; }
        public int ? Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Detalle { get; set; }
    }
}
