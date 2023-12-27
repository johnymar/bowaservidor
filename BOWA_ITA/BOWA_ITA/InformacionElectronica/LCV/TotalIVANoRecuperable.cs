using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    /// <summary>
    /// Hasta 5 repeticiones.
    /// </summary>
    public class TotalIVANoRecuperable
    {
        /// <summary>
        /// Código de IVA No Recuperable
        /// </summary>
        [XmlElement("CodIVANoRec")]
        public Enum.CodigoIVANoRecuperable .CodigoIVANoRecuperableEnum CodigoIVANoRecuperable { get; set; }
        public bool ShouldSerializeCodigoIVANoRecuperable() { return true; }

        /// <summary>
        /// Número de operaciones con IVA No Recuperable
        /// </summary>
        [XmlElement("TotOpIVANoRec")]
        public int CantidadOperacionesIVANoRecuperable { get; set; }
        public bool ShouldSerializeCantidadOperacionesIVANoRecuperable() { return CantidadOperacionesIVANoRecuperable != 0; }

        /// <summary>
        /// Total de IVA No Recuperable.
        /// </summary>
        [XmlElement("TotMntIVANoRec")]
        public int TotalMontoIVANoRecuperable { get; set; }
        public bool ShouldSerializeTotalMontoIVANoRecuperable() { return true; }
    }
}