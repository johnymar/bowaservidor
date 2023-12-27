using System.Collections.Generic;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class ResumenSegmento
    {
        /// <summary>
        /// En este resumen se deben entregar totalizados los montos por campo de cada tipo de documento, para los documentos incluidos en el detalle.
        /// Todos los montos pueden ser negativos así como también el campo “cantidad de documentos” que puede ser negativo en un envío de ajuste.
        /// 1 - 40 repeticiones.
        /// </summary>
        [XmlElement("TotalesSegmento")]
        public List<TotalSegmento> TotalesSegmento { get; set; }
        public bool ShouldSerializeTotalesSegmento() { return TotalesSegmento.Count != 0; }

        /// <summary>
        /// Total Guias Anuladas (Anulado = 2). Dato opcional.
        /// </summary>
        [XmlElement("TotGuiaAnulada")]
        public int TotalesGuiasAnuladas { get; set; }
        public bool ShouldSerializeTotalesGuiasAnuladas() { return TotalesGuiasAnuladas != 0; }

        /// <summary>
        /// Total Folios Anulados (Anulado = 1). Dato opcional.
        /// </summary>
        [XmlElement("TotFolAnulado")]
        public int TotalesFoliosAnulados { get; set; }
        public bool ShouldSerializeTotalesFoliosAnulados() { return TotalesFoliosAnulados != 0; }

        /// <summary>
        /// Total de Guias Venta. Dato obligatorio. Cantidad de Guías en que el campo ANULADO/MODIFICADO
        /// distinto de 1 y 2 y el campo TIPO DE OPERACIÓN = 1 (que constituye venta)
        /// </summary>
        [XmlElement("TotGuiaVenta")]
        public int TotalesGuiasDeVentas { get; set; }
        public bool ShouldSerializeTotalesGuiasDeVentas() { return TotalesGuiasDeVentas != 0; }

        /// <summary>
        /// Monto Total de Guias de Venta. Dato obligatorio. Suma del Monto Total de Guías Venta.
        /// </summary>
        [XmlElement("TotMntGuiaVta")]
        public int MontoTotalVentasGuia { get; set; }
        public bool ShouldSerializeMontoTotalVentasGuia() { return MontoTotalVentasGuia != 0; }

        /// <summary>
        /// Indica la cantidad y el Monto Total de los distintos tipos de Guías no Venta.
        /// Se repite hasta 6 veces, según los distintos códigos no  venta.
        /// </summary>
        [XmlElement("TotTraslado")]
        public List<TotalTraslado> Traslados { get; set; }
        public bool ShouldSerializeTraslados() { return Traslados.Count != 0; }


        public ResumenSegmento()
        {
            TotalesSegmento = new List<TotalSegmento>();
            Traslados = new List<TotalTraslado>();
        }
    }
}