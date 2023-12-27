using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaSystem.DTE.Engine.XML
{
    public static class Schemas
    {
        private const string RootFolder = @"XML\Schemas\";
        public const string EnvioDTE = RootFolder + "EnvioDTE_v10.xsd";
        public const string EnvioBoleta = RootFolder + "EnvioBOLETA_v11.xsd";
        public const string DTE = RootFolder + "DTE_v10.xsd";
        public const string TiposSII = RootFolder + "SiiTypes_v10.xsd";
        public const string Firma = RootFolder + "xmldsignature_v10.xsd";
        public const string ConsumoFolios = RootFolder + "ConsumoFolio_v10.xsd";

        public const string LCV_CAL = RootFolder + "IECV\\LceCal_v10.xsd";
        public const string LCV_COCERT = RootFolder + "IECV\\LceCoCertif_v10.xsd";
        public const string LCV_TIPOS = RootFolder + "IECV\\LceSiiTypes_v10.xsd";
        public const string LCV_LIBRO = RootFolder + "IECV\\LibroCV_v10.xsd";
        public const string LCV_FIRMA = RootFolder + "IECV\\xmldsignature_v10.xsd";

        public const string LibroBoletas = RootFolder + "IECV\\LibroBOLETA_v10.xsd";
        public const string LibroGuias = RootFolder + "IECV\\LibroGuia_v10.xsd";

        public const string RESULT = RootFolder + "RESULTADO\\RespuestaEnvioDTE_v10.xsd";
    }
}
