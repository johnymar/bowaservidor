using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ItaSystem.DTE.Engine.PDF417
{
    public static class PDF417
    {
        public static System.Drawing.Bitmap GetTimbreImage(string text, out string message)
        {
            try
            {
                if (String.IsNullOrEmpty(text))
                {
                    message = "Contenido vacío o nulo.";
                    return null;
                }
                BarcodePDF417 pdf417 = new BarcodePDF417();
                pdf417.Options = BarcodePDF417.PDF417_FORCE_BINARY;
                pdf417.CodeRows = 5;
                pdf417.CodeColumns = 18;
                pdf417.LenCodewords = 999;   
                pdf417.ErrorLevel = 5;
                pdf417.Text = Encoding.GetEncoding("ISO-8859-1", new EncoderReplacementFallback("#"), new DecoderReplacementFallback("##")).GetBytes(text);
                System.Drawing.Bitmap timbre = new System.Drawing.Bitmap(pdf417.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                message = "";
                return timbre;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return null;
            }
        }
    }
}
