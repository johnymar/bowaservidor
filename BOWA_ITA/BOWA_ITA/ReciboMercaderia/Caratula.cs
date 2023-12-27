using ItaSystem.DTE.Engine.Helpers;
using System;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.ReciboMercaderia
{
    public class Caratula
    {
        [XmlAttribute("version")]
        public string Version { get { return Config.Resources.versionCaratulaRecibo; } set { } }

        /// <summary>
        /// RUT que genera este Recibo (RUT Receptor del Documento)
        /// </summary>
        [XmlElement("RutResponde")]
        public string RutResponde { get; set; }
        public bool ShouldSerializeRutResponde() { return true; }

        /// <summary>
        /// RUT al que se le envian los Recibos (RUT Emisor de los Documento)
        /// </summary>
        [XmlElement("RutRecibe")]
        public string RutRecibe { get; set; }
        public bool ShouldSerializeRutRecibe() { return true; }

        [XmlIgnore]
        private string _nombreContacto;
        /// <summary>
        /// Persona de Contacto para aclarar dudas
        /// </summary>
        [XmlElement("NmbContacto")]
        public string NombreContacto { get { return _nombreContacto.Truncate(40); } set { this._nombreContacto = value; } }
        public bool ShouldSerializeNombreContacto() { return !string.IsNullOrEmpty(NombreContacto); }

        [XmlIgnore]
        private string _fonoContacto;
        /// <summary>
        /// Telefono de Contacto
        /// </summary>
        [XmlElement("FonoContacto")]
        public string FonoContacto { get { return _fonoContacto.Truncate(40); } set { this._fonoContacto = value; } }
        public bool ShouldSerializeFonoContacto() { return !string.IsNullOrEmpty(FonoContacto); }

        [XmlIgnore]
        private string _mailContacto;
        /// <summary>
        /// Correo Electronico de Contacto
        /// </summary>
        [XmlElement("MailContacto")]
        public string MailContacto { get { return _mailContacto.Truncate(80); } set { this._mailContacto = value; } }
        public bool ShouldSerializeMailContacto() { return !string.IsNullOrEmpty(MailContacto); }

        /// <summary>
        /// Fecha y Hora de la Firma del Archivo de EnvioRecibo.
        /// (AAAA-MM-DDTHH:MM:SS
        /// No not set this property, set FechaHoraFirma instead.
        /// </summary>
        [XmlElement("TmstFirmaEnv")]
        public string FechaHoraFirmaString { get; set; }

        /// <summary>
        /// Fecha y Hora de la Firma del Archivo de EnvioRecibo.
        /// (AAAA-MM-DDTHH:MM:SS
        /// </summary>
        [XmlIgnore]
        public DateTime FechaHoraFirma { get { return DateTime.Parse(FechaHoraFirmaString); } set { FechaHoraFirmaString = value.ToString(Config.Resources.DateTimeFormat); } }

    }
}