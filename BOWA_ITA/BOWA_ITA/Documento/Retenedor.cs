using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ItaSystem.DTE.Engine.Helpers;

namespace ItaSystem.DTE.Engine.Documento
{
    public class Retenedor
    {
        [XmlIgnore]
        private string _indAgente;
        /// <summary>
        /// Indicador agente retenedor.
        /// Obligatorio para agentes retenedores, indica para cada transacción si es agente retenedor del producto que está vendiendo. 
        /// </summary>
        [XmlElement("IndAgente")]
        public string IndicadorAgente { get { return _indAgente.Truncate(1); } set { _indAgente = value; } }

        /// <summary>
        /// Monto base faenamiento.
        /// Sólo para transacciones realizadas por Agentes Retenedores, según códigos de retención 17.
        /// </summary>
        [XmlElement("MntBaseFaena")]
        public int MontoBaseFaenamiento { get; set; }
        public bool ShouldSerializeMontoBaseFaenamiento() { return MontoBaseFaenamiento != 0; }

        /// <summary>
        /// Márgenes de comercialización.
        /// Sólo para transacciones realizadas por Agentes Retenedores, según códigos de retención 14 y 50.
        /// </summary>
        [XmlElement("MntMargComer")]
        public int MontoMargenComercializacion { get; set; }
        public bool ShouldSerializeMontoMargenComercializacion() { return MontoMargenComercializacion != 0; }

        /// <summary>
        /// Precio unitario neto consumidor final.
        /// Sólo para transacciones realizadas por Agentes Retenedores, según códigos de retención 14, 17 y 50.
        /// </summary>
        [XmlElement("PrcConsFinal")]
        public int PrecioConsumidorFinal { get; set; }
        public bool ShouldSerialize() { return PrecioConsumidorFinal != 0; }

        public Retenedor()
        {
            IndicadorAgente = string.Empty;
            MontoBaseFaenamiento = 0;
            MontoMargenComercializacion = 0;
            PrecioConsumidorFinal = 0;
        }
    }
}
