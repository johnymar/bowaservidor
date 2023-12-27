using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Enum
{
    public class FormaPago
    {
        /// <summary>
        /// Indica en qué forma se pagará. En el caso de una Factura por “Entrega Gratuita”, se debe indicar el 3. Una Factura de este tipo no tiene derecho a crédito fiscal.
        /// </summary>
        public enum FormaPagoEnum : int
        {
            /// <summary>
            /// No se ha asignado un valor aún.
            /// </summary>
            [XmlEnum("")]
            NotSet = 0,
            [XmlEnum("1")]
            Contado = 1,
            [XmlEnum("2")]
            Credito = 2,
            [XmlEnum("3")]
            SinCosto = 3
        }

       
    }
}
