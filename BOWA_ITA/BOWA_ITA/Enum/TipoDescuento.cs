using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Enum
{
    public class ExpresionDinero
    {
        public enum ExpresionDineroEnum 
        { 
            /// <summary>
            /// Aún no se ha definido un valor.
            /// </summary>
            [XmlEnum("")]
            NotSet,
            /// <summary>
            /// El valor se expresa como porcentaje
            /// </summary>
            [XmlEnum("%")]
            Porcentaje,
            /// <summary>
            /// El valor se expresa en pesos.
            /// </summary>
            [XmlEnum("$")]
            Pesos
        }
    }
}
