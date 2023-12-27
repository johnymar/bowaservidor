using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    /// <summary>
    /// Hasta 20 repeticiones.
    /// </summary>
    public class ImpuestosSegmento
    {
        /// <summary>
        /// Codigo del otro impuesto.
        /// </summary>
        [XmlElement("CodImp")]
        public Enum.TipoImpuesto.TipoImpuestoEnum CodigoImpuesto { get; set; }
        public bool ShouldSerializeCodigoImpuesto() { return true; }

        /// <summary>
        /// Monto del otro impuesto
        /// </summary>
        [XmlElement("TotMntImp")]
        public int TotalMontoImpuesto { get; set; }
        public bool ShouldSerializeTotalMontoImpuesto() { return true; }
    }
}