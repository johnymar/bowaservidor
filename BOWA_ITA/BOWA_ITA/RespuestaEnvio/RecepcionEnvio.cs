using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.RespuestaEnvio
{
    public class RecepcionEnvio
    {
        /// <summary>
        /// Nombre del archivo de envío.
        /// </summary>
        [XmlElement("NmbEnvio")]
        public string NombreArchivoEnvio { get; set; }

        /// <summary>
        /// Fecha y hora de rececpcion del envío.
        /// </summary>
        [XmlIgnore]
        public DateTime FechaRecepcion { get { return DateTime.Parse(FechaRecepcionString); } set { this.FechaRecepcionString = value.ToString(Config.Resources.DateTimeFormat); } }

        /// <summary>
        /// Nombre del archivo de envío.
        /// Do not set this property, set FechaRecepcion instead.
        /// </summary>
        [XmlElement("FchRecep")]
        public string FechaRecepcionString { get; set; }

        /// <summary>
        /// Número único (Generado por el receptor) para identificar el envío.
        /// </summary>
        [XmlElement("CodEnvio")]
        public int CodigoEnvio { get; set; }

        /// <summary>
        /// Valor del atributo ID del tag &lt;EnvioDTE&gt; del envío.
        /// </summary>
        [XmlElement("EnvioDTEID")]
        public string EnvioDTEId { get; set; }

        /// <summary>
        /// Campo DigestValue de la firma digital del envío.
        /// </summary>
        [XmlElement("Digest")]
        public string Digest { get; set; }

        /// <summary>
        /// RUT del emisor informado en la carátula del envío.
        /// </summary>
        [XmlElement("RutEmisor")]
        public string RutEmisor { get; set; }

        /// <summary>
        /// RUT del receptor informado en la carátula del envío.
        /// </summary>
        [XmlElement("RutReceptor")]
        public string RutReceptor { get; set; }

        /// <summary>
        /// Estado de recepción del envío.
        /// </summary>
        [XmlElement("EstadoRecepEnv")]
        public Enum.EstadoEnvioEmpresa.EstadoEnvioEmpresaEnum EstadoRecepcionEnvio { get; set; }

        /// <summary>
        /// Información adicional para el estado de recepción.
        /// </summary>
        [XmlElement("RecepEnvGlosa")]
        public string GlosaEstadoRecepcionEnvio { get { return Enum.EstadoEnvioEmpresa.Glosa(EstadoRecepcionEnvio); } set { } }

        /// <summary>
        /// Número de documentos incluidos en el envio que se informan como recibidos/no recibidos.
        /// </summary>
        [XmlElement("NroDTE")]
        public int NumeroDTE { get; set; }

        /// <summary>
        /// Resultados de recepción de los documentos incluidos en el envío.
        /// </summary>
        [XmlElement("RecepcionDTE")]
        public List<RecepcionDTE> RecepcionDTE { get; set; }
    }
}
