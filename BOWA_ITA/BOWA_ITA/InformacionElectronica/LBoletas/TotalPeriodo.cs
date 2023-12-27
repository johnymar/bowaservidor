using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LBoletas
{
    public class TotalPeriodo
    {
        /// <summary>
        /// Para todos los tipos de documentos que se han informado en el archivo de detalle.
        /// Para todos los tipos de documentos que se han informado en el mes.
        /// Se debe agregar el resumen para los documentos correspondientes a Boletas y Resumen pasajes. 
        /// Códigos(35, 38, 39, 41, 105, 500, 501, 919, 920, 922 y 924)
        /// </summary>
        [XmlElement("TpoDoc")]
        public Enum.TipoDTE.TipoDocumentoLibro TipoDocumento { get; set; }
        public bool ShouldSerializeTipoDocumento() { return true; }

        /// <summary>
        /// Número de documentos anulados.
        /// Cantidad de documentos cuyos folios han sido anulados, previo al envío al SII. 
        /// Estos documentos no se totalizan en los campos siguientes.
        /// En el caso de los documentos manuales y de las Boletas Electrónicas , se registran todos los folios anulados.
        /// No se contabiliza aquí si un documento electrónico ha sido anulado, posterior al envío al SII, con Una Nota de Crédito o Debito.
        /// Esos documentos deben ser informados como emitidos, con todos los datos que correspondan.
        /// </summary>
        [XmlElement("TotAnulado")]
        public int CantidadDocumentosAnulados { get; set; }
        public bool ShouldSerializeCantidadDocumentosAnulados() { return CantidadDocumentosAnulados != 0; }

        [XmlElement("TotalesServicio")]
        public List<TotalServicio> TotalesServicio { get; set; }
        public bool ShouldSerializeTotalesServicio() { return TotalesServicio.Count != 0; }


        public TotalPeriodo()
        {
            TotalesServicio = new List<TotalServicio>();
        }
    }
}
