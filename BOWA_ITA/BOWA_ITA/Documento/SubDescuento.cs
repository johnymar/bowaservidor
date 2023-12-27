using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Documento
{
    public class SubDescuento
    {
        /// <summary>
        /// Indica en qué está expresado el descuento, en porcentaje (%) o pesos ($).
        /// </summary>
        [XmlElement("TipoDscto")]
        public Enum.ExpresionDinero.ExpresionDineroEnum TipoDescuento { get; set; }

        [XmlIgnore]
        private double _valorDescuento;
        /// <summary>
        /// Valor de subdescuento.
        /// </summary>
        [XmlElement("ValorDscto")]
        public double ValorDescuento { get { return Math.Round(_valorDescuento, 2); } set { _valorDescuento = value; } }

        public SubDescuento()
        {
            TipoDescuento = Enum.ExpresionDinero.ExpresionDineroEnum.NotSet;
            ValorDescuento = 0;
        }
    }
}
