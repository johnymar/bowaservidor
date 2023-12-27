using ItaSystem.DTE.Engine.XML;
using ITA_CHILE.Documento;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Documento
{
    [XmlRoot("DTE", Namespace = "")]
    public class DTE
    {
        private string DTERelativeFilePath { get; set; }
        public string DTEfilepath { get { return AppDomain.CurrentDomain.BaseDirectory + DTERelativeFilePath; } }

        [XmlAttribute("version")]
        public string Version { get { return Engine.Config.Resources.versionDTE; } set { } }

        [XmlElement("Documento")]
        public Documento Documento { get; set; }
        public bool ShouldSerializeDocumento() { return !string.IsNullOrEmpty(Documento.Id); }

        [XmlElement("Exportaciones")]
        public Exportaciones Exportaciones { get; set; }
        public bool ShouldSerializeExportaciones() { return !string.IsNullOrEmpty(Exportaciones.Id); }

        //public override string ToString() {
        //    return File.ReadAllText(DTEfilepath);
        //}

        public override string ToString()
        {
            if (string.IsNullOrEmpty(DTEfilepath)) {
                string filepath = "";
                string xmlContent = XmlHandler.Serialize<DTE>(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filepath);
                this.DTERelativeFilePath = filepath;
                return xmlContent;
            }
            return File.ReadAllText(DTEfilepath, Encoding.GetEncoding("ISO-8859-1"));
        }

        public string ToDisk(string path)
        {
            string filePath = "";
            XmlHandler.Serialize<DTE>(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, null, path);
            return filePath;
        }

        public DTE()
        {
            Documento = new Documento();
            Exportaciones = new Exportaciones();
        }

        public string Firmar(string nombreCertificado, out string message, string outputDirectory = "out\\temp\\", string customName = "", string password = "")
        {
            Documento.FechaHoraFirma = DateTime.Now;
            string filePath = "";
            message = "";
            try
            {
                string xmlContent = XmlHandler.Serialize<DTE>(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, null, outputDirectory, true, "", customName);
                string newContentSigned = ITA_CHILE.Security.Firma.Firma.FirmarDocumentoContent(filePath, this.Documento.Id, nombreCertificado, xmlContent, password);
                this.DTERelativeFilePath = filePath;
               
            }
            catch (Exception ex)
            {
                filePath = ex.Message;
                message = ex.StackTrace;
            }
            return filePath;
        }

        public string FirmarExportacion(string nombreCertificado, out string message, string outputDirectory = "out\\temp\\", string customName = "", string password = "")
        {
            message = "";
            Exportaciones.FechaHoraFirma = DateTime.Now;
            string filePath = "";
            try
            {
                string xmlContent = XmlHandler.Serialize<DTE>(this, SerializationType.SerializationTypes.LineBreakNoIndent, out filePath, true, false, null, outputDirectory, true, "", customName);
                var content = ITA_CHILE.Security.Firma.Firma.FirmarDocumentoPath(filePath, this.Exportaciones.Id, nombreCertificado, password);
                this.DTERelativeFilePath = filePath;
            }
            catch (Exception ex)
            {
                filePath = ex.Message;
                message = ex.StackTrace;
            }
            return filePath;
        }
    }
}
