using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Windows.Forms;


using System.Xml;
using dte1.clases;
using Clave_manager;

namespace dte1
{
    public partial class Form1 : Form
    {
        Handler handler = new Handler();
        Configuracion configuracion = new Configuracion();

        public static string sucur = "";
        public static string equipo = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {



            sucur = sucu.Text;
            equipo = equi.Text;
            configuracion.LeerArchivo();
            handler.configuracion = configuracion;

            //  var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
            // foreach (var print in printers)
            //     comboPrinters.Items.Add(print);

        }


        private void button2_Click(object sender, EventArgs e)
        {
            ConfiguracionSistema formulario = new ConfiguracionSistema();
            formulario.ShowDialog();
            handler.configuracion.LeerArchivo();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IngresarTimbraje formulario = new IngresarTimbraje();
            formulario.ShowDialog();
        }
        public string key = "";
        private void button4_Click(object sender, EventArgs e)
        {
            sucur = sucu.Text;
            equipo = equip.Text;
            openFileDialog1.ShowDialog();
            if (File.Exists(openFileDialog1.FileName))
            {
                string pathFile = openFileDialog1.FileName;
                string xml1 = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                var dte = bowuaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<bowuaSystem.DTE.Engine.Documento.DTE>(xml1);
                var path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
                if (!path.Equals(""))
                {
                    handler.Validate(path, BOWUA_CHILE.Security.Firma.Firma.TipoXML.DTE, bowuaSystem.DTE.Engine.XML.Schemas.DTE);
                    MessageBox.Show("boleta firmada: " + path);
                    //    Clave.SubirArchivo( path, handler.configuracion.Empresa.RutCuerpo.ToString()+"-"+ handler.configuracion.Empresa.DV);
                }
            }
        }



        private void button5_Click(object sender, EventArgs e)
        {
#pragma warning disable CS0436 // El tipo entra en conflicto con un tipo importado
            //   PrintableDocument document = new ThermalPrinting.Core.PrintableDocument();
#pragma warning restore CS0436 // El tipo entra en conflicto con un tipo importado
            try
            {
                string pathFile = @"C:\\Users\\admin\\source\\repos\\dte\\bin\\Debug\\out\\temp\\DTE_39_76571997-6_61.xml";
                string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));

                var dte = bowuaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<bowuaSystem.DTE.Engine.Documento.DTE>(xml);

#pragma warning disable CS0436 // El tipo entra en conflicto con un tipo importado
                //    document = PrintableDocument.FromDTE(dte);
#pragma warning restore CS0436 // El tipo entra en conflicto con un tipo importado
                //     document.TimbreImage = dte.Documento.TimbrePDF417(out string outMessage);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar el archivo.\n\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // ThermalPrintHandler handler = new ThermalPrintHandler(document);
            DateTime dateFechaResolucion = DateTime.Now;
            //  document.FechaResolucion = dateFechaResolucion;
            //   handler.NombreImpresora = comboPrinters.Text;
            //  handler.Print(true);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            openFileDialog1.ShowDialog();
            try
            {
                if (File.Exists(openFileDialog1.FileName))
                {
                    string pathFile = openFileDialog1.FileName;
                    string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                    var dte1 = bowuaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<bowuaSystem.DTE.Engine.Documento.DTE>(xml);
                    String outMessage = "Imagen lista";
                    pictureBox1.BackgroundImage = dte1.Documento.TimbrePDF417(out outMessage);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(pathFile);
                    XmlNodeList elemList = doc.GetElementsByTagName("TED");
                    for (int i = 0; i < elemList.Count; i++)
                    {
                        textBox1.Text = textBox1.Text + (elemList[i].OuterXml) + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al cargar el archivo.\n\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            button5.Text = "ESPERE...";
            sucur = sucu.Text;
            equipo = equi.Text;
            key_manager.key = Clave.Bajar_key(handler.configuracion.Empresa.RutCuerpo.ToString() + "-" + handler.configuracion.Empresa.DV, sucur, equipo);
            button5.Text = key_manager.key;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            string[] pathFiles = openFileDialog1.FileNames;
            List<bowuaSystem.DTE.Engine.Documento.DTE> dtes = new List<bowuaSystem.DTE.Engine.Documento.DTE>();
            foreach (string pathFile in pathFiles)
            {
                string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                var dte = bowuaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<bowuaSystem.DTE.Engine.Documento.DTE>(xml);
                dtes.Add(dte);
            }
            var rcof = handler.GenerarRCOF(dtes);
            rcof.DocumentoConsumoFolios.Id = "RCOF_" + DateTime.Now.Ticks.ToString();
            /*Firmar retorna además a través de un out, el XML formado*/
            string xmlString = string.Empty;
            var filePathArchivo = rcof.Firmar(sucur, equipo, configuracion.Certificado.Nombre, out xmlString, "out\\temp\\", "", "", handler.configuracion.Empresa.RutCuerpo.ToString() + "-" + handler.configuracion.Empresa.DV);

            if (xmlString == "")
            {
                MessageBox.Show("Error Aplicacion Inactiva");

            }
            else
            {
                MessageBox.Show("RCOF Generado correctamente en " + filePathArchivo);
                //.Validate(filePathArchivo, BOWUA_CHILE.Security.Firma.Firma.TipoXML.DTE, bowuaSystem.DTE.Engine.XML.Schemas.ConsumoFolios);
            }

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            string p1 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\out\\temp\\";

            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            string[] pathFiles = openFileDialog1.FileNames;
            List<bowuaSystem.DTE.Engine.Documento.DTE> dtes = new List<bowuaSystem.DTE.Engine.Documento.DTE>();
            List<string> xmlDtes = new List<string>();
            foreach (string pathFile in pathFiles)
            {
                string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                var dte = bowuaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<bowuaSystem.DTE.Engine.Documento.DTE>(xml);
                dtes.Add(dte);
                xmlDtes.Add(xml);
            }
            var EnvioSII = handler.GenerarEnvioBoletaDTEToSII(dtes, xmlDtes);
            var filePath = EnvioSII.Firmar(sucur, equipo, configuracion.Certificado.Nombre, true, "out\\temp\\", "", handler.configuracion.Empresa.RutCuerpo.ToString() + "-" + handler.configuracion.Empresa.DV);
            if (filePath == "")
            {
                MessageBox.Show("Aplicacion no Activada");
            }
            else
            {
                try
                {
                    MessageBox.Show("Envio Boleta Generado correctamente en " + p1 + filePath);

                    // handler.Validate(filePath, BOWUA_CHILE.Security.Firma.Firma.TipoXML.EnvioBoleta, bowuaSystem.DTE.Engine.XML.Schemas.EnvioBoleta);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            string pathFile = openFileDialog1.FileName;
            long trackId = handler.EnviarEnvioDTEToSII(pathFile, false);
            MessageBox.Show("Sobre enviado correctamente. TrackID: " + trackId.ToString());

        }
        public long EnviarEnvioDTEToSII(string filePathEnvio, AmbienteEnum ambiente, string pp, bool nuevaBoleta = false)
        {


            
            configuracion.LeerArchivo();
            string messageResult = string.Empty;
            long trackID = -1;
            int i;
            try
            {
                for (i = 1; i <= 5; i++)
                {
                    EnvioDTEResult responseEnvio = new EnvioDTEResult();

                    if (nuevaBoleta) responseEnvio = bowuaSystem.DTE.WS.EnvioBoleta.EnvioBoleta.Enviar(configuracion.Certificado.Rut, configuracion.Empresa.RutEmpresa, filePathEnvio, configuracion.Certificado.Nombre, ambiente, out messageResult, ".\\out\\tkn.dat");
                    else responseEnvio = bowuaSystem.DTE.WS.EnvioDTE.EnvioDTE.Enviar(configuracion.Certificado.Rut, configuracion.Empresa.RutEmpresa, filePathEnvio, configuracion.Certificado.Nombre, ambiente, out messageResult, ".\\out\\tkn.dat", "");

                    if (responseEnvio != null || string.IsNullOrEmpty(messageResult))
                    {
                        trackID = responseEnvio.TrackId;

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
                return 0;
            }
            return 0;

        }

      

        private void button9_Click_1(object sender, EventArgs e)
        {
            consultaestado formulario = new consultaestado();
            formulario.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string outMessage = "";
            openFileDialog1.ShowDialog();
            string pathFile = openFileDialog1.FileName;

            string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
            var dte1 = bowuaSystem.DTE.Engine.XML.XmlHandler.DeserializeFromString<bowuaSystem.DTE.Engine.Documento.DTE>(xml);
                outMessage = "Imagen lista";
                pictureBox1.BackgroundImage = dte1.Documento.TimbrePDF417(out outMessage);
            XmlDocument doc = new XmlDocument();
            doc.Load(pathFile);
            XmlNodeList elemList = doc.GetElementsByTagName("TED");
                for (int i = 0; i < elemList.Count; i++)
                {
                    textBox1.Text = textBox1.Text + (elemList[i].OuterXml) + "\r\n";
                }
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            GenerarBoletaElectronica formulario = new GenerarBoletaElectronica();
            formulario.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            handler.configuracion.LeerArchivo();
            String status=bowuaSystem.DTE.Engine.CAFHandler.CAFHandler.enviar_caf("out\\caf\\", 1, 1, configuracion.Empresa.RutEmpresa);
            MessageBox.Show("Resultado al adjuntar CAF: " + status);
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)

        {
            string status="Archivo no existe";
            string path = @"C:\Users\Johnymar\source\repos\repos\dte1\bin\Debug\out\temp\\DTE_39_77187151-8_1.xml";
            if (File.Exists(path))
            {
                status = bowuaSystem.DTE.Engine.CAFHandler.CAFHandler.subir_boleta(configuracion.Empresa.RutEmpresa, 1, 1, @"C:\Users\Johnymar\source\repos\repos\dte1\bin\Debug\out\temp\\", "DTE_39_77187151-8_1.xml");

            }
            MessageBox.Show("Resultado al enviar xml al servidor: " + status);

        }

        private void button13_Click(object sender, EventArgs e)
        {
            string status = "Archivo no existe";
            if (File.Exists(@"c:\\Users\\Usuario\\source\\repos\\dte1\\bin\\Debug\\out\\temp\\100.pdf"))
            {
                status = bowuaSystem.DTE.Engine.CAFHandler.CAFHandler.subir_pdf(configuracion.Empresa.RutEmpresa, 1, 1, @"c:\\Users\\Usuario\\source\\repos\\dte1\\bin\\Debug\\out\\temp\\", "100.pdf");
            }
            MessageBox.Show("Resultado al enviar pdf al servidor: " + status);
        }

        private void sucu_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
