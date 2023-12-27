using bowuaSystem.DTE.Engine.Documento;
using bowuaSystem.DTE.Engine.Enum;
using bowuaSystem.DTE.Engine.Envio;
using bowuaSystem.DTE.Engine.RespuestaEnvio;
using bowuaSystem.DTE.WS.EstadoDTE;
using BOWUA_CHILE.Security.Firma;
using dte1.clases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using bowuaSystem.DTE.WS.EstadoEnvio;
using static BOWUA_CHILE.Enum.Ambiente;
using static ITA_CHILE.Enum.Ambiente;
using ItaSystem.DTE.WS.EstadoEnvio;

namespace dte1
{
    public class Handler
    {

        public Configuracion configuracion;

        public Handler()
        {
            this.configuracion = new Configuracion();
        }

        public long EnviarEnvioDTEToSII(string filePathEnvio, bool produccion)
        {
            string messageResult = string.Empty;
            long trackID = -1;
            int i;
            try
            {
                for (i = 1; i <= 5; i++)
                {
                    string rutEmisorNumero = configuracion.Empresa.RutEmpresa.Substring(0, configuracion.Empresa.RutEmpresa.Length - 2);
                    string rutEmisorDigito = configuracion.Empresa.RutEmpresa.Substring(configuracion.Empresa.RutEmpresa.Length - 1);
                    string rutEmpresaNumero = configuracion.Empresa.RutEmpresa.Substring(0, configuracion.Empresa.RutEmpresa.Length - 2);
                    string rutEmpresaDigito = configuracion.Empresa.RutEmpresa.Substring(configuracion.Empresa.RutEmpresa.Length - 1);
                    var responseEnvio = bowuaSystem.DTE.WS.EnvioDTE.EnvioDTE.Enviar(rutEmisorNumero+"-"+rutEmisorDigito, rutEmpresaNumero+"-"+rutEmpresaDigito, filePathEnvio, configuracion.Certificado.Nombre, AmbienteEnum.Produccion, out messageResult, ".\\out\\tkn.dat");

                    if (responseEnvio != null || string.IsNullOrEmpty(messageResult))
                    {
                        trackID = responseEnvio.TrackId;
                        string line = trackID.ToString();

                        // Set a variable to the Documents path.
                
                    string docPath = ".//out//";
                       
                        // Write the string array to a new file named "WriteLines.txt".
                        TextWriter tw = new StreamWriter(docPath+"envios.txt", true);
                        tw.WriteLine(line);
                        tw.Close();
                        
                    

                    /*Aquí pueden obtener todos los datos de la respuesta, tal como:
                     * Estado
                     * Fecha
                     * Archivo
                     * Glosa
                     * XML
                     * Entre otros*/
                    return trackID;
                    }
                }

                if (i == 5)
                    throw new Exception("SE HA ALCANZADO EL MÁXIMO NÚMERO DE INTENTOS: " + messageResult);
            }
            catch (Exception ex)
            {
                messageResult = ex.Message;
                return 0;
            }
            return 0;
        }

        

        public DTE GenerateDTE(TipoDTE.DTEType tipoDTE, int folio, string idDTE = "")
        {
            configuracion.LeerArchivo();
            // DOCUMENTO
            var dte = new DTE();
            //
            // DOCUMENTO - ENCABEZADO - CAMPO OBLIGATORIO
            //Id = puede ser compuesto según tus propios requerimientos pero debe ser único                  
            dte.Documento.Id = string.IsNullOrEmpty(idDTE) ? "DTE_" + DateTime.Now.Ticks.ToString() : idDTE;

            // DOCUMENTO - ENCABEZADO - IDENTIFICADOR DEL DOCUMENTO - CAMPOS OBLIGATORIOS
            dte.Documento.Encabezado.IdentificacionDTE.TipoDTE = tipoDTE;
            dte.Documento.Encabezado.IdentificacionDTE.FechaEmision = DateTime.Now;
            dte.Documento.Encabezado.IdentificacionDTE.Folio = folio;

            //DOCUMENTO - ENCABEZADO - EMISOR - CAMPOS OBLIGATORIOS          
            dte.Documento.Encabezado.Emisor.Rut = configuracion.Empresa.RutEmpresa;
            dte.Documento.Encabezado.Emisor.DireccionOrigen = configuracion.Empresa.Direccion;
            dte.Documento.Encabezado.Emisor.ComunaOrigen = configuracion.Empresa.Comuna;

            //Para boletas electrónicas
            if (tipoDTE == TipoDTE.DTEType.BoletaElectronica)
            {
                dte.Documento.Encabezado.IdentificacionDTE.IndicadorServicio = bowuaSystem.DTE.Engine.Enum.IndicadorServicio.IndicadorServicioEnum.BoletaVentasYServicios;
                dte.Documento.Encabezado.Emisor.RazonSocialBoleta = configuracion.Empresa.RazonSocial;
                dte.Documento.Encabezado.Emisor.GiroBoleta = configuracion.Empresa.Giro;
            }
            else
            {
                dte.Documento.Encabezado.Emisor.ActividadEconomica = configuracion.Empresa.CodigosActividades.Select(x => x.Codigo).ToList();
                dte.Documento.Encabezado.Emisor.RazonSocial = configuracion.Empresa.RazonSocial;
                dte.Documento.Encabezado.Emisor.Giro = configuracion.Empresa.Giro;
            }

            if (tipoDTE == TipoDTE.DTEType.GuiaDespachoElectronica)
            {
                dte.Documento.Encabezado.IdentificacionDTE.TipoTraslado = TipoTraslado.TipoTrasladoEnum.OperacionConstituyeVenta;
                dte.Documento.Encabezado.IdentificacionDTE.TipoDespacho = TipoDespacho.TipoDespachoEnum.EmisorACliente;
            }
            //DOCUMENTO - ENCABEZADO - RECEPTOR - CAMPOS OBLIGATORIOS

            dte.Documento.Encabezado.Receptor.Rut = "66666666-6";
            dte.Documento.Encabezado.Receptor.RazonSocial = "Razon Social de Cliente";
            dte.Documento.Encabezado.Receptor.Direccion = "Dirección de cliente";
            dte.Documento.Encabezado.Receptor.Comuna = "Comuna de cliente";
            if (tipoDTE != TipoDTE.DTEType.BoletaElectronica)
            {
                dte.Documento.Encabezado.Receptor.Ciudad = "Ciudad de cliente";
                dte.Documento.Encabezado.Receptor.Giro = "Giro de cliente";
            }


            dte.Documento.Referencias = new List<Referencia>();

            return dte;
        }

        public void GenerateDetails(DTE dte, List<ItemBoleta> items)
        {
            //DOCUMENTO - DETALLES
            dte.Documento.Detalles = new List<bowuaSystem.DTE.Engine.Documento.Detalle>();

            int contador = 1;
            foreach (var det in items)
            {
                var detalle = new bowuaSystem.DTE.Engine.Documento.Detalle();
                detalle.NumeroLinea = contador;
                /*IndicadorExento = Sólo aplica si el producto es exento de IVA*/
                detalle.IndicadorExento = det.Afecto ? bowuaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NotSet : bowuaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NoAfectoOExento;

                detalle.Nombre = det.Nombre;
                detalle.Cantidad = (double)det.Cantidad;
                detalle.Precio = det.Precio;
                if (!string.IsNullOrEmpty(det.UnidadMedida))
                {
                    detalle.UnidadMedida = det.UnidadMedida;
                }
                /*Monto del item*/
                /*Recordar que debe restarse el descuento del detalle y sumarse el recargo*/
                if (det.Descuento != 0)
                {
                    detalle.Descuento = (int)Math.Round(det.Total * (det.Descuento / 100), 0);
                    //detalle.DescuentoPorcentaje = det.Descuento;
                }
                detalle.MontoItem = det.Total - detalle.Descuento;

                if (det.TipoImpuesto != bowuaSystem.DTE.Engine.Enum.TipoImpuesto.TipoImpuestoEnum.NotSet)
                {
                    detalle.CodigoImpuestoAdicional = new List<bowuaSystem.DTE.Engine.Enum.TipoImpuesto.TipoImpuestoEnum>();
                    detalle.CodigoImpuestoAdicional.Add(det.TipoImpuesto);
                }

                dte.Documento.Detalles.Add(detalle);
                contador++;
            }
            calculosTotales(dte);
        }


        private void calculosTotales(DTE dte)
        {
            try
            {
                //DOCUMENTO - ENCABEZADO - TOTALES - CAMPOS OBLIGATORIOS
                if (dte.Documento.Encabezado.IdentificacionDTE.TipoDTE != bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                {
                    dte.Documento.Encabezado.Totales.TasaIVA = Convert.ToDouble(19);
                    var neto = dte.Documento.Detalles
                        .Where(x => x.IndicadorExento == bowuaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NotSet)
                        .Sum(x => x.MontoItem);

                    var exento = dte.Documento.Detalles
                        .Where(x => x.IndicadorExento == bowuaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NoAfectoOExento)
                        .Sum(x => x.MontoItem);

                    var descuentos = dte.Documento.DescuentosRecargos?
                        .Where(x => x.TipoMovimiento == bowuaSystem.DTE.Engine.Enum.TipoMovimiento.TipoMovimientoEnum.Descuento
                        && x.TipoValor == bowuaSystem.DTE.Engine.Enum.ExpresionDinero.ExpresionDineroEnum.Porcentaje)
                        .Sum(x => x.Valor);

                    if (descuentos.HasValue && descuentos.Value > 0)
                    {
                        var montoDescuentoAfecto = (int)Math.Round(neto * (descuentos.Value / 100), 0, MidpointRounding.AwayFromZero);
                        neto -= montoDescuentoAfecto;
                    }
                    var iva = (int)Math.Round(neto * 0.19, 0);
                    int retenido = 0;

                    if (dte.Documento.Detalles.Any(x => x.CodigoImpuestoAdicional != null))
                    {
                        retenido = (int)Math.Round(
                            dte.Documento.Detalles
                            .Where(x => x.CodigoImpuestoAdicional.First() == bowuaSystem.DTE.Engine.Enum.TipoImpuesto.TipoImpuestoEnum.IVARetenidoTotal)
                            .Sum(x => x.MontoItem) * 0.19, 0);

                        if (retenido != 0)
                        {
                            dte.Documento.Encabezado.Totales.ImpuestosRetenciones = new List<bowuaSystem.DTE.Engine.Documento.ImpuestosRetenciones>();
                            dte.Documento.Encabezado.Totales.ImpuestosRetenciones.Add(new bowuaSystem.DTE.Engine.Documento.ImpuestosRetenciones()
                            {
                                MontoImpuesto = retenido,
                                TasaImpuesto = 19,
                                TipoImpuesto = bowuaSystem.DTE.Engine.Enum.TipoImpuesto.TipoImpuestoEnum.IVARetenidoTotal
                            });
                        }
                    }

                    dte.Documento.Encabezado.Totales.MontoNeto = neto;
                    dte.Documento.Encabezado.Totales.MontoExento = exento;
                    dte.Documento.Encabezado.Totales.IVA = iva;
                    dte.Documento.Encabezado.Totales.MontoTotal = neto + exento + iva - retenido;
                }
                else
                {
                    var totalBrutoAfecto = dte.Documento.Detalles
                        .Where(x => x.IndicadorExento == bowuaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NotSet)
                        .Sum(x => x.MontoItem);

                    var totalExento = dte.Documento.Detalles
                        .Where(x => x.IndicadorExento == bowuaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NoAfectoOExento)
                        .Sum(x => x.MontoItem);

                    /*En las boletas, sólo es necesario informar el monto total*/
                    var neto = (int)Math.Round(totalBrutoAfecto / 1.19, 0, MidpointRounding.AwayFromZero);
                    var iva = (int)Math.Round(neto * 0.19, 0, MidpointRounding.AwayFromZero);
                    dte.Documento.Encabezado.Totales.IVA = iva;
                    dte.Documento.Encabezado.Totales.MontoNeto = neto;
                    dte.Documento.Encabezado.Totales.MontoTotal = neto + totalExento + iva;
                }
            }

            catch { }
        }
        public EstadoEnvioResult ConsultarEstadoEnvio(bool produccion, long trackId)
        {
            //string signature = SIMPLE_API.Security.Firma.Firma.GetFirmaFromString(xmlEnvio);
            int rutEmpresa = configuracion.Empresa.RutCuerpo;
            string rutEmpresaDigito = configuracion.Empresa.DV;

            var responseEstadoEnvio = EstadoEnvio.GetEstado(rutEmpresa, rutEmpresaDigito, trackId, configuracion.Certificado.Nombre, AmbienteEnum.Produccion, out string error, ".\\out\\tkn.dat");

            if (!String.IsNullOrEmpty(error))
                throw new Exception(error);

            return responseEstadoEnvio;
        }

        public string TimbrarYFirmarXMLDTE(DTE dte, string pathResultrefere, string pathCaf)
        {
            /*En primer lugar, el documento debe timbrarse con el CAF que descargas desde el SII, es simular
             * cuando antes debías ir con las facturas en papel para que te las timbraran */
            string messageOut = string.Empty;
            dte.Documento.Timbrar(
                EnsureExists((int)dte.Documento.Encabezado.IdentificacionDTE.TipoDTE, dte.Documento.Encabezado.IdentificacionDTE.Folio, pathCaf), 
                out messageOut);

            /*El documento timbrado se guarda en la variable pathResult*/

            /*Finalmente, el documento timbrado debe firmarse con el certificado digital*/
            /*Se debe entregar en el argumento del método Firmar, el "FriendlyName" o Nombre descriptivo del certificado*/
            /*Retorna el filePath donde estará el archivo XML timbrado y firmado, listo para ser enviado al SII*/
            String c=dte.Firmar(Form1.sucur, Form1.equipo, configuracion.Certificado.Nombre, out messageOut, "out\\temp\\", "","",key_manager.key, configuracion.Empresa.RutCuerpo.ToString() + "-" + configuracion.Empresa.DV);
            if (c.Equals(""))
            {
                MessageBox.Show("Error - aplicacion no activa", "Error", MessageBoxButtons.OK);
            }
            return c;
        }

        public bowuaSystem.DTE.Engine.RCOF.ConsumoFolios GenerarRCOF(List<DTE> dtes)
        {
            var rcof = new bowuaSystem.DTE.Engine.RCOF.ConsumoFolios();
            //preparo los datos segun los DTE seleccionados
            DateTime fechaInicio = dtes.Min(x => x.Documento.Encabezado.IdentificacionDTE.FechaEmision);
            DateTime fechaFinal = dtes.Max(x => x.Documento.Encabezado.IdentificacionDTE.FechaEmision);

            rcof.DocumentoConsumoFolios.Caratula.FechaFinal = fechaInicio;
            rcof.DocumentoConsumoFolios.Caratula.NroResol = configuracion.Empresa.NumeroResolucion;
            rcof.DocumentoConsumoFolios.Caratula.RutEmisor = configuracion.Empresa.RutEmpresa;
            rcof.DocumentoConsumoFolios.Caratula.RutEnvia = configuracion.Certificado.Rut;
            rcof.DocumentoConsumoFolios.Caratula.SecEnvio = key_manager.secuencia_envio;
            rcof.DocumentoConsumoFolios.Caratula.FechaEnvio = DateTime.Now;
            List<bowuaSystem.DTE.Engine.RCOF.Resumen> resumenes = new List<bowuaSystem.DTE.Engine.RCOF.Resumen>();

            /*datos de boletas electrónicas afectas*/
            /* Estos datos se deben calcular, debido a que no se informa IVA en boletas electrónicas 
             */
            int totalBrutoAfecto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica)
                        .Sum(x => x.Documento.Detalles
                           .Where(y => y.IndicadorExento == bowuaSystem.DTE.Engine.Enum.IndicadorFacturacionExencion.IndicadorFacturacionExencionEnum.NotSet)
                            .Sum(y => y.MontoItem));


            int totalExento = 0;

            int totalNeto = (int)Math.Round(totalBrutoAfecto / 1.19, 0, MidpointRounding.AwayFromZero);
            int totalIVA = (int)Math.Round(totalNeto * 0.19, 0, MidpointRounding.AwayFromZero);
            int totalTotal = totalExento + totalNeto + totalIVA;

            int rangoInicial = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Min(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            int rangoFinal = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Max(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            resumenes.Add(new bowuaSystem.DTE.Engine.RCOF.Resumen
            {
                FoliosAnulados = 0,
                FoliosEmitidos = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Count(),
                FoliosUtilizados = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Count(),
                MntExento = totalExento,
                MntIva = totalIVA,
                MntNeto = totalNeto,
                MntTotal = totalTotal,
                TasaIVA = 19,
                TipoDocumento = bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica,
                RangoUtilizados = new List<bowuaSystem.DTE.Engine.RCOF.RangoUtilizados>() { new bowuaSystem.DTE.Engine.RCOF.RangoUtilizados() { Inicial = rangoInicial, Final = rangoFinal } }
                //RangoAnulados = new List<bowuaSystem.DTE.Engine.RCOF.RangoAnulados>() { new bowuaSystem.DTE.Engine.RCOF.RangoAnulados() { Final = 0, Inicial = 0 } }
            });

            /*datos de notas de credito electronicas*/
            /*datos de boletas electrónicas afectas*/
            totalNeto = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoNeto);
            totalExento = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoExento);
            totalIVA = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.IVA);
            totalTotal = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Sum(x => x.Documento.Encabezado.Totales.MontoTotal);
            rangoInicial = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Min(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            rangoFinal = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica).Max(x => x.Documento.Encabezado.IdentificacionDTE.Folio);
            resumenes.Add(new bowuaSystem.DTE.Engine.RCOF.Resumen
            {
                FoliosAnulados = 0,
                FoliosEmitidos = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Count(),
                FoliosUtilizados = dtes.Where(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE == bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica).Count(),
                MntExento = totalExento,
                MntIva = totalIVA,
                MntNeto = totalNeto,
                MntTotal = totalTotal,
                TasaIVA = 19,
                TipoDocumento = bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica,
                RangoUtilizados = new List<bowuaSystem.DTE.Engine.RCOF.RangoUtilizados>() { new bowuaSystem.DTE.Engine.RCOF.RangoUtilizados() { Inicial = rangoInicial, Final = rangoFinal } }
                //RangoAnulados =new List<bowuaSystem.DTE.Engine.RCOF.RangoAnulados>() { new bowuaSystem.DTE.Engine.RCOF.RangoAnulados() { Final = 0, Inicial = 0 } }
            });

            rcof.DocumentoConsumoFolios.Resumen = resumenes;
            return rcof;
        }

        //public bool ValidateEnvio(string filePath, bowuaSystem.DTE.Security.Firma.Firma.TipoXML tipo)
        //{
        //    string messageResult = string.Empty;
        //    if (bowuaSystem.DTE.Engine.XML.XmlHandler.ValidateWithSchema(filePath, out messageResult, bowuaSystem.DTE.Engine.XML.Schemas.EnvioDTE))
        //        if (bowuaSystem.DTE.Security.Firma.Firma.VerificarFirma(filePath, tipo))
        //            return true;
        //        else
        //            throw new Exception("NO SE HA PODIDO VERIFICAR LA FIRMA DEL ENVÍO");
        //    throw new Exception(messageResult);
        //}

        public EnvioBoleta GenerarEnvioBoletaDTEToSII(List<DTE> dtes, List<string> xmlDtes)
        {
            var EnvioSII = new bowuaSystem.DTE.Engine.Envio.EnvioBoleta();
            EnvioSII.SetDTE = new bowuaSystem.DTE.Engine.Envio.SetDTE();
            var date1 = DateTime.Now.Date;
            string v =date1.ToString("ddMMyyyy",CultureInfo.InvariantCulture);        
             EnvioSII.SetDTE.Id = "FENV010"+ "_"+v;
            /*Es necesario agregar en el envío, los objetos DTE como sus respectivos XML en strings*/
            foreach (var a in dtes)
                EnvioSII.SetDTE.DTEs.Add(a);
            foreach (var a in xmlDtes)
            {
                EnvioSII.SetDTE.dteXmls.Add(a);
                EnvioSII.SetDTE.signedXmls.Add(a);
            }

            EnvioSII.SetDTE.Caratula = new bowuaSystem.DTE.Engine.Envio.Caratula();
            EnvioSII.SetDTE.Caratula.FechaEnvio = DateTime.Now;
            /*Fecha de Resolución y Número de Resolución se averiguan en el sitio del SII según ambiente de producción o certificación*/
            EnvioSII.SetDTE.Caratula.FechaResolucion = configuracion.Empresa.FechaResolucion;
            EnvioSII.SetDTE.Caratula.NumeroResolucion = configuracion.Empresa.NumeroResolucion;

            EnvioSII.SetDTE.Caratula.RutEmisor = configuracion.Empresa.RutEmpresa;
            EnvioSII.SetDTE.Caratula.RutEnvia = configuracion.Certificado.Rut;
            EnvioSII.SetDTE.Caratula.RutReceptor = "60803000-K"; //Este es el RUT del SII
            EnvioSII.SetDTE.Caratula.SubTotalesDTE = new List<bowuaSystem.DTE.Engine.Envio.SubTotalesDTE>();

            /*En la carátula del envío, se debe indicar cuantos documentos de cada tipo se están enviando*/
            var tipos = EnvioSII.SetDTE.DTEs.GroupBy(x => x.Documento.Encabezado.IdentificacionDTE.TipoDTE);
            foreach (var a in tipos)
            {
                EnvioSII.SetDTE.Caratula.SubTotalesDTE.Add(new bowuaSystem.DTE.Engine.Envio.SubTotalesDTE()
                {
                    Cantidad = a.Count(),
                    TipoDTE = a.ElementAt(0).Documento.Encabezado.IdentificacionDTE.TipoDTE
                });
            }
            return EnvioSII;
        }
        public bool Validate(string filePath, Firma.TipoXML tipo, string schema)
        {
            string messageResult = string.Empty;
            if (bowuaSystem.DTE.Engine.XML.XmlHandler.ValidateWithSchema(filePath, out messageResult, schema))
                if (BOWUA_CHILE.Security.Firma.Firma.VerificarFirma(filePath, tipo, out string messageOutFirma))
                    return true;
                else
                    MessageBox.Show("Error al validar firma electrónica: " + messageResult + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        
            MessageBox.Show("Error: " + messageResult + ". Verifique que contiene la carpeta XML con los XSD para validación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private string EnsureExists(int tipoDTE, int folio, string pathCaf)
        {
            var cafFile = string.Empty;
            try
            {
                
                foreach (var file in System.IO.Directory.GetFiles(pathCaf))
                    if (ParseName((new FileInfo(file)).Name, tipoDTE, folio))
                        cafFile = file;
                if (string.IsNullOrEmpty(cafFile))
                    throw new Exception("NO HAY UN CÓDIGO DE AUTORIZACIÓN DE FOLIOS (CAF) ASIGNADO PARA ESTE TIPO DE DOCUMENTO (" + tipoDTE + ") QUE INCLUYA EL FOLIO REQUERIDO (" + folio + ").");
                
            }
            catch (Exception ex) { 
                            MessageBox.Show(""+ ex);
            }
            return cafFile;
        }

        private static bool ParseName(string name, int tipoDTE, int folio)
        {
            try
            {
                var values = name.Substring(0, name.IndexOf('.')).Split('_');
                int tipo = Convert.ToInt32(values[0]);
                int desde = Convert.ToInt32(values[1]);
                int hasta = Convert.ToInt32(values[2]);
     
                return tipoDTE == tipo && desde <= folio && folio <= hasta;
            }
            catch { return false; }
        }

    
        public string ObtenerCertificados()
        {
            var certificadosMaquina = bowuaSystem.DTE.Engine.Utilidades.ObtenerCertificadosMaquinas();
            var certificadosUsuario = bowuaSystem.DTE.Engine.Utilidades.ObtenerCertificadosUsuario();
            string result = "Máquina:\n";
            foreach (var a in certificadosMaquina)
                result += a + "\n";
            result += "Usuario:\n";
            foreach (var a in certificadosUsuario)
                result += a + "\n";
            return result;
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public static string TipoDTEString(TipoDTE.DTEType tipo)
        {
            switch (tipo)
            {
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.FacturaCompraElectronica: return "FACTURA DE COMPRA ELECTRÓNICA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.FacturaElectronica: return "FACTURA ELECTRÓNICA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.FacturaElectronicaExenta: return "FACTURA ELECTRÓNICA EXENTA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.GuiaDespachoElectronica: return "GUIA DE DESPACHO ELECTRÓNICA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica: return "NOTA DE CRÉDITO ELECTRÓNICA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaDebitoElectronica: return "NOTA DE DÉBITO ELECTRÓNICA";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.Factura: return "FACTURA MANUAL";
                case bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.NotaCredito: return "NOTA DE CRÉDITO MANUAL";
            }
            return "Not Set";
        }

    }
}
