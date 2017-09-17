﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1008
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.1008.
// 
#pragma warning disable 1591

namespace ChexarIO.Communication.CallCenterService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="CallCenterServiceSoap", Namespace="http://tempuri.org/")]
    public partial class CallCenterService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CompanyLoginOperationCompleted;
        
        private System.Threading.SendOrPostCallback ComposeMessageOperationCompleted;
        
        private System.Threading.SendOrPostCallback CheckForMessageOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConfirmMessageOperationCompleted;
        
        private System.Threading.SendOrPostCallback TicketStatusOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CallCenterService() {
            this.Url = global::ChexarIO.Communication.Properties.Settings.Default.ChexarIO_Communication_CallCenterService_CallCenterService;
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
        public event CompanyLoginCompletedEventHandler CompanyLoginCompleted;
        
        /// <remarks/>
        public event ComposeMessageCompletedEventHandler ComposeMessageCompleted;
        
        /// <remarks/>
        public event CheckForMessageCompletedEventHandler CheckForMessageCompleted;
        
        /// <remarks/>
        public event ConfirmMessageCompletedEventHandler ConfirmMessageCompleted;
        
        /// <remarks/>
        public event TicketStatusCompletedEventHandler TicketStatusCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CompanyLogin", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CompanyLogin(string CompanyId, string Username, string Password) {
            object[] results = this.Invoke("CompanyLogin", new object[] {
                        CompanyId,
                        Username,
                        Password});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CompanyLoginAsync(string CompanyId, string Username, string Password) {
            this.CompanyLoginAsync(CompanyId, Username, Password, null);
        }
        
        /// <remarks/>
        public void CompanyLoginAsync(string CompanyId, string Username, string Password, object userState) {
            if ((this.CompanyLoginOperationCompleted == null)) {
                this.CompanyLoginOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCompanyLoginOperationCompleted);
            }
            this.InvokeAsync("CompanyLogin", new object[] {
                        CompanyId,
                        Username,
                        Password}, this.CompanyLoginOperationCompleted, userState);
        }
        
        private void OnCompanyLoginOperationCompleted(object arg) {
            if ((this.CompanyLoginCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CompanyLoginCompleted(this, new CompanyLoginCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ComposeMessage", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ComposeMessage(string token, int TicketNo, int EmployeeId, string Message) {
            object[] results = this.Invoke("ComposeMessage", new object[] {
                        token,
                        TicketNo,
                        EmployeeId,
                        Message});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ComposeMessageAsync(string token, int TicketNo, int EmployeeId, string Message) {
            this.ComposeMessageAsync(token, TicketNo, EmployeeId, Message, null);
        }
        
        /// <remarks/>
        public void ComposeMessageAsync(string token, int TicketNo, int EmployeeId, string Message, object userState) {
            if ((this.ComposeMessageOperationCompleted == null)) {
                this.ComposeMessageOperationCompleted = new System.Threading.SendOrPostCallback(this.OnComposeMessageOperationCompleted);
            }
            this.InvokeAsync("ComposeMessage", new object[] {
                        token,
                        TicketNo,
                        EmployeeId,
                        Message}, this.ComposeMessageOperationCompleted, userState);
        }
        
        private void OnComposeMessageOperationCompleted(object arg) {
            if ((this.ComposeMessageCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ComposeMessageCompleted(this, new ComposeMessageCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CheckForMessage", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CheckForMessage(string token, int TicketNo) {
            object[] results = this.Invoke("CheckForMessage", new object[] {
                        token,
                        TicketNo});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CheckForMessageAsync(string token, int TicketNo) {
            this.CheckForMessageAsync(token, TicketNo, null);
        }
        
        /// <remarks/>
        public void CheckForMessageAsync(string token, int TicketNo, object userState) {
            if ((this.CheckForMessageOperationCompleted == null)) {
                this.CheckForMessageOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCheckForMessageOperationCompleted);
            }
            this.InvokeAsync("CheckForMessage", new object[] {
                        token,
                        TicketNo}, this.CheckForMessageOperationCompleted, userState);
        }
        
        private void OnCheckForMessageOperationCompleted(object arg) {
            if ((this.CheckForMessageCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CheckForMessageCompleted(this, new CheckForMessageCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ConfirmMessage", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ConfirmMessage(string token, int MessageId) {
            object[] results = this.Invoke("ConfirmMessage", new object[] {
                        token,
                        MessageId});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ConfirmMessageAsync(string token, int MessageId) {
            this.ConfirmMessageAsync(token, MessageId, null);
        }
        
        /// <remarks/>
        public void ConfirmMessageAsync(string token, int MessageId, object userState) {
            if ((this.ConfirmMessageOperationCompleted == null)) {
                this.ConfirmMessageOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConfirmMessageOperationCompleted);
            }
            this.InvokeAsync("ConfirmMessage", new object[] {
                        token,
                        MessageId}, this.ConfirmMessageOperationCompleted, userState);
        }
        
        private void OnConfirmMessageOperationCompleted(object arg) {
            if ((this.ConfirmMessageCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConfirmMessageCompleted(this, new ConfirmMessageCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/TicketStatus", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string TicketStatus(string token, int TicketNo) {
            object[] results = this.Invoke("TicketStatus", new object[] {
                        token,
                        TicketNo});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void TicketStatusAsync(string token, int TicketNo) {
            this.TicketStatusAsync(token, TicketNo, null);
        }
        
        /// <remarks/>
        public void TicketStatusAsync(string token, int TicketNo, object userState) {
            if ((this.TicketStatusOperationCompleted == null)) {
                this.TicketStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTicketStatusOperationCompleted);
            }
            this.InvokeAsync("TicketStatus", new object[] {
                        token,
                        TicketNo}, this.TicketStatusOperationCompleted, userState);
        }
        
        private void OnTicketStatusOperationCompleted(object arg) {
            if ((this.TicketStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TicketStatusCompleted(this, new TicketStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void CompanyLoginCompletedEventHandler(object sender, CompanyLoginCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CompanyLoginCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CompanyLoginCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ComposeMessageCompletedEventHandler(object sender, ComposeMessageCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ComposeMessageCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ComposeMessageCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void CheckForMessageCompletedEventHandler(object sender, CheckForMessageCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CheckForMessageCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CheckForMessageCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ConfirmMessageCompletedEventHandler(object sender, ConfirmMessageCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConfirmMessageCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConfirmMessageCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void TicketStatusCompletedEventHandler(object sender, TicketStatusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TicketStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal TicketStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
}

#pragma warning restore 1591