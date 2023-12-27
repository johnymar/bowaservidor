using dte1.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace dte1
{
    public partial class GenerarBoletaElectronica : Form
    {
        Handler handler = new Handler();
        List<ItemBoleta> items;

        public GenerarBoletaElectronica()
        {
            InitializeComponent();
            items = new List<ItemBoleta>();
        }

        private void GenerarBoletaElectronica_Load(object sender, EventArgs e)
        {
            gridResultados.AutoGenerateColumns = false;
            handler.configuracion = new Configuracion();
            handler.configuracion.LeerArchivo();
        }

        

        

        private void gridResultados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 5)
                {
                    var item = gridResultados.Rows[e.RowIndex].DataBoundItem as ItemBoleta;
                    items.Remove(item);
                    gridResultados.DataSource = null;
                    gridResultados.DataSource = items;
                }
            }
        }

        public string name = "0";
        private void botonGenerar_Click(object sender, EventArgs e)
        {
            var dte = handler.GenerateDTE(bowuaSystem.DTE.Engine.Enum.TipoDTE.DTEType.BoletaElectronica, (int)numericFolio.Value);
            handler.GenerateDetails(dte, items);
            Referencias(dte, TipoReferencia.TipoReferenciaEnum.SetPruebas, TipoDTE.TipoReferencia.BoletaElectronica, null, 0, numericCasoPrueba.Text);
            var path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
           // handler.Validate(path, BOWUA_CHILE.Security.Firma.Firma.TipoXML.DTE, bowuaSystem.DTE.Engine.XML.Schemas.DTE);
            MessageBox.Show("Documento generado exitosamente");
        }

        public void Referencias(DTE dte, TipoReferencia.TipoReferenciaEnum operacionReferencia, TipoDTE.TipoReferencia tipoDocumentoReferencia, DateTime? fechaDocReferencia, int? folioReferencia = 0, string casoPrueba = "")
        {
            casoPrueba = "CASO-" + numericCasoPrueba.Value.ToString("N0");
            if (operacionReferencia == TipoReferencia.TipoReferenciaEnum.SetPruebas)  //REFERENCIA A SET DE PRUEBAS
            {
                if (tipoDocumentoReferencia == TipoDTE.TipoReferencia.BoletaElectronica || tipoDocumentoReferencia == TipoDTE.TipoReferencia.BoletaExentaElectronica)
                {
                    dte.Documento.Referencias.Add(new bowuaSystem.DTE.Engine.Documento.Referencia()
                    {
                        CodigoReferencia = TipoReferencia.TipoReferenciaEnum.SetPruebas,
                        Numero = dte.Documento.Referencias.Count + 1,
                        RazonReferencia = casoPrueba,
                    });
                }
                else
                {
                    dte.Documento.Referencias.Add(new bowuaSystem.DTE.Engine.Documento.Referencia()
                    {
                        CodigoReferencia = operacionReferencia,
                        FechaDocumentoReferencia = fechaDocReferencia.Value,
                        FolioReferencia = folioReferencia.ToString(),
                        Numero = dte.Documento.Referencias.Count + 1,
                        RazonReferencia = casoPrueba,
                        TipoDocumento = tipoDocumentoReferencia
                    });
                }
            }
            else
            {
                dte.Documento.Referencias.Add(new bowuaSystem.DTE.Engine.Documento.Referencia()
                {
                    CodigoReferencia = operacionReferencia,
                    FechaDocumentoReferencia = fechaDocReferencia.Value,
                    FolioReferencia = folioReferencia.ToString(),
                    Numero = dte.Documento.Referencias.Count + 1,
                    RazonReferencia = operacionReferencia == TipoReferencia.TipoReferenciaEnum.AnulaDocumentoReferencia ? "ANULA" : "CORRIGE" + " DOCUMENTO N° " + folioReferencia.ToString(),
                    TipoDocumento = tipoDocumentoReferencia
                });
            }
        }
        private void GenerarBoletaElectronica_Load_1(object sender, EventArgs e)
        {

        }

        private void botonAgregarLinea_Click_1(object sender, EventArgs e)
        {
            
            ItemBoleta item = new ItemBoleta();
            item.Nombre = textNombre.Text;
            item.Cantidad = numericCantidad.Value;
            item.Afecto = checkAfecto.Checked;
            item.Precio = (int)numericPrecio.Value;
            item.UnidadMedida = checkUnidad.Checked ? "Kg." : string.Empty;
            items.Add(item);
            gridResultados.DataSource = null;
            gridResultados.DataSource = items;

            textNombre.Text = "";
            numericCantidad.Value = 1;
            checkAfecto.Checked = true;
         //   items.Add(item);
        }
    }
    }
