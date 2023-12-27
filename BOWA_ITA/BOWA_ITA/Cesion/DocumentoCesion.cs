using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ITA_CHILE.Cesion
{
    public class DocumentoCesion
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlElement("SeqCesion")]
        public int Secuencia { get; set; }
        //public bool ShouldSerializeRutCedente() { return true; }

        [XmlElement("IdDTE")]
        public IdDTE IdentificacionDTE { get; set; }

        [XmlElement("Cedente")]
        public Cedente Cedente { get; set; }

        [XmlElement("Cesionario")]
        public Cesionario Cesionario { get; set; }

        [XmlElement("MontoCesion")]
        public int MontoCesion { get; set; }

        [XmlElement("UltimoVencimiento")]
        public string UltimoVencimientoString { get; set; }

        [XmlIgnore]
        public DateTime UltimoVencimiento { get { return DateTime.Parse(UltimoVencimientoString); } set { UltimoVencimientoString = value.ToString(ItaSystem.DTE.Engine.Config.Resources.DateFormat); } }

        [XmlElement("OtrasCondiciones")]
        public string OtrasCondiciones { get; set; }

        [XmlElement("eMailDeudor")]
        public string eMailDeudor { get; set; }

        [XmlElement("TmstCesion")]
        public string TmstCesionString { get; set; }

        [XmlIgnore]
        public DateTime TmstCesion { get { return DateTime.Parse(TmstCesionString); } set { TmstCesionString = value.ToString(ItaSystem.DTE.Engine.Config.Resources.DateTimeFormat); } }


        public DocumentoCesion(ItaSystem.DTE.Engine.Documento.DTE dteCedido, int secuencia)
        {
            ID = "CesionID_" + dteCedido.Documento.Id;
            IdentificacionDTE = new IdDTE()
            {
                FechaEmision = dteCedido.Documento.Encabezado.IdentificacionDTE.FechaEmision,
                Folio = dteCedido.Documento.Encabezado.IdentificacionDTE.Folio,
                MontoTotal = dteCedido.Documento.Encabezado.Totales.MontoTotal,
                RUTEmisor = dteCedido.Documento.Encabezado.Emisor.Rut,
                RUTReceptor = dteCedido.Documento.Encabezado.Receptor.Rut,
                TipoDTE = dteCedido.Documento.Encabezado.IdentificacionDTE.TipoDTE
            };
            Cedente = new Cedente();
            Cesionario = new Cesionario();
            Secuencia = secuencia;

            MontoCesion = dteCedido.Documento.Encabezado.Totales.MontoTotal;
            UltimoVencimiento = dteCedido.Documento.Encabezado.IdentificacionDTE.FechaEmision;
            TmstCesion = DateTime.Now;        
        }

        public DocumentoCesion()
        {
            IdentificacionDTE = new IdDTE();
            Cedente = new Cedente();
            Cesionario = new Cesionario();
        }

    }
}
