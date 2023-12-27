using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class TotalLiquidacionDetalle
    {
        /// <summary>
        /// Rut Emisor (LV).
        /// </summary>
        [XmlElement("RutEmisor")]
        public string RutEmisor { get; set; }
        public bool ShouldSerializeRutEmisor() { return true; }

        /// <summary>
        /// Valor neto comisiones y otros cargos (LV).
        /// </summary>
        [XmlElement("ValComNeto")]
        public int NetoComisionesYOtrosCargos { get; set; }
        public bool ShouldSerializeNetoComisionesYOtrosCargos() { return NetoComisionesYOtrosCargos != 0; }

        /// <summary>
        /// Valor comisiones y otros cargos no afectos o exentos (LV).
        /// </summary>
        [XmlElement("ValComExe")]
        public int ExentoComisionesYOtrosCargos { get; set; }
        public bool ShouldSerializeExentoComisionesYOtrosCargos() { return ExentoComisionesYOtrosCargos != 0; }

        /// <summary>
        /// Valor IVA comisiones y otros cargos (LV).
        /// </summary>
        [XmlElement("ValComIVA")]
        public int IVAComisionesYOtrosCargos { get; set; }
        public bool ShouldSerializeIVAComisionesYOtrosCargos() { return IVAComisionesYOtrosCargos != 0; }
    }
}