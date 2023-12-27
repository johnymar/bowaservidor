using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.RespuestaEnvio
{
    /// <summary>
    /// Envío de respuesta a la recepción de documentos tributarios eletrónicos.
    /// </summary>
    [XmlRoot]
    public class RespuestaDTE
    {
        /// <summary>
        /// Versión de envío DTE.
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get { return Config.Resources.versionRespuestaEnvio; } set { } }

        /// <summary>
        /// Localización del schema del XML.
        /// </summary>
        [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = "http://www.sii.cl/SiiDte RespuestaEnvioDTE_v10.xsd";

        /// <summary>
        /// Información de resultado de los proceso de recepción de envíos y documentos.
        /// </summary>
        [XmlElement("Resultado")]
        public Resultado Resultado { get; set; }

        public string Firmar(string nombreCertificado, string password = "")
        {
            Resultado.Caratula.Fecha = DateTime.Now;
            string filePath = "";

            List<string> namespaces = new List<string>();
            namespaces.Add("xsi&http://www.w3.org/2001/XMLSchema-instance");

            string xmlContent = 
                Engine.XML.XmlHandler.Serialize(this, Engine.XML.SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, namespaces);

            var content = ITA_CHILE.Security.Firma.Firma.FirmarDocumentoPath(filePath, Resultado.Id.ToString(), nombreCertificado, password);
            return filePath;
        }
    }
}
