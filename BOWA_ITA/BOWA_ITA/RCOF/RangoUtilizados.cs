using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.RCOF
{
    public class RangoUtilizados
    {
        [XmlElement("Inicial")]
        public int Inicial { get; set; }
        public bool ShouldSerializeInicial() { return Inicial != 0; }

        [XmlElement("Final")]
        public int Final { get; set; }
        public bool ShouldSerializeFinal() { return Final != 0; }
    }
}
