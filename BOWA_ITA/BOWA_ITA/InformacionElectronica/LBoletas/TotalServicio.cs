using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LBoletas
{
    public class TotalServicio
    {
        [XmlElement("TpoServ")]
        public int TipoServicio { get; set; }
        public bool ShouldSerializeTipoServicio() { return TipoServicio != 0; }

        [XmlElement("PeriodoDevengado")]
        public string PeriodoDevengado { get; set; }
        public bool ShouldSerializePeriodoDevengado() { return !string.IsNullOrEmpty(PeriodoDevengado); }

        [XmlElement("TotDoc")]
        public int CantidadDocumentos { get; set; }
        public bool ShouldSerializeCantidadDocumentos() { return CantidadDocumentos != 0; }

        [XmlElement("TotMntExe")]
        public int TotalExento { get; set; }
        public bool ShouldSerializeTotalExento() { return TotalExento != 0; }

        [XmlElement("TotMntNeto")]
        public int TotalNeto { get; set; }
        public bool ShouldSerializeTotalNeto() { return TotalNeto != 0; }

        [XmlElement("TasaIVA")]
        public double TasaIVA { get; set; }
        public bool ShouldSerializeTasaIVA() { return TasaIVA != 0; }

        [XmlElement("TotMntIVA")]
        public int TotalIVA { get; set; }
        public bool ShouldSerializeTotalIVA() { return TotalIVA != 0; }

        [XmlElement("TotMntTotal")]
        public int TotalTotal { get; set; }
        public bool ShouldSerializeTotalTotal() { return TotalTotal != 0; }

        [XmlElement("TotMntNoFact")]
        public int TotalNoFacturable { get; set; }
        public bool ShouldSerializeTotalNoFacturable() { return TotalNoFacturable != 0; }

        [XmlElement("TotMntPeriodo")]
        public int TotalPeriodo { get; set; }
        public bool ShouldSerializeTotalPeriodo() { return TotalPeriodo != 0; }

        [XmlElement("TotSaldoAnt")]
        public int TotalSaldoAnterior { get; set; }
        public bool ShouldSerializeTotalSaldoAnterior() { return TotalSaldoAnterior != 0; }

        [XmlElement("TotVlrPagar")]
        public int TotalValorPagar { get; set; }
        public bool ShouldSerializeTotalValorPagar() { return TotalValorPagar != 0; }

        [XmlElement("TotTicket")]
        public int TotalTicket { get; set; }
        public bool ShouldSerializeTotalTicket() { return TotalTicket != 0; }
    }
}
