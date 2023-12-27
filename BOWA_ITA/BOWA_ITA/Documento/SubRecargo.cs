using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Documento
{
    public class SubRecargo
    {
        /// <summary>
        /// Tipo de subdescuento.
        /// </summary>
        [XmlElement("TipoRecargo")]
        public Enum.ExpresionDinero.ExpresionDineroEnum TipoRecargo { get; set; }

        [XmlIgnore]
        private double _valorRecargo;
        /// <summary>
        /// Valor de subdescuento.
        /// </summary>
        [XmlElement("ValorRecargo")]
        public double ValorRecargo { get { return Math.Round(_valorRecargo, 2); } set { _valorRecargo = value; } }

        public SubRecargo()
        {
            TipoRecargo = Enum.ExpresionDinero.ExpresionDineroEnum.NotSet;
            ValorRecargo = 0;
        }
    }
}
