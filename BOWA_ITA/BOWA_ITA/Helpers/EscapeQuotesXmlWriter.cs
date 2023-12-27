using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ITA_CHILE.Helpers
{
    //public class EscapeQuotesXmlWriter : XmlWrappingWriter
    //{
    //    public EscapeQuotesXmlWriter(XmlWriter baseWriter) : base(baseWriter)
    //    {
    //    }

    //    public override void WriteString(string text)
    //    {
    //        foreach (char ch in text)
    //        {
    //            if (ch == '"')
    //            {
    //                WriteEntityRef("quot");
    //            }
    //            else if (ch == '\'')
    //            {
    //                WriteEntityRef("apos");
    //            }
    //            else
    //            {
    //                base.WriteString(ch.ToString());
    //            }
    //        }
    //    }
    //    public override void WriteChars(char[] buffer, int index, int count)
    //    {
    //        WriteString(new String(buffer.Where((ch, pos) => pos >= index && pos < index + count).ToArray()));
    //    }

    //}
}
