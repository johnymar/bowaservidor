using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Enum
{
    public class TipoRecargoComision
    {
        public enum TipoRecargoComisionEnum
        {
            /// <summary>
            /// No se ha definido un valor aún.
            /// </summary>
            [XmlEnum("")]
            NotSet,            
            [XmlEnum("C")]
            Comision,
            [XmlEnum("O")]
            OtrosCargos
        }
    }
}
