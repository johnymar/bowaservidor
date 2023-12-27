using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    /// <summary>
    /// Hasta 5 repeticiones.
    /// </summary>
    public class TotalIVANoRecuperableDetalle
    {
        /// <summary>
        /// Código de IVA No Recuperable
        /// </summary>
        [XmlElement("CodIVANoRec")]
        public Enum.CodigoIVANoRecuperable.CodigoIVANoRecuperableEnum CodigoIVANoRecuperable { get; set; }
        public bool ShouldSerializeCodigoIVANoRecuperable() { return true; }

        /// <summary>
        /// Total de IVA No Recuperable.
        /// </summary>
        [XmlElement("MntIVANoRec")]
        public int TotalMontoIVANoRecuperable { get; set; }
        public bool ShouldSerializeTotalMontoIVANoRecuperable() { return true; }
    }
}