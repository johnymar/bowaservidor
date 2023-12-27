using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Envio
{
    public class SubTotalesDTE
    {
        /// <summary>
        /// Tipo de DTE enviado.
        /// </summary>
        [XmlElement("TpoDTE")]
        public Enum.TipoDTE.DTEType TipoDTE { get; set; }

        /// <summary>
        /// Numero de DTEs enviados.
        /// </summary>
        [XmlElement("NroDTE")]
        public int Cantidad { get; set; }
    }
}
