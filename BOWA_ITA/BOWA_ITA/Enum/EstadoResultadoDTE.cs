﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.Enum
{
    public static class EstadoResultadoDTE
    {
        public enum EstadoResultadoDTEEnum : int
        {
            /// <summary>
            /// 0: DTE Aceptado OK.
            /// </summary>
            [XmlEnum("0")]
            Ok = 0,
            /// <summary>
            /// 1: DTE Aceptado con Discrepancia.
            /// </summary>
            [XmlEnum("1")]
            AceptadoReparos = 1,
            /// <summary>
            /// 2: DTE Rechazado.
            /// </summary>
            [XmlEnum("2")]
            Rechazo = 2
        }

        public static string Motivo { get; set; }

        public static string Glosa(EstadoResultadoDTEEnum state)
        {
            switch(state)
            {
                case EstadoResultadoDTEEnum.Ok:
                    return "DTE Aceptado Ok. " + Motivo;
                case EstadoResultadoDTEEnum.AceptadoReparos:
                    return "DTE Aceptado con discrepancias - " + Motivo;
                case EstadoResultadoDTEEnum.Rechazo:
                    return "DTE Rechazado - " + Motivo;
                default:
                    throw new Exception("Invalid state");
            }
        }
    }
}
