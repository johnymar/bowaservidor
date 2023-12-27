using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Enum
{
    public class IndicadorExento
    {
        public enum IndicadorExentoEnum : int
        {
            /// <summary>
            /// Aún no se ha definido un valor.
            /// </summary>
            [XmlEnum("")]
            NotSet = 0,
            /// <summary>
            /// No afecto o exento de IVA.
            /// </summary>
            [XmlEnum("1")]
            Exento = 1,
            /// <summary>
            /// No facturable.
            /// </summary>
            [XmlEnum("2")]
            NoFacturable = 2
        }
    }
}
