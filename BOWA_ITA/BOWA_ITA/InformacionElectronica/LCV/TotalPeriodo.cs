using System.Collections.Generic;
using System.Xml.Serialization;

namespace ItaSystem.DTE.Engine.InformacionElectronica.LCV
{
    public class TotalPeriodo
    {
        /// <summary>
        /// Para todos los tipos de documentos que se han informado en el archivo de detalle.
        /// Para todos los tipos de documentos que se han informado en el mes.
        /// Se debe agregar el resumen para los documentos correspondientes a Boletas y Resumen pasajes. 
        /// Códigos(35, 38, 39, 41, 105, 500, 501, 919, 920, 922 y 924)
        /// </summary>
        [XmlElement("TpoDoc")]
        public Enum.TipoDTE.TipoDocumentoLibro TipoDocumento { get; set; }
        public bool ShouldSerializeTipoDocumento() { return true; }

        /// <summary>        
        /// Tipo de impuesto resumido (LC)
        /// Si se omite, se asume 1: IVA.
        /// </summary>
        [XmlElement("TpoImp")]
        public Enum.TipoImpuesto.TipoImpuestoResumido TipoImpuesto { get; set; }
        public bool ShouldSerializeTipoImpuesto() { return TipoImpuesto != Enum.TipoImpuesto.TipoImpuestoResumido.NotSet; }

        /// <summary>
        /// Número de Documentos del Tipo Incluidos en el Libro Electronico.
        /// Cantidad de documentos del tipo especificado en el campo Tipo de Documento. Incluye anulados.    
        /// Cantidad de documentos del tipo especificado en el campo 1. 
        /// Incluye anulados.
        /// Si tipo de documento es 35, 38, 39 y 41 se indica la cantidad de boletas emitidas en el mes.
        /// En el caso del documento 919 y 924 (Resumen Pasajes) se anota la cantidad de pasajes que se han vendido SIN FACTURA EXENTA.
        /// </summary>
        [XmlElement("TotDoc")]
        public int CantidadDocumentos { get; set; }
        public bool ShouldSerializeCantidadDocumentos() { return true; }

        /// <summary>
        /// Número de documentos anulados.
        /// Cantidad de documentos cuyos folios han sido anulados, previo al envío al SII. 
        /// Estos documentos no se totalizan en los campos siguientes.
        /// En el caso de los documentos manuales y de las Boletas Electrónicas , se registran todos los folios anulados.
        /// No se contabiliza aquí si un documento electrónico ha sido anulado, posterior al envío al SII, con Una Nota de Crédito o Debito.
        /// Esos documentos deben ser informados como emitidos, con todos los datos que correspondan.
        /// </summary>
        [XmlElement("TotAnulado")]
        public int CantidadDocumentosAnulados { get; set; }
        public bool ShouldSerializeCantidadDocumentosAnulados() { return CantidadDocumentosAnulados != 0; }

        /// <summary>
        /// Número de operaciones exentas.
        /// CAMPO PRÓXIMO A ELIMINARSE. Se mantiene para contribuyentes que están informándolo.
        /// Cantidad de documentos en que se ha registrado Montos del tipo especificado en el campo 3. 
        /// En el caso de facturas exentas, este monto es igual al del campo 2. 
        /// No se usa si tipo de documento es 35, 38, 39, 41, 105, 500, 501, 919, 920, 922 y 924.
        /// </summary>
        [XmlElement("TotOpExe")]
        public int CantidadOperacionesExentas { get; set; }
        public bool ShouldSerializeCantidadOperacionesExentas() { return CantidadOperacionesExentas != 0; }

        /// <summary>
        /// Total de los montos exentos.
        /// Si no hay montos de este tipo, se debe registrar un 0 
        /// </summary>
        [XmlElement("TotMntExe")]
        public int TotalMontoExento { get; set; }
        public bool ShouldSerializeTotalMontoExento() { return true; }

        /// <summary>
        /// Total de los montos netos.
        /// Si no hay montos de este tipo, se debe registrar un 0 
        /// </summary>
        [XmlElement("TotMntNeto")]
        public int TotalMontoNeto { get; set; }
        public bool ShouldSerializeTotalMontoNeto() { return true; }

        /// <summary>
        /// Total de operaciones con IVA Recuperable (LC). 
        /// ??
        /// </summary>
        [XmlElement("TotOpIVARec")]
        public int CantidadOperacionesIVaRecuperable { get; set; }
        public bool ShouldSerializeCantidadOperacionesIVaRecuperable() { return CantidadOperacionesIVaRecuperable != 0; }

        /// <summary>
        /// Total de los montos de IVA (LV) o IVA Recuperable (LC).
        /// Si no hay montos de este tipo, se debe registrar un 0.
        /// </summary>
        [XmlElement("TotMntIVA")]
        public int TotalMontoIva { get; set; }
        public bool ShouldSerializeTotalMontoIva() { return true; }

        /// <summary>
        /// Cantidad de operaciones de Activo Fijo (LC).
        /// </summary>
        [XmlElement("TotOpActivoFijo")]
        public int CantidadOperacionesActivoFijo { get; set; }
        public bool ShouldSerializeCantidadOperacionesActivoFijo() { return CantidadOperacionesActivoFijo != 0; }

        /// <summary>
        /// Total monto neto de las operaciones de Activo Fijo.
        /// </summary>
        [XmlElement("TotMntActivoFijo")]
        public int TotalMontoNetoOperacionesActivoFijo { get; set; }
        public bool ShouldSerializeTotalMontoNetoOperacionesActivoFijo() { return TotalMontoNetoOperacionesActivoFijo != 0; }

        /// <summary>
        /// Total del IVA de las operaciones de Activo Fijo.
        /// </summary>
        [XmlElement("TotMntIVAActivoFijo")]
        public int TotalMontoIVAOperacionesActivoFijo { get; set; }
        public bool ShouldSerializeTotalMontoIVAOperacionesActivoFijo() { return TotalMontoIVAOperacionesActivoFijo != 0; }

        /// <summary>
        /// Totales de IVA No Recuperable (LC).
        /// Hasta 5 repeticiones.
        /// </summary>
        [XmlElement("TotIVANoRec")]
        public List<TotalIVANoRecuperable> TotalIVANoRecuperable { get; set; }
        public bool ShouldSerializeTotalIVANoRecuperable() { return TotalIVANoRecuperable != null ? TotalIVANoRecuperable.Count > 0 : false; }

        /// <summary>
        /// Número de operaciones conIVA Uso Común (LC)
        /// </summary>
        [XmlElement("TotOpIVAUsoComun")]
        public int CantidadOperacionesConIvaUsoComun { get; set; }
        public bool ShouldSerializeCantidadOperacionesConIvaUsoComun() { return CantidadOperacionesConIvaUsoComun != 0; }

        /// <summary>
        /// Total IVA Uso Común (LC)
        /// </summary>
        [XmlElement("TotIVAUsoComun")]
        public int TotalIVAUsoComun { get; set; }
        public bool ShouldSerializeTotalIVAUsoComun() { return TotalIVAUsoComun != 0; }

        /// <summary>
        /// Factor de proporcionalidad de IVA (LC)
        /// </summary>
        [XmlElement("FctProp")]
        public double FactorProporcionalidadIVA { get; set; }
        public bool ShouldSerializeFactorProporcionalidadIVA() { return FactorProporcionalidadIVA != 0; }

        /// <summary>
        /// Total crédito IVA Uso Común. (LC).
        /// </summary>
        [XmlElement("TotCredIVAUsoComun")]
        public int TotalCreditoIVAUsoComun { get; set; }
        public bool ShouldSerializeTotalCreditoIVAUsoComun() { return TotalCreditoIVAUsoComun != 0; }

        /// <summary>
        /// Total IVa Fuera de Plazo.
        /// Sólo en Notas de Crédito.
        /// </summary>
        [XmlElement("TotIVAFueraPlazo")]
        public int TotalIVAFueraPlazo { get; set; }
        public bool ShouldSerializeTotalIVAFueraPlazo() { return TotalIVAFueraPlazo != 0; }

        /// <summary>
        /// Total IVA Propio en operaciones a cuenta de terceros (LV).
        /// Sólo cuando hay venta o servicio por cuenta de terceros.
        /// </summary>
        [XmlElement("TotIVAPropio")]
        public int TotalIVAPropio { get; set; }
        public bool ShouldSerializeTotalIVAPropio() { return TotalIVAPropio != 0; }

        /// <summary>
        /// Total de IVA a cuenta de terceros. (LV).
        ///  Sólo cuando hay venta o servicio por cuenta de terceros.
        /// </summary>
        [XmlElement("TotIVATerceros")]
        public int TotalIVATerceros { get; set; }
        public bool ShouldSerializeTotalIVATerceros() { return TotalIVATerceros != 0; }

        /// <summary>
        /// Total Ley 18.211 (LV).
        /// Impuesto Zona Franca.
        /// </summary>
        [XmlElement("TotLey18211")]
        public int TotalLey18211 { get; set; }
        public bool ShouldSerializeTotalLey18211() { return TotalLey18211 != 0; }

        /// <summary>
        /// Totales otros impuestos.
        /// </summary>
        [XmlElement("TotOtrosImp")]
        public List<ImpuestosPeriodo> Impuestos { get; set; }
        public bool ShouldSerializeImpuestos() { return Impuestos != null ? Impuestos.Count > 0 : false; }

        /// <summary>
        /// Total de impuestos sin derecho a crédito (LC).
        /// </summary>
        [XmlElement("TotImpSinCredito")]
        public int TotalImpuestosSinDerechoACredito { get; set; }
        public bool ShouldSerializeTotalImpuestosSinDerechoACredito() { return TotalImpuestosSinDerechoACredito != 0; }

        /// <summary>
        /// Número de operaciones con IVA Retenido Total (LV).
        /// CAMPO PRÓXIMO A ELIMINARSE. Se mantiene para contribuyentes que están informándolo.
        /// Cantidad de documentos en que se ha registrado Montos del tipo especificado en el campo siguiente.
        /// Sólo aplica para Factura de Compra recibidas
        /// </summary>
        [XmlElement("TotOpIVARetTotal")]
        public int CantidadOperacionesConIVARetenidoTotal { get; set; }
        public bool ShouldSerializeCantidadOperacionesConIVARetenidoTotal() { return CantidadOperacionesConIVARetenidoTotal != 0; }

        /// <summary>
        /// Total de IVA Retenido Total (LV).
        /// </summary>
        [XmlElement("TotIVARetTotal")]
        public int TotalIVARetenidoTotal { get; set; }
        public bool ShouldSerializeTotalIVARetenidoTotal() { return TotalIVARetenidoTotal != 0; }

        /// <summary>
        /// Número de operaciones con IVA Retenido Parcial (LV).
        /// CAMPO PRÓXIMO A ELIMINARSE. Se mantiene para contribuyentes que están informándolo.
        /// Cantidad de documentos en que se ha registrado Montos del tipo especificado en el campo siguiente.
        /// Sólo aplica para Factura de Compra recibidas
        /// </summary>
        [XmlElement("TotOpIVARetParcial")]
        public int CantidadOperacionesConIVARetenidoParcial { get; set; }
        public bool ShouldSerializeCantidadOperacionesConIVARetenidoParcial() { return CantidadOperacionesConIVARetenidoParcial != 0; }

        /// <summary>
        /// Total de IVA Retenido Parcial (LV).
        /// </summary>
        [XmlElement("TotIVARetParcial")]
        public int TotalIVARetenidoParcial { get; set; }
        public bool ShouldSerializeTotalIVARetenidoParcial() { return TotalIVARetenidoParcial != 0; }

        /// <summary>
        /// Total crédito empresa constructora.
        /// </summary>
        [XmlElement("TotCredEC")]
        public int TotalCreditoEmpresaContructora { get; set; }
        public bool ShouldSerializeTotalCreditoEmpresaContructora() { return TotalCreditoEmpresaContructora != 0; }

        /// <summary>
        /// Total de los depósitos por Envase (LV).
        /// </summary>
        [XmlElement("TotDepEnvase")]
        public int TotalDepositoEnvase { get; set; }
        public bool ShouldSerializeTotalDepositoEnvase() { return TotalDepositoEnvase != 0; }

        /// <summary>
        /// Info. Elect. de Venta (LV).
        /// Totaliza por código para cada tipo de documento
        /// </summary>
        [XmlElement("TotLiquidaciones")]
        public List<TotalLiquidacion> TotalLiquidacion { get; set; }
        public bool ShouldSerializeTotalLiquidacion() { return TotalLiquidacion != null ? TotalLiquidacion.Count > 0 : false; }

        /// <summary>
        /// Total de los montos totales.
        /// Totales de los documentos.
        /// </summary>
        [XmlElement("TotMntTotal")]
        public int TotalMonto { get; set; }
        public bool ShouldSerializeTotalMonto() { return true; }

        /// <summary>
        /// Número de operaciones con IVA No Retenido (LV).
        /// CAMPO PRÓXIMO A ELIMINARSE. Se mantiene para contribuyentes que están informándolo.
        /// Cantidad de documentos en que se ha registrado Montos del tipo especificado en el campo siguiente.
        /// Sólo aplica para Factura de Compra recibidas        
        /// </summary>
        [XmlElement("TotOpIVANoRetenido")]
        public int CantidadOperacionesConIvaNoRetenido { get; set; }
        public bool ShouldSerializeCantidadOperacionesConIvaNoRetenido() { return CantidadOperacionesConIvaNoRetenido != 0; }

        /// <summary>
        /// Total IVA No Retenido
        /// </summary>
        [XmlElement("TotIVANoRetenido")]
        public int TotalMontoIvaNoRetenido { get; set; }
        public bool ShouldSerializeTotalMontoIvaNoRetenido() { return TotalMontoIvaNoRetenido != 0; }

        /// <summary>
        /// Total monto No Facturable (LV).
        /// Valor puede ser negativo.
        /// </summary>
        [XmlElement("TotMntNoFact")]
        public int TotalMontoNoFacturable { get; set; }
        public bool ShouldSerializeTotalMontoNoFacturable() { return TotalMontoNoFacturable != 0; }

        /// <summary>
        /// Total monto periodo (LV).
        /// Valor puede ser negativo
        /// </summary>
        [XmlElement("TotMntPeriodo")]
        public int TotalMontoPeriodo { get; set; }
        public bool ShouldSerializeTotalMontoPeriodo() { return TotalMontoPeriodo != 0; }

        /// <summary>
        /// Total venta pasaje Nacional (LV).
        /// Sólo ventas con Factura Exenta.
        /// </summary>
        [XmlElement("TotPsjNac")]
        public int TotalPasajeNacional { get; set; }
        public bool ShouldSerializeTotalPasajeNacional() { return TotalPasajeNacional != 0; }

        /// <summary>
        /// Total venta pasaje internacional (LV).
        /// Sólo ventas con Factura Exenta.
        /// </summary>
        [XmlElement("TotPsjInt")]
        public int TotalPasajeInternacional { get; set; }
        public bool ShouldSerializeTotalPasajeInternacional() { return TotalPasajeInternacional != 0; }

        /// <summary>
        /// Total Tabacos - Puros (LC).
        /// </summary>
        [XmlElement("TotTabPuros")]
        public int TotalTabacos_Puros { get; set; }
        public bool ShouldSerializeTotalTabacos_Puros() { return TotalTabacos_Puros != 0; }

        /// <summary>
        /// Total Tabacos - Cigarrillos
        /// </summary>
        [XmlElement("TotTabCigarrillos")]
        public int TotalTabacos_Cigarrillos { get; set; }
        public bool ShouldSerializeTotalTabacos_Cigarrillos() { return TotalTabacos_Cigarrillos != 0; }

        /// <summary>
        /// Total Tabacos - Elaborados
        /// </summary>
        [XmlElement("TotTabElaborado")]
        public int TotalTabacos_Elaborados { get; set; }
        public bool ShouldSerializeTotalTabacos_Elaborados() { return TotalTabacos_Elaborados != 0; }

        /// <summary>
        /// Total impuesto vehiculos (LC).
        /// </summary>
        [XmlElement("TotImpVehiculo")]
        public int TotalImpuestoVehiculo { get; set; }
        public bool ShouldSerializeTotalImpuestoVehiculo() { return TotalImpuestoVehiculo != 0; }
    }
}