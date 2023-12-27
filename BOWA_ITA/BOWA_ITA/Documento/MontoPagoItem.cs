using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ItaSystem.DTE.Engine.Helpers;

namespace ItaSystem.DTE.Engine.Documento
{
    public class MontoPagoItem
    {
        /// <summary>
        /// Fecha de pago programado. AAAA-MM-DD
        /// </summary>
        [XmlElement("FchPago")]
        public string FechaPago { get; set; }

        /// <summary>
        /// Monto de pago programado.
        /// </summary>
        [XmlElement("MntPago")]
        public int MontoPago { get; set; }

        [XmlIgnore]
        private string _glosa;
        /// <summary>
        /// Glosa adicional para calificar pago
        /// </summary>
        [XmlElement("GlosaPagos")]
        public string GlosaDescripcion { get { return _glosa.Truncate(40); } set { _glosa = value; } }
    }
}
