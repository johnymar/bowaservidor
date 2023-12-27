using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class TotalLiquidacion
    {
        /// <summary>
        /// Valor neto comisiones y otros cargos (LV).
        /// Puede ser negativo.
        /// </summary>
        [XmlElement("TotValComNeto")]
        public int TotalNetoComisionesYOtrosCargos { get; set; }
        public bool ShouldSerializeTotalNetoComisionesYOtrosCargos() { return TotalNetoComisionesYOtrosCargos != 0; }

        /// <summary>
        /// Valor comisiones y otros cargos no afectos o exentos (LV).
        /// Puede ser negativo.
        /// </summary>
        [XmlElement("TotValComExe")]
        public int TotalExentoComisionesYOtrosCargos { get; set; }
        public bool ShouldSerializeTotalExentoComisionesYOtrosCargos() { return TotalExentoComisionesYOtrosCargos != 0; }

        /// <summary>
        /// Valor IVA comisiones y otros cargos (LV).
        /// Puede ser negativo.
        /// </summary>
        [XmlElement("TotValComIVA")]
        public int TotalIVAComisionesYOtrosCargos { get; set; }
        public bool ShouldSerializeTotalIVAComisionesYOtrosCargos() { return TotalIVAComisionesYOtrosCargos != 0; }
    }
}