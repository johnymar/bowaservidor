using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ItaSystem.DTE.Engine.Helpers;

namespace ItaSystem.DTE.Engine.RespuestaEnvio
{
    public class Caratula
    {
        [XmlAttribute("version")]
        public string version { get { return Config.Resources.versionRespuestaEnvio; } set { } }

        /// <summary>
        /// RUT que genera la respuesta (RUT receptor de los DTE).
        /// </summary>
        [XmlElement("RutResponde")]
        public string RutResponde { get; set; }

        /// <summary>
        /// RUT al que se le envia la respuesta (RUT emisor de los DTE).
        /// </summary>
        [XmlElement("RutRecibe")]
        public string RutRecibe { get; set; }

        /// <summary>
        /// Número único de identificación de la respuesta.
        /// </summary>
        [XmlElement("IdRespuesta")]
        public int IdRespuesta { get; set; }

        /// <summary>
        /// Número de envíos en sección de recepción o número de DTE en sección de resultados.
        /// </summary>
        [XmlElement("NroDetalles")]
        public int NumeroDetalles { get; set; }

        [XmlIgnore]
        private string _nombreContacto;
        /// <summary>
        /// Persona de contacto para aclarar dudas.
        /// </summary>
        [XmlElement("NmbContacto")]
        public string NombreContacto { get { return _nombreContacto.Truncate(40); } set { _nombreContacto = value; } }

        [XmlIgnore]
        private string _fonoContacto;

        /// <summary>
        /// Teléfono de contacto.
        /// </summary>
        [XmlElement("FonoContacto")]
        public string FonoContacto { get { return _fonoContacto.Truncate(40); } set { _fonoContacto = value; } }

        [XmlIgnore]
        private string _mailContacto;

        /// <summary>
        /// Correo electrónico de contacto.
        /// </summary>
        [XmlElement("MailContacto")]
        public string MailContacto { get { return _mailContacto.Truncate(80); } set { _mailContacto = value; } }

        /// <summary>
        /// Fecha y hora de la firma del archivo de respuesta.
        /// </summary>
        [XmlIgnore]
        public DateTime Fecha { get { return DateTime.Parse(FechaString); } set { this.FechaString = value.ToString(Config.Resources.DateTimeFormat); } }

        /// <summary>
        /// Fecha y hora de la firma del archivo de respuesta.
        /// Do no set this property, set Fecha instead.
        /// </summary>
        [XmlElement("TmstFirmaResp")]
        public string FechaString { get; set; }
    }
}
