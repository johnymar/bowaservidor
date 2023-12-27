using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaSystem.DTE.Engine.XML
{
    public static class SerializationType
    {
        public enum SerializationTypes : int
        {
            Inline = 0,
            LineBreakNoIndent = 1,
            PrettyPrint = 2
        }
    }
}
