using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class TotalTraslado
    {

        /// <summary>
        /// Indicador de Tipo de Traslado. 
        /// 1. Operación constituye venta
        /// 2. Venta por efectuar
        /// 3. Consignaciones
        /// 4. Productos en Demostración
        /// 5. Traslados Internos
        /// 6. Otros Traslados No Venta
        /// </summary>
        [XmlElement("TpoTraslado")]
        public Enum.TipoTraslado.TipoTrasladoEnum TipoTraslado { get; set; }
        public bool ShouldSerializeTipoTraslado() { return TipoTraslado != Enum.TipoTraslado.TipoTrasladoEnum.NotSet; }

        /// <summary>
        /// Cantidad de Guias del Período
        /// </summary>
        [XmlElement("CantGuia")]
        public int CantidadGuia { get; set; }
        public bool ShouldSerializeCantidadGuias() { return true; }

        /// <summary>
        /// Monto de Guias del Período. Indicar Monto, en caso que se hayan valorizado 
        /// </summary>

        [XmlElement("MntGuia")]
        public int MontoGuia { get; set; }
        public bool ShouldSerializeMontoGuias() { return MontoGuia != 0; }
    }
}