using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Envio
{
    [XmlRoot("SetDTE")]
    public class SetDTE
    {
        [XmlIgnore]
        public bool serializeDTE { get; set; }

        [XmlIgnore]
        public int DB_Id { get; set; }


        /// <summary>
        /// Id del envío.
        /// </summary>
        [XmlAttribute("ID")]
        public string Id { get; set; }
        //public string Id { get { return "SetDTE"; } set { } }

        /// <summary>
        /// Resumen de información que se envía.
        /// </summary>
        [XmlElement("Caratula")]
        public Caratula Caratula { get; set; }

        [XmlElement("DTE")]
        public List<DTE.Engine.Documento.DTE> DTEs { get; set; }
        public bool ShouldSerializeDTEs() { return serializeDTE; }

        [XmlIgnore]
        public List<string> signedXmls { get; set; }

        [XmlIgnore]
        public List<string> dteXmls { get; set; }

        public SetDTE()
        {
            DTEs = new List<Documento.DTE>();
            signedXmls = new List<string>();
            dteXmls = new List<string>();
            serializeDTE = false;
        }
    }
}
