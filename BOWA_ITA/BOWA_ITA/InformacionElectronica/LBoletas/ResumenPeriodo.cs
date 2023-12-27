using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LBoletas
{
    public class ResumenPeriodo
    {
        /// <summary>
        /// En este resumen se deben entregar totalizados los montos por campo de cada tipo de documento de todo el período. 
        /// También es obligatorio en el caso de efectuar ajustes posteriores al cierre del período.        
        /// 1 a 40 repeticiones.
        /// </summary>
        [XmlElement("TotalesPeriodo")]
        public List<TotalPeriodo> TotalesPeriodo { get; set; }
        public bool ShouldSerializeTotalesPeriodo() { return TotalesPeriodo.Count != 0; }

        public ResumenPeriodo()
        {
            TotalesPeriodo = new List<TotalPeriodo>();
        }
    }
}
