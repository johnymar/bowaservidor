using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Enum
{
    public class TipoServicio
    {
        public enum TipoServicioEnum : int
        {
            /// <summary>
            /// No se ha asignado un valor aún.
            /// </summary>
            [XmlEnum("")]
            NotSet = 0,
            [XmlEnum("1")]
            BoletaServiciosPeriodicos = 1,
            [XmlEnum("2")]
            BoletaServiciosPeriodicosDomiciliarios = 2,
            [XmlEnum("3")]
            BoletaVentasYServicios = 3,
            [XmlEnum("4")]
            BoletaEspectaculosPorTerceros = 4
        }
    }
}
