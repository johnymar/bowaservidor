using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    /// <summary>
    /// Hasta 20 repeticiones.
    /// </summary>
    public class ImpuestosPeriodo
    {
        /// <summary>
        /// Codigo del otro impuesto.
        /// Según lo informado en detalle.
        /// </summary>
        [XmlElement("CodImp")]
        public Enum.TipoImpuesto.TipoImpuestoEnum CodigoImpuesto { get; set; }
        public bool ShouldSerializeCodigoImpuesto() { return true; }

        /// <summary>
        /// Monto total del otro impuesto.
        /// Totaliza los valores correspondiente al código por tipo de documento del detalle.
        /// </summary>
        [XmlElement("TotMntImp")]
        public int TotalMontoImpuesto { get; set; }
        public bool ShouldSerializeTotalMontoImpuesto() { return true; }

        /// <summary>
        /// Factor impuesto adicional (LC)
        /// </summary>
        [XmlElement("FctImpAdic")]
        public double FactorImpuestoAdicional { get; set; }
        public bool ShouldSerializeFactorImpuestoAdicional() { return FactorImpuestoAdicional != 0; }

        /// <summary>
        /// Total crédito impuesto (LC)
        /// </summary>
        [XmlElement("TotCredImp")]
        public int TotalCreditoImpuesto { get; set; }
        public bool ShouldSerializeTotalCreditoImpuesto() { return TotalCreditoImpuesto != 0; }
    }
}