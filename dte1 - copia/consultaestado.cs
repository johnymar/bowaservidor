using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using bd;

namespace dte1
{
    public partial class consultaestado : Form
    {
        public static string Cliente = "";
        public static string trackid = "";
        public static string Estado = "";
        public static string Glosa = "";
        public static string Numatencion = "";
        public static string Fecha = "";
        public static string hora = "";
        public static string dte = "";
        public static string tipoDoc = "";
        public static string Informados = "";
        public static string aceptados = "";
        public static string rechazados = "";
        public static string reparos = "";
        Handler handler = new Handler();
        private DBConnect db;
        

        public consultaestado()
        {
            db = new DBConnect();
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            handler.configuracion.LeerArchivo();
            textRUTEmpresa.Text = handler.configuracion.Empresa.RutCuerpo.ToString();
            textDVEmpresa.Text = handler.configuracion.Empresa.DV;
            string docPath = ".//out//";

            List<string> lines = File.ReadAllLines(docPath + "envios.txt").ToList();
            foreach (string elemento in lines)
            {
                comboBox1.Items.Add(elemento);
            }
        }

        private void botonConsultar_Click(object sender, EventArgs e)
        {
            string docPath = ".//out//chequeado//";
            long trackId = long.Parse(comboBox1.SelectedItem.ToString());
            try
            {
                var responseEstadoDTE = handler.ConsultarEstadoEnvio(radioProduccion.Checked, trackId);
                textRespuesta.Text = responseEstadoDTE.ResponseXml;

                var doc = new XmlDocument();
                doc.LoadXml(responseEstadoDTE.ResponseXml);
                doc.PreserveWhitespace = true;
                doc.Save(docPath + "respuesta_" + comboBox1.SelectedItem.ToString() + ".xml");

                string[] dato = new string[4];
                string[] dato1 = new string[5];
                
             
                

                trackid = dato[0];
                Estado = dato[1];
                Glosa = dato[2];
                string s=dato[3];
               /* if (!dato[3].Equals(null)){
                    string[] f = dato[3].Split('(');
                    Numatencion = f[0];
                    string[] h = f[1].Split(' ');
                    Fecha = h[0];
                    hora = h[1];
                }
                

                tipoDoc = dato1[0];
                Informados = dato1[1];
                aceptados = dato1[2];
                rechazados = dato1[3];
                reparos = dato1[4];
                if (tipoDoc.Equals(null))
                {
                    dte = "ConsumoFolios";
                }
                else
                {
                    dte = "EnvioBoleta";
                }
                db.insertar();*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error:" + ex);
            }


        }
    }
}
