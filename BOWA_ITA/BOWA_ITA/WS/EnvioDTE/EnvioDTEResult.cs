using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaSystem.DTE.WS.EnvioDTE
{
    public class EnvioDTEResult
    {
        public string RutEnvia { get; set; }
        public string RutEmpresa { get; set; }
        public string File { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }

        public bool Ok { get { return true; } }

        public string Glosa { get { return DecodeEstado(); } }
        
        public string Errores { get; set; }
        public long TrackId { get; set; }

        public string ResponseXml { get; set; }

        private string DecodeEstado()
        {
            switch(Estado)
            {
                case "ex":
                    return Errores;
                case "0":
                    return "Upload OK";
                case "1":
                    return "El Sender no tiene permiso para enviar";
                case "2":
                    return "Error en el tamaño del archivo (muy grande o muy chico)";
                case "3":
                    return "Archivo cortado (tamaño <> al parámetro size.";
                case "5":
                    return "No está autenticado";
                case "6":
                    return "Empresa no autorizada a enviar archivos.";
                case "7":
                    return "Schema inválido." + Errores;
                case "8":
                    return "Firma del documento.";
                case "9":
                    return "Sistema bloqueado.";
                default:
                    return "";
            }
        }
        public EnvioDTEResult()
        {
            TrackId = -999999;
        }

    }
}
