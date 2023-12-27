using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.RespuestaEnvio
{
    public class Resultado
    {
        [XmlAttribute("ID")]
        public string Id { get; set; }

        /// <summary>
        /// Información de identificación del envío de resultados.
        /// </summary>
        [XmlElement("Caratula")]
        public Caratula Caratula { get; set; }

        /// <summary>
        /// Restultados de la recepción de envíos de documentos.
        /// </summary>
        [XmlElement("RecepcionEnvio")]
        public List<RecepcionEnvio> RecepcionEnvio { get; set; }

        /// <summary>
        /// Resultados de la aprobación comercial de documentos.
        /// </summary>
        [XmlElement("ResultadoDTE")]
        public List<ResultadoDTE> ResultadoDTE { get; set; }

        public string Firmar(string nombreCertificado)
        {
            this.Caratula.Fecha = DateTime.Now;
            string filePath = "";

            string xmlContent = Engine.XML.XmlHandler.Serialize<Resultado>(this, Engine.XML.SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false);           
            var content = ITA_CHILE.Security.Firma.Firma.FirmarDocumentoPath(filePath, Id.ToString(), nombreCertificado);
            return filePath;
        }
    }
}
