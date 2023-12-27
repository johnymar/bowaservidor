using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Documento
{
    [XmlRoot("TED")]
    public class TimbreElectronico
    {
        /// <summary>
        /// Versión
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get { return Config.Resources.versionTED; } set { } }

        /// <summary>
        /// Datos básicos del documento.
        /// </summary>
        [XmlElement("DD")]
        public DatosBasicos DatosBasicos { get; set; }

        /// <summary>
        /// Valor de la firma digital sobre los datos básicos.
        /// </summary>
        [XmlElement("FRMT")]
        public FirmaDigital FirmaDigital { get; set; }

        internal void Firmar(string _CAFFilePath, bool escribeArchivo = false, string tempFilePath = "out\\temp\\")
        {
            //this.DatosBasicos.CAF.Datos.RazonSocialEmisor = SecurityElement.Escape(this.DatosBasicos.CAF.Datos.RazonSocialEmisor);
            string xml = XML.XmlHandler.Serialize<DatosBasicos>(DatosBasicos, XML.SerializationType.SerializationTypes.Inline, false, null, tempFilePath, escribeArchivo);
            //xml = xml.Replace("O\"H", "O&quot;H");
            string privateKey = CAFHandler.CAFHandler.GetPrivateKey(_CAFFilePath);


            this.FirmaDigital = new FirmaDigital();
            this.FirmaDigital.Firma = ITA_CHILE.Security.Timbre.Timbre.Timbrar(xml, privateKey);
            //this.DatosBasicos.CAF.Datos.RazonSocialEmisor = SecurityElement.Escape(this.DatosBasicos.CAF.Datos.RazonSocialEmisor);
            //this.DatosBasicos.CAF.Datos.RazonSocialEmisor = this.DatosBasicos.CAF.Datos.RazonSocialEmisor.Replace("&quot;", "\"");
        }

        public void Verificar(string xml, string _CAFFilePath)
        {
            string privateKey = CAFHandler.CAFHandler.GetPrivateKey(_CAFFilePath);
            //ITA_CHILE.Security.Timbre.Timbre.VerificarTimbre(xml, _CAFFilePath, )
        }

        public override string ToString()
        {
            string xml = XML.XmlHandler.Serialize<TimbreElectronico>(this, XML.SerializationType.SerializationTypes.Inline, false, null, "", false);

            return xml;
        }
    }
}
