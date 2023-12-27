﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace BOWA_ITA.Certificacion.RegistroReclamoWS {
    using System.Diagnostics;
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="RegistroReclamoDteServicePortBinding", Namespace="http://ws.registroreclamodte.diii.sdi.sii.cl")]
    public partial class RegistroReclamoDteServiceEndpointService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback consultarDocDteCedibleOperationCompleted;
        
        private System.Threading.SendOrPostCallback consultarFechaRecepcionSiiOperationCompleted;
        
        private System.Threading.SendOrPostCallback getVersionOperationCompleted;
        
        private System.Threading.SendOrPostCallback ingresarAceptacionReclamoDocOperationCompleted;
        
        private System.Threading.SendOrPostCallback listarEventosHistDocOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        internal string Token;

        /// <remarks/>
        public RegistroReclamoDteServiceEndpointService() {
            this.Url = global::BOWA_ITA.Properties.Settings.Default.BOWA_ITA_Certificacion_RegistroReclamoWS_RegistroReclamoDteServiceEndpointService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event consultarDocDteCedibleCompletedEventHandler consultarDocDteCedibleCompleted;
        
        /// <remarks/>
        public event consultarFechaRecepcionSiiCompletedEventHandler consultarFechaRecepcionSiiCompleted;
        
        /// <remarks/>
        public event getVersionCompletedEventHandler getVersionCompleted;
        
        /// <remarks/>
        public event ingresarAceptacionReclamoDocCompletedEventHandler ingresarAceptacionReclamoDocCompleted;
        
        /// <remarks/>
        public event listarEventosHistDocCompletedEventHandler listarEventosHistDocCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://ws.registroreclamodte.diii.sdi.sii.cl", ResponseNamespace="http://ws.registroreclamodte.diii.sdi.sii.cl", Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public respuestaTo consultarDocDteCedible(string rutEmisor, string dvEmisor, string tipoDoc, string folio) {
            object[] results = this.Invoke("consultarDocDteCedible", new object[] {
                        rutEmisor,
                        dvEmisor,
                        tipoDoc,
                        folio});
            return ((respuestaTo)(results[0]));
        }
        
        /// <remarks/>
        public void consultarDocDteCedibleAsync(string rutEmisor, string dvEmisor, string tipoDoc, string folio) {
            this.consultarDocDteCedibleAsync(rutEmisor, dvEmisor, tipoDoc, folio, null);
        }
        
        /// <remarks/>
        public void consultarDocDteCedibleAsync(string rutEmisor, string dvEmisor, string tipoDoc, string folio, object userState) {
            if ((this.consultarDocDteCedibleOperationCompleted == null)) {
                this.consultarDocDteCedibleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnconsultarDocDteCedibleOperationCompleted);
            }
            this.InvokeAsync("consultarDocDteCedible", new object[] {
                        rutEmisor,
                        dvEmisor,
                        tipoDoc,
                        folio}, this.consultarDocDteCedibleOperationCompleted, userState);
        }
        
        private void OnconsultarDocDteCedibleOperationCompleted(object arg) {
            if ((this.consultarDocDteCedibleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.consultarDocDteCedibleCompleted(this, new consultarDocDteCedibleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://ws.registroreclamodte.diii.sdi.sii.cl", ResponseNamespace="http://ws.registroreclamodte.diii.sdi.sii.cl", Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public string consultarFechaRecepcionSii(string rutEmisor, string dvEmisor, string tipoDoc, string folio) {
            object[] results = this.Invoke("consultarFechaRecepcionSii", new object[] {
                        rutEmisor,
                        dvEmisor,
                        tipoDoc,
                        folio});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void consultarFechaRecepcionSiiAsync(string rutEmisor, string dvEmisor, string tipoDoc, string folio) {
            this.consultarFechaRecepcionSiiAsync(rutEmisor, dvEmisor, tipoDoc, folio, null);
        }
        
        /// <remarks/>
        public void consultarFechaRecepcionSiiAsync(string rutEmisor, string dvEmisor, string tipoDoc, string folio, object userState) {
            if ((this.consultarFechaRecepcionSiiOperationCompleted == null)) {
                this.consultarFechaRecepcionSiiOperationCompleted = new System.Threading.SendOrPostCallback(this.OnconsultarFechaRecepcionSiiOperationCompleted);
            }
            this.InvokeAsync("consultarFechaRecepcionSii", new object[] {
                        rutEmisor,
                        dvEmisor,
                        tipoDoc,
                        folio}, this.consultarFechaRecepcionSiiOperationCompleted, userState);
        }
        
        private void OnconsultarFechaRecepcionSiiOperationCompleted(object arg) {
            if ((this.consultarFechaRecepcionSiiCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.consultarFechaRecepcionSiiCompleted(this, new consultarFechaRecepcionSiiCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://ws.registroreclamodte.diii.sdi.sii.cl", ResponseNamespace="http://ws.registroreclamodte.diii.sdi.sii.cl", Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public string getVersion() {
            object[] results = this.Invoke("getVersion", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getVersionAsync() {
            this.getVersionAsync(null);
        }
        
        /// <remarks/>
        public void getVersionAsync(object userState) {
            if ((this.getVersionOperationCompleted == null)) {
                this.getVersionOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetVersionOperationCompleted);
            }
            this.InvokeAsync("getVersion", new object[0], this.getVersionOperationCompleted, userState);
        }
        
        private void OngetVersionOperationCompleted(object arg) {
            if ((this.getVersionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getVersionCompleted(this, new getVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://ws.registroreclamodte.diii.sdi.sii.cl", ResponseNamespace="http://ws.registroreclamodte.diii.sdi.sii.cl", Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public respuestaTo ingresarAceptacionReclamoDoc(string rutEmisor, string dvEmisor, string tipoDoc, string folio, string accionDoc) {
            object[] results = this.Invoke("ingresarAceptacionReclamoDoc", new object[] {
                        rutEmisor,
                        dvEmisor,
                        tipoDoc,
                        folio,
                        accionDoc});
            return ((respuestaTo)(results[0]));
        }
        
        /// <remarks/>
        public void ingresarAceptacionReclamoDocAsync(string rutEmisor, string dvEmisor, string tipoDoc, string folio, string accionDoc) {
            this.ingresarAceptacionReclamoDocAsync(rutEmisor, dvEmisor, tipoDoc, folio, accionDoc, null);
        }
        
        /// <remarks/>
        public void ingresarAceptacionReclamoDocAsync(string rutEmisor, string dvEmisor, string tipoDoc, string folio, string accionDoc, object userState) {
            if ((this.ingresarAceptacionReclamoDocOperationCompleted == null)) {
                this.ingresarAceptacionReclamoDocOperationCompleted = new System.Threading.SendOrPostCallback(this.OningresarAceptacionReclamoDocOperationCompleted);
            }
            this.InvokeAsync("ingresarAceptacionReclamoDoc", new object[] {
                        rutEmisor,
                        dvEmisor,
                        tipoDoc,
                        folio,
                        accionDoc}, this.ingresarAceptacionReclamoDocOperationCompleted, userState);
        }
        
        private void OningresarAceptacionReclamoDocOperationCompleted(object arg) {
            if ((this.ingresarAceptacionReclamoDocCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ingresarAceptacionReclamoDocCompleted(this, new ingresarAceptacionReclamoDocCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://ws.registroreclamodte.diii.sdi.sii.cl", ResponseNamespace="http://ws.registroreclamodte.diii.sdi.sii.cl", Use=System.Web.Services.Description.SoapBindingUse.Literal)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public respuestaTo listarEventosHistDoc(string rutEmisor, string dvEmisor, string tipoDoc, string folio) {
            object[] results = this.Invoke("listarEventosHistDoc", new object[] {
                        rutEmisor,
                        dvEmisor,
                        tipoDoc,
                        folio});
            return ((respuestaTo)(results[0]));
        }
        
        /// <remarks/>
        public void listarEventosHistDocAsync(string rutEmisor, string dvEmisor, string tipoDoc, string folio) {
            this.listarEventosHistDocAsync(rutEmisor, dvEmisor, tipoDoc, folio, null);
        }
        
        /// <remarks/>
        public void listarEventosHistDocAsync(string rutEmisor, string dvEmisor, string tipoDoc, string folio, object userState) {
            if ((this.listarEventosHistDocOperationCompleted == null)) {
                this.listarEventosHistDocOperationCompleted = new System.Threading.SendOrPostCallback(this.OnlistarEventosHistDocOperationCompleted);
            }
            this.InvokeAsync("listarEventosHistDoc", new object[] {
                        rutEmisor,
                        dvEmisor,
                        tipoDoc,
                        folio}, this.listarEventosHistDocOperationCompleted, userState);
        }
        
        private void OnlistarEventosHistDocOperationCompleted(object arg) {
            if ((this.listarEventosHistDocCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.listarEventosHistDocCompleted(this, new listarEventosHistDocCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.registroreclamodte.diii.sdi.sii.cl")]
    public partial class respuestaTo {
        
        private int codRespField;
        
        private bool codRespFieldSpecified;
        
        private string descRespField;
        
        private DteEventoDocTo[] listaEventosDocField;
        
        private string rutTokenField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int codResp {
            get {
                return this.codRespField;
            }
            set {
                this.codRespField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool codRespSpecified {
            get {
                return this.codRespFieldSpecified;
            }
            set {
                this.codRespFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string descResp {
            get {
                return this.descRespField;
            }
            set {
                this.descRespField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("listaEventosDoc", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public DteEventoDocTo[] listaEventosDoc {
            get {
                return this.listaEventosDocField;
            }
            set {
                this.listaEventosDocField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string rutToken {
            get {
                return this.rutTokenField;
            }
            set {
                this.rutTokenField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.registroreclamodte.diii.sdi.sii.cl")]
    public partial class DteEventoDocTo {
        
        private string codEventoField;
        
        private string descEventoField;
        
        private string rutResponsableField;
        
        private string dvResponsableField;
        
        private string fechaEventoField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string codEvento {
            get {
                return this.codEventoField;
            }
            set {
                this.codEventoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string descEvento {
            get {
                return this.descEventoField;
            }
            set {
                this.descEventoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string rutResponsable {
            get {
                return this.rutResponsableField;
            }
            set {
                this.rutResponsableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dvResponsable {
            get {
                return this.dvResponsableField;
            }
            set {
                this.dvResponsableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fechaEvento {
            get {
                return this.fechaEventoField;
            }
            set {
                this.fechaEventoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void consultarDocDteCedibleCompletedEventHandler(object sender, consultarDocDteCedibleCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class consultarDocDteCedibleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal consultarDocDteCedibleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public respuestaTo Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((respuestaTo)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void consultarFechaRecepcionSiiCompletedEventHandler(object sender, consultarFechaRecepcionSiiCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class consultarFechaRecepcionSiiCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal consultarFechaRecepcionSiiCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void getVersionCompletedEventHandler(object sender, getVersionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getVersionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getVersionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void ingresarAceptacionReclamoDocCompletedEventHandler(object sender, ingresarAceptacionReclamoDocCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ingresarAceptacionReclamoDocCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ingresarAceptacionReclamoDocCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public respuestaTo Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((respuestaTo)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void listarEventosHistDocCompletedEventHandler(object sender, listarEventosHistDocCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class listarEventosHistDocCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal listarEventosHistDocCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public respuestaTo Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((respuestaTo)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591