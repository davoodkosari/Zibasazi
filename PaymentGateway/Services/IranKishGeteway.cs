﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=4.6.81.0.
// 

public static class IranKishGeteway
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "MerchantBinding",
        Namespace = "http://tejarat/paymentGateway/definitions")]
    public partial class MerchantService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback verifyOperationCompleted;

        /// <remarks/>
        public MerchantService()
        {
            this.Url = "https://kica.shaparak.ir:443/epay/services";
        }

        /// <remarks/>
        public event verifyCompletedEventHandler verifyCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return:
            System.Xml.Serialization.XmlElementAttribute("verifyResponse",
                Namespace = "http://tejarat/paymentGateway/definitions")]
        public long verify(
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tejarat/paymentGateway/definitions")] verifyRequest verifyRequest)
        {
            object[] results = this.Invoke("verify", new object[]
            {
                verifyRequest
            });
            return ((long) (results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult Beginverify(verifyRequest verifyRequest, System.AsyncCallback callback,
            object asyncState)
        {
            return this.BeginInvoke("verify", new object[]
            {
                verifyRequest
            }, callback, asyncState);
        }

        /// <remarks/>
        public long Endverify(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((long) (results[0]));
        }

        /// <remarks/>
        public void verifyAsync(verifyRequest verifyRequest)
        {
            this.verifyAsync(verifyRequest, null);
        }

        /// <remarks/>
        public void verifyAsync(verifyRequest verifyRequest, object userState)
        {
            if ((this.verifyOperationCompleted == null))
            {
                this.verifyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnverifyOperationCompleted);
            }
            this.InvokeAsync("verify", new object[]
            {
                verifyRequest
            }, this.verifyOperationCompleted, userState);
        }

        private void OnverifyOperationCompleted(object arg)
        {
            if ((this.verifyCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs =
                    ((System.Web.Services.Protocols.InvokeCompletedEventArgs) (arg));
                this.verifyCompleted(this,
                    new verifyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled,
                        invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,
        Namespace = "http://tejarat/paymentGateway/definitions")]
    public partial class verifyRequest
    {

        private string merchantIdField;

        private string referenceNumberField;

        /// <remarks/>
        public string merchantId
        {
            get { return this.merchantIdField; }
            set { this.merchantIdField = value; }
        }

        /// <remarks/>
        public string referenceNumber
        {
            get { return this.referenceNumberField; }
            set { this.referenceNumberField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.81.0")]
    public delegate void verifyCompletedEventHandler(object sender, verifyCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class verifyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal verifyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public long Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((long) (this.results[0]));
            }
        }
    }

}
