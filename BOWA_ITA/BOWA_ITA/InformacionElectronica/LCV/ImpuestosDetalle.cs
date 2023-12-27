using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class ImpuestosDetalle
    {
        /// <summary>
        /// Codigo del otro impuesto.
        /// </summary>
        [XmlElement("CodImp")]
        public Enum.TipoImpuesto.TipoImpuestoEnum CodigoImpuesto { get; set; }
        public bool ShouldSerializeCodigoImpuesto() { return true; }

        /// <summary>
        /// Tasa del impuesto o recargo.
        /// </summary>
        [XmlElement("TasaImp")]
        public double TasaImpuesto { get; set; }
        public bool ShouldSerializeTasaImpuesto() { return TasaImpuesto != 0; }

        /// <summary>
        /// Monto del otro impuesto
        /// </summary>
        [XmlElement("MntImp")]
        public int TotalMontoImpuesto { get; set; }
        public bool ShouldSerializeTotalMontoImpuesto() { return true; }
    }
}