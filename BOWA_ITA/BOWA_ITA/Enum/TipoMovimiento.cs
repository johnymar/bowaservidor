using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Enum
{
    public class TipoMovimiento
    {
        public enum TipoMovimientoEnum
        {
            /// <summary>
            /// Aún no se ha definido un valor.
            /// </summary>
            [XmlEnum("")]
            NotSet,
            [XmlEnum("D")]
            Descuento,
            [XmlEnum("R")]
            Recargo
        }
    }
}
