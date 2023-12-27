using dte1.clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dte1
{
    public partial class ConfiguracionSistema : Form
    {
        private Button botonGuardar;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private Label label61;
        private GroupBox groupBox2;
        private NumericUpDown numericNResolucion;
        private DateTimePicker dateFechaResolucion;
        private Label label1;
        private Label label4;
        private Label label56;
        private GroupBox groupBox3;
        private TextBox textComuna;
        private TextBox textRazonSocial;
        private Label label57;
        private TextBox textRutEmpresa;
        private TextBox textGiro;
        private Label label55;
        private TextBox textDireccionEmpresa;
        private Label label54;
        private GroupBox groupBox1;
        private ComboBox comboCertificados;
        private Label label2;
        private Label label3;
        private TextBox textRutCertificado;
        Configuracion configuracion = new Configuracion();

        public ConfiguracionSistema()
        {
            InitializeComponent();
        }

       

        private void InitializeComponent()
        {
            this.botonGuardar = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.label61 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericNResolucion = new System.Windows.Forms.NumericUpDown();
            this.dateFechaResolucion = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textComuna = new System.Windows.Forms.TextBox();
            this.textRazonSocial = new System.Windows.Forms.TextBox();
            this.label57 = new System.Windows.Forms.Label();
            this.textRutEmpresa = new System.Windows.Forms.TextBox();
            this.textGiro = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.textDireccionEmpresa = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboCertificados = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textRutCertificado = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericNResolucion)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // botonGuardar
            // 
            this.botonGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.botonGuardar.Location = new System.Drawing.Point(265, 32);
            this.botonGuardar.Name = "botonGuardar";
            this.botonGuardar.Size = new System.Drawing.Size(79, 33);
            this.botonGuardar.TabIndex = 38;
            this.botonGuardar.Text = "Guardar";
            this.botonGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.botonGuardar.UseVisualStyleBackColor = true;
            this.botonGuardar.Click += new System.EventHandler(this.botonGuardar_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 35;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(9, 100);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(49, 13);
            this.label61.TabIndex = 44;
            this.label61.Text = "Comuna:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.botonGuardar);
            this.groupBox2.Controls.Add(this.numericNResolucion);
            this.groupBox2.Controls.Add(this.dateFechaResolucion);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(-6, 240);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(352, 78);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Resolución";
            // 
            // numericNResolucion
            // 
            this.numericNResolucion.Location = new System.Drawing.Point(118, 20);
            this.numericNResolucion.Name = "numericNResolucion";
            this.numericNResolucion.Size = new System.Drawing.Size(48, 20);
            this.numericNResolucion.TabIndex = 35;
            // 
            // dateFechaResolucion
            // 
            this.dateFechaResolucion.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateFechaResolucion.Location = new System.Drawing.Point(118, 45);
            this.dateFechaResolucion.Name = "dateFechaResolucion";
            this.dateFechaResolucion.Size = new System.Drawing.Size(112, 20);
            this.dateFechaResolucion.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "N° Resolución:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Fecha Resolución:";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(9, 22);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(33, 13);
            this.label56.TabIndex = 32;
            this.label56.Text = "RUT:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label61);
            this.groupBox3.Controls.Add(this.label56);
            this.groupBox3.Controls.Add(this.textComuna);
            this.groupBox3.Controls.Add(this.textRazonSocial);
            this.groupBox3.Controls.Add(this.label57);
            this.groupBox3.Controls.Add(this.textRutEmpresa);
            this.groupBox3.Controls.Add(this.textGiro);
            this.groupBox3.Controls.Add(this.label55);
            this.groupBox3.Controls.Add(this.textDireccionEmpresa);
            this.groupBox3.Controls.Add(this.label54);
            this.groupBox3.Location = new System.Drawing.Point(-6, -1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(352, 151);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Datos Empresa";
            // 
            // textComuna
            // 
            this.textComuna.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textComuna.Location = new System.Drawing.Point(118, 97);
            this.textComuna.Name = "textComuna";
            this.textComuna.Size = new System.Drawing.Size(87, 20);
            this.textComuna.TabIndex = 30;
            // 
            // textRazonSocial
            // 
            this.textRazonSocial.Location = new System.Drawing.Point(118, 45);
            this.textRazonSocial.Name = "textRazonSocial";
            this.textRazonSocial.Size = new System.Drawing.Size(226, 20);
            this.textRazonSocial.TabIndex = 10;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(9, 48);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(73, 13);
            this.label57.TabIndex = 30;
            this.label57.Text = "Razón Social:";
            // 
            // textRutEmpresa
            // 
            this.textRutEmpresa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textRutEmpresa.Location = new System.Drawing.Point(118, 19);
            this.textRutEmpresa.Name = "textRutEmpresa";
            this.textRutEmpresa.Size = new System.Drawing.Size(87, 20);
            this.textRutEmpresa.TabIndex = 0;
            // 
            // textGiro
            // 
            this.textGiro.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textGiro.Location = new System.Drawing.Point(118, 71);
            this.textGiro.Name = "textGiro";
            this.textGiro.Size = new System.Drawing.Size(226, 20);
            this.textGiro.TabIndex = 20;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(9, 74);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(78, 13);
            this.label55.TabIndex = 34;
            this.label55.Text = "Giro Comercial:";
            // 
            // textDireccionEmpresa
            // 
            this.textDireccionEmpresa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textDireccionEmpresa.Location = new System.Drawing.Point(118, 123);
            this.textDireccionEmpresa.Name = "textDireccionEmpresa";
            this.textDireccionEmpresa.Size = new System.Drawing.Size(226, 20);
            this.textDireccionEmpresa.TabIndex = 40;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(9, 126);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(55, 13);
            this.label54.TabIndex = 36;
            this.label54.Text = "Dirección:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboCertificados);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textRutCertificado);
            this.groupBox1.Location = new System.Drawing.Point(-6, 156);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 78);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos Certificado";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // comboCertificados
            // 
            this.comboCertificados.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCertificados.FormattingEnabled = true;
            this.comboCertificados.Location = new System.Drawing.Point(118, 45);
            this.comboCertificados.Name = "comboCertificados";
            this.comboCertificados.Size = new System.Drawing.Size(226, 21);
            this.comboCertificados.TabIndex = 34;
            this.comboCertificados.SelectedIndexChanged += new System.EventHandler(this.comboCertificados_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "RUT:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Nombre Descriptivo:";
            // 
            // textRutCertificado
            // 
            this.textRutCertificado.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textRutCertificado.Location = new System.Drawing.Point(118, 19);
            this.textRutCertificado.Name = "textRutCertificado";
            this.textRutCertificado.Size = new System.Drawing.Size(112, 20);
            this.textRutCertificado.TabIndex = 33;
            // 
            // ConfiguracionSistema
            // 
            this.ClientSize = new System.Drawing.Size(348, 337);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "ConfiguracionSistema";
            this.Text = "CERTIFICADO";
            this.Load += new System.EventHandler(this.ConfiguracionSistema_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericNResolucion)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void ConfiguracionSistema_Load(object sender, EventArgs e)
        {
            //gridResultados.AutoGenerateColumns = gridProductos.AutoGenerateColumns = false;

            try
            {
                X509Store store = new X509Store(StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = store.Certificates;
                foreach (X509Certificate2 c in certCollection)
                {
                    comboCertificados.Items.Add(c.FriendlyName);
                }
            }
            catch { }


            try
            {
                configuracion.LeerArchivo();

                textRutEmpresa.Text = configuracion.Empresa.RutEmpresa;
                textGiro.Text = configuracion.Empresa.Giro;
                textRazonSocial.Text = configuracion.Empresa.RazonSocial;
                textComuna.Text = configuracion.Empresa.Comuna;
                textDireccionEmpresa.Text = configuracion.Empresa.Direccion;
                textRutCertificado.Text = configuracion.Certificado.Rut;
                comboCertificados.SelectedItem = configuracion.Certificado.Nombre;
                numericNResolucion.Value = configuracion.Empresa.NumeroResolucion;
                dateFechaResolucion.Value = configuracion.Empresa.FechaResolucion;
                //textAPIKey.Text = configuracion.APIKey;

                //gridResultados.DataSource = null;
                // gridResultados.DataSource = configuracion.Empresa.CodigosActividades;

                //gridProductos.DataSource = null;
                //gridProductos.DataSource = configuracion.ProductosSimulacion;
            }
            catch { }
        }

        private void botonGuardar_Click(object sender, EventArgs e)
        {
            configuracion.Empresa.RutEmpresa = textRutEmpresa.Text;
            configuracion.Empresa.Giro = textGiro.Text;
            configuracion.Empresa.RazonSocial = textRazonSocial.Text;
            configuracion.Empresa.Comuna = textComuna.Text;
            configuracion.Empresa.Direccion = textDireccionEmpresa.Text;
            configuracion.Empresa.NumeroResolucion = (int)numericNResolucion.Value;
            configuracion.Empresa.FechaResolucion = dateFechaResolucion.Value.Date;
           

            configuracion.Certificado.Rut = textRutCertificado.Text;
            configuracion.Certificado.Nombre = comboCertificados.SelectedItem.ToString();

            configuracion.GenerarArchivo();

            MessageBox.Show("Configuración guardada correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void comboCertificados_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
