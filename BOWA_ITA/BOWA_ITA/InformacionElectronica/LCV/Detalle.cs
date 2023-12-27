using ItaSystem.DTE.Engine.Helpers;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class Detalle
    {
        [XmlElement("Folio")]
        public int Folio { get; set; }
        public bool ShouldSerializeFolio() { return Folio != 0; }

        /// <summary>
        /// Tipo de documento
        /// </summary>
        [XmlElement("TpoDoc")]
        public Enum.TipoDTE.DTEType TipoDocumento { get; set; }
        public bool ShouldSerializeTipoDocumento() { return TipoDocumento != Enum.TipoDTE.DTEType.NotSet; }

        //[XmlIgnore]
        //public Enum.TipoLibro.TipoLibroOrigen TipoLibro { get; set; }
        /// <summary>
        /// Indica si nota de débito o nota de crédito afecta a una factura de compra.
        /// ??
        /// </summary>
        [XmlElement("Emisor")]
        public int Emisor { get; set; }
        public bool ShouldSerializeEmisor() { return Emisor != 0; }

        /// <summary>
        /// Indica si nota de debito o notade credito afecta a una factura de compra
        /// ??
        /// </summary>
        [XmlElement("IndFactCompra")]
        public int IndicadorFacturaCompra { get; set; }
        public bool ShouldSerializeIndicadorFacturaCompra() { return IndicadorFacturaCompra != 0; }

        /// <summary>
        /// Identificador o folio del documento.
        /// </summary>
        [XmlElement("NroDoc")]
        public int NumeroDocumento { get; set; }
        public bool ShouldSerializeNumeroDocumento() { return NumeroDocumento != 0; }

        /// <summary>
        /// Indica que el estado del documento es Anulado.
        /// </summary>
        [XmlElement("Anulado")]
        public Enum.IndicadorAnulado.IndicadorAnuladoEnum IndicadorAnulado { get; set; }
        public bool ShouldSerializeIndicadorAnulado() { return IndicadorAnulado != Enum.IndicadorAnulado.IndicadorAnuladoEnum.NotSet; }

        /// <summary>
        /// Indica si agrega o elimina información
        /// </summary>
        [XmlElement("Operacion")]
        public Enum.OperacionDetalle.OperacionDetalleEnum Operacion { get; set; }
        public bool ShouldSerializeOperacion() { return Operacion != Enum.OperacionDetalle.OperacionDetalleEnum.NotSet; }

        /// <summary>
        /// Tipo de impuesto usado en la operacion (LC).
        /// </summary>
        [XmlElement("TpoImp")]
        public Enum.TipoImpuesto.TipoImpuestoResumido TipoImpuesto { get; set; }
        public bool ShouldSerializeTipoImpuesto() { return TipoImpuesto != Enum.TipoImpuesto.TipoImpuestoResumido.NotSet; }

        /// <summary>
        /// Tasa de impuesto usada en la operación.
        /// 3 enteros, dos decimales.
        /// 1 - 100.
        /// Se debe registrar la tasa del IVA  o del DL 18.211.
        /// </summary>
        [XmlElement("TasaImp")]
        public double TasaImpuestoOperacion { get; set; }
        public bool ShouldSerializeTasaImpuestoOperacion() { return TasaImpuestoOperacion != 0; }

        [XmlIgnore]
        private string _numeroInterno;
        /// <summary>
        /// Número interno.
        /// Referencia del comprobante
        /// </summary>
        [XmlElement("NumInt")]
        public string NumeroInterno { get { return _numeroInterno.Truncate(10); } set { _numeroInterno = value; } }
        public bool ShouldSerializeNumeroInterno() { return !string.IsNullOrEmpty(NumeroInterno); }

        /// <summary>
        /// Indica si corresponde a un servicio (LV).
        /// 
        /// </summary>
        [XmlElement("IndServicio")]
        public Enum.IndicadorServicio.IndicadorServicioDetalleLibroEnum IndicadorServicio { get; set; }
        public bool ShouldSerializeIndicadorServicio() { return IndicadorServicio != Enum.IndicadorServicio.IndicadorServicioDetalleLibroEnum.NotSet; }

        /// <summary>
        /// Indicador de venta sin costo (LV).
        /// </summary>
        [XmlElement("IndSinCosto")]
        public Enum.IndicadorSinCosto.IndicadorSinCostoEnum IndicadorSinCosto { get; set; }
        public bool ShouldSerializeIndicadorSinCosto() { return IndicadorSinCosto != Enum.IndicadorSinCosto.IndicadorSinCostoEnum.NotSet; }

        /// <summary>
        /// Fecha del documento. AAAA-MM-DD
        /// </summary>
        [XmlIgnore]
        public DateTime FechaDocumento { get { return DateTime.Parse(FechaDocumentoString); } set { this.FechaDocumentoString = value.ToString(Config.Resources.DateFormat); } }

        /// <summary>
        /// Fecha del documento. AAAA-MM-DD
        /// Do not set this property, set FechaDocumento instead.
        /// </summary>
        [XmlElement("FchDoc")]
        public string FechaDocumentoString { get; set; }
        public bool ShouldSerializeFechaDocumentoString() { return true; }

        /// <summary>
        /// Codigo de Sucursal entregado por el SII.
        /// </summary>
        [XmlElement("CdgSIISucur")]
        public int CodigoSucursal { get; set; }
        public bool ShouldSerializeCodigoSucursal() { return CodigoSucursal != 0; }

        /// <summary>
        /// Rut del contraparte en la operación comercial
        /// </summary>
        [XmlElement("RUTDoc")]
        public string RutDocumento { get; set; }
        public bool ShouldSerializeRutDocumento() { return !string.IsNullOrEmpty(RutDocumento); }

        [XmlIgnore]
        private string _razonSocial;
        /// <summary>
        /// Razón social de la contraparte del documento.
        /// </summary>
        [XmlElement("RznSoc")]
        public string RazonSocial { get { return _razonSocial.Truncate(50); } set { _razonSocial = value; } }
        public bool ShouldSerializeRazonSocial() { return !string.IsNullOrEmpty(RazonSocial); }

        ///// <summary>
        ///// Para factura de exportación: Corresponde al número o código de identificación del receptro extranjero.
        ///// Se deben incluir guiones y dígitos verificadores.
        ///// ??
        ///// </summary>
        //[XmlElement("NumId")]
        //public string NumeroIdentificadorReceptorExtranjero { get; set; }
        //public bool ShouldSerializeNumeroIdentificadorReceptorExtranjero() { return !string.IsNullOrEmpty(NumeroIdentificadorReceptorExtranjero); }

        /// <summary>
        /// Receptor Extranjero (LV).
        /// </summary>
        public Documento.Extranjero Extranjero { get; set; }
        public bool ShouldSerializeExtranjero() { return Extranjero != null; }

        /// <summary>
        /// Tipo de documento de referencia (LV)
        /// </summary>
        [XmlElement("TpoDocRef")]
        public Enum.TipoDTE.TipoDocumentoLibro TipoDocumentoReferencia { get; set; }
        public bool ShouldSerializeTipoDocumentoReferencia() { return TipoDocumentoReferencia != Enum.TipoDTE.TipoDocumentoLibro.NotSet; }

        /// <summary>
        /// Tipo de Operacion Realizada
        /// </summary>
        [XmlElement("TpoOper")]
        public Enum.TipoTraslado.TipoTrasladoEnum TipoOperacion { get; set; }
        public bool ShouldSerializeTipoOperacion() { return TipoOperacion != Enum.TipoTraslado.TipoTrasladoEnum.NotSet; }


        /// <summary>
        /// Folio del documento de referencia (LV)
        /// </summary>
        [XmlElement("FolioDocRef")]
        public int FolioDocumentoReferencia { get; set; }
        public bool ShouldSerializeFolioDocumentoReferencia() { return FolioDocumentoReferencia != 0; }

        /// <summary>
        /// Monto exento o no gravado del documento.
        /// </summary>
        [XmlElement("MntExe")]
        public int MontoExento { get; set; }
        public bool ShouldSerializeMontoExento() { return MontoExento != 0; }

        /// <summary>
        /// Monto neto del documento
        /// </summary>
        [XmlElement("MntNeto")]
        public int MontoNeto { get; set; }
        public bool ShouldSerializeMontoNeto() { return true; }

        /// <summary>
        /// Monto de IVA del Documento.
        /// </summary>
        [XmlElement("MntIVA")]
        public int MontoIva { get; set; }
        public bool ShouldSerializeMontoIva() { return MontoIva != 0; }

        /// <summary>
        /// Monto Neto Activo Fijo (LC).
        /// </summary>
        [XmlElement("MntActivoFijo")]
        public int MontoNetoActivoFijo { get; set; }
        public bool ShouldSerializeMontoNetoActivoFijo() { return MontoNetoActivoFijo != 0; }

        /// <summary>
        /// IVA de la operación de Activo Fijo.
        /// </summary>
        [XmlElement("MntIVAActivoFijo")]
        public int MontoIVAActivoFijo { get; set; }
        public bool ShouldSerializeMontoIVAActivoFijo() { return MontoIVAActivoFijo != 0; }

        /// <summary>
        /// Totales de IVA No Recuperable (LC).
        /// Hasta 5 repeticiones.
        /// </summary>
        [XmlElement("IVANoRec")]
        public List<TotalIVANoRecuperableDetalle> IVANoRecuperable { get; set; }
        public bool ShouldSerializeIVANoRecuperable() { return IVANoRecuperable != null ? IVANoRecuperable.Count > 0 : false; }

        /// <summary>
        /// IVA de compras destinadas en parte a ventas exentas y afectas (LC).
        /// </summary>
        [XmlElement("IVAUsoComun")]
        public int IVAUsoComun { get; set; }
        public bool ShouldSerializeIVAUsoComun() { return IVAUsoComun != 0; }

        /// <summary>
        /// Sólo nota de crédito fuera de plazo. (LV)
        /// </summary>
        [XmlElement("IVAFueraPlazo")]
        public int IVAFueraPlazo { get; set; }
        public bool ShouldSerializeIVAFueraPlazo() { return IVAFueraPlazo != 0; }

        /// <summary>
        /// IVA propio en operaciones a cuenta de terceros (LV).
        /// </summary>
        [XmlElement("IVAPropio")]
        public int IVAPropio { get; set; }
        public bool ShouldSerializeIVAPropio() { return IVAPropio != 0; }

        /// <summary>
        /// IVA por cuenta de terceros (LV).
        /// </summary>
        [XmlElement("IVATerceros")]
        public int IVATerceros { get; set; }
        public bool ShouldSerializeIVATerceros() { return IVATerceros != 0; }

        /// <summary>
        /// Ley 18.211.
        /// </summary>
        [XmlElement("Ley18211")]
        public int Ley18211 { get; set; }
        public bool ShouldSerializeLey18211() { return Ley18211 != 0; }

        /// <summary>
        /// Otros impuestos o recargos.
        /// Hasta 20 repeticiones.
        /// </summary>
        [XmlElement("OtrosImp")]
        public List<ImpuestosDetalle> Impuestos { get; set; }
        public bool ShouldSerializeImpuestos() { return Impuestos != null ? Impuestos.Count > 0 : false; }

        /// <summary>
        /// Monto del impuesto sin derecho a crédito. (LC)
        /// </summary>
        [XmlElement("MntSinCredito")]
        public int MontoImpuestosSinDerechoACredito { get; set; }
        public bool ShouldSerializeMontoImpuestosSinDerechoACredito() { return MontoImpuestosSinDerechoACredito != 0; }

        /// <summary>
        /// IVA Retenido Total
        /// </summary>
        [XmlElement("IVARetTotal")]
        public int IVARetenidoTotal { get; set; }
        public bool ShouldSerializeIVARetenidoTotal() { return IVARetenidoTotal != 0; }

        /// <summary>
        /// IVA Retenido Parcial
        /// </summary>
        [XmlElement("IVARetParcial")]
        public int IVARetenidoParcial { get; set; }
        public bool ShouldSerializeIVARetenidoParcial() { return IVARetenidoParcial != 0; }

        /// <summary>
        /// Crédito 65% Empresas Constructoras (LV)
        /// </summary>
        [XmlElement("CredEC")]
        public int CreditoEmpresaContructora { get; set; }
        public bool ShouldSerializeCreditoEmpresaContructora() { return CreditoEmpresaContructora != 0; }

        /// <summary>
        /// Depósito por Envase (LV).
        /// </summary>
        [XmlElement("DepEnvase")]
        public int DepositoEnvase { get; set; }
        public bool ShouldSerializeDepositoEnvase() { return DepositoEnvase != 0; }

        /// <summary>
        /// Info. Elect. de Venta (LV).
        /// </summary>
        [XmlElement("Liquidaciones")]
        public List<TotalLiquidacionDetalle> TotalLiquidacionDetalle { get; set; }
        public bool ShouldSerializeTotalLiquidacionDetalle() { return TotalLiquidacionDetalle != null ? TotalLiquidacionDetalle.Count > 0 : false; }

        /// <summary>
        /// Monto Total del documento.
        /// </summary>
        [XmlElement("MntTotal")]
        public int MontoTotal { get; set; }
        public bool ShouldSerializeMontoTotal() { return true; }

        /// <summary>
        /// IVA No Retenido.
        /// </summary>
        [XmlElement("IVANoRetenido")]
        public int MontoIvaNoRetenido { get; set; }
        public bool ShouldSerializeMontoIvaNoRetenido() { return MontoIvaNoRetenido != 0; }

        /// <summary>
        /// Monto No Facturable (LV)
        /// </summary>
        [XmlElement("MntNoFact")]
        public int MontoNoFacturable { get; set; }
        public bool ShouldSerializeMontoNoFacturable() { return MontoNoFacturable != 0; }

        /// <summary>
        /// Total monto periodo (LV).
        /// </summary>
        [XmlElement("MntPeriodo")]
        public int MontoPeriodo { get; set; }
        public bool ShouldSerializeMontoPeriodo() { return MontoPeriodo != 0; }

        /// <summary>
        /// Venta pasaje Nacional (LV).
        /// </summary>
        [XmlElement("PsjNac")]
        public int PasajeNacional { get; set; }
        public bool ShouldSerializePasajeNacional() { return PasajeNacional != 0; }

        /// <summary>
        /// Venta pasaje internacional (LV).
        /// </summary>
        [XmlElement("PsjInt")]
        public int PasajeInternacional { get; set; }
        public bool ShouldSerializePasajeInternacional() { return PasajeInternacional != 0; }

        /// <summary>
        /// Tabacos - Puros (LC).
        /// </summary>
        [XmlElement("TabPuros")]
        public int Tabacos_Puros { get; set; }
        public bool ShouldSerializeTabacos_Puros() { return Tabacos_Puros != 0; }

        /// <summary>
        /// Tabacos - Cigarrillos
        /// </summary>
        [XmlElement("TabCigarrillos")]
        public int Tabacos_Cigarrillos { get; set; }
        public bool ShouldSerializeTabacos_Cigarrillos() { return Tabacos_Cigarrillos != 0; }

        /// <summary>
        /// Tabacos - Elaborados
        /// </summary>
        [XmlElement("TabElaborado")]
        public int Tabacos_Elaborados { get; set; }
        public bool ShouldSerializeTabacos_Elaborados() { return Tabacos_Elaborados != 0; }

        /// <summary>
        /// Impuesto a vehiculo (LC).
        /// </summary>
        [XmlElement("ImpVehiculo")]
        public int ImpuestoVehiculo { get; set; }
        public bool ShouldSerializeImpuestoVehiculo() { return ImpuestoVehiculo != 0; }
    }
}