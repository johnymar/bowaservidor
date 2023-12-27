using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LBoletas
{
    public class Detalle
    {
        [XmlElement("TpoDoc")]
        public Enum.TipoDTE.TipoDocumentoLibro TipoDocumento { get; set; }
        public bool ShouldSerializeTipoDocumento() { return true; }

        [XmlElement("FolioDoc")]
        public int FolioDocumento { get; set; }
        public bool ShouldSerializeFolioDocumento() { return true; }

        [XmlElement("Anulado")]
        public Enum.IndicadorAnulado.IndicadorAnuladoEnum IndicadorAnulado { get; set; }
        public bool ShouldSerializeIndicadorAnulado() { return IndicadorAnulado != Enum.IndicadorAnulado.IndicadorAnuladoEnum.NotSet; }

        [XmlElement("TpoServ")]
        public int TipoServicio { get; set; }
        public bool ShouldSerializeTipoServicio() { return TipoServicio != 0; }        

        [XmlIgnore]
        public DateTime FechaEmision { get { return DateTime.Parse(FechaEmisionString); } set { this.FechaEmisionString = value.ToString(Config.Resources.DateFormat); } }
        [XmlElement("FchEmiDoc")]
        public string FechaEmisionString { get; set; }
        public bool ShouldSerializeFechaDocumentoString() { return true; }

        [XmlIgnore]
        public DateTime FechaVencimiento { get { return DateTime.Parse(FechaVencimientoString); } set { this.FechaVencimientoString = value.ToString(Config.Resources.DateFormat); } }
        [XmlElement("FchVencDoc")]
        public string FechaVencimientoString { get; set; }
        public bool ShouldSerializeFechaVencimientoString() { return true; }

        [XmlIgnore]
        public DateTime PeriodoDesde { get { return DateTime.Parse(PeriodoDesdeString); } set { this.PeriodoDesdeString = value.ToString(Config.Resources.DateFormat); } }
        [XmlElement("PeriodoDesde")]
        public string PeriodoDesdeString { get; set; }
        public bool ShouldSerializePeriodoDesdeString() { return true; }

        [XmlIgnore]
        public DateTime PeriodoHasta { get { return DateTime.Parse(PeriodoHastaString); } set { this.PeriodoHastaString = value.ToString(Config.Resources.DateFormat); } }
        [XmlElement("PeriodoHasta")]
        public string PeriodoHastaString { get; set; }
        public bool ShouldSerializePeriodoHastaString() { return true; }

        [XmlElement("CdgSIISucur")]
        public int CodigoSucursal { get; set; }
        public bool ShouldSerializeCodigoSucursal() { return CodigoSucursal != 0; }

        [XmlElement("RUTCliente")]
        public string RutCliente { get; set; }
        public bool ShouldSerializeRutCliente() { return true; }

        [XmlElement("MntExe")]
        public int MontoExento { get; set; }
        public bool ShouldSerializeMontoExento() { return MontoExento != 0; }

        [XmlElement("MntTotal")]
        public int MontoTotal { get; set; }
        public bool ShouldSerializeMontoTotal() { return true; }

        [XmlElement("MntNoFact")]
        public int MontoNoFacturable { get; set; }
        public bool ShouldSerializeMontoNoFacturable() { return MontoNoFacturable != 0; }

        [XmlElement("MntPeriodo")]
        public int MontoPeriodo { get; set; }
        public bool ShouldSerializeMontoPeriodo() { return MontoPeriodo != 0; }

        [XmlElement("SaldoAnt")]
        public int SaldoAnterior { get; set; }
        public bool ShouldSerializeSaldoAnterior() { return SaldoAnterior != 0; }

        [XmlElement("VlrPagar")]
        public int ValorPagar { get; set; }
        public bool ShouldSerializeValorPagar() { return ValorPagar != 0; }

        [XmlElement("TotTicketBoleta")]
        public int TotalTicket { get; set; }
        public bool ShouldSerializeTotalTicket() { return TotalTicket != 0; }

        public Detalle()
        {
            TipoServicio = 3;
        }
    }
}
