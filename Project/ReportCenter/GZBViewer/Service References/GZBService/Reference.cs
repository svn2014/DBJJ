﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportCenter.GZBService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProcedureParameter", Namespace="http://schemas.datacontract.org/2004/07/DBWcfService")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ReportCenter.GZBService.ProcedureParameter[]))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ReportCenter.GZBService.ProcedureParameter.DBType))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(System.Data.ParameterDirection))]
    public partial class ProcedureParameter : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Data.ParameterDirection DirectionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int SizeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ReportCenter.GZBService.ProcedureParameter.DBType TypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private object ValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Data.ParameterDirection Direction {
            get {
                return this.DirectionField;
            }
            set {
                if ((this.DirectionField.Equals(value) != true)) {
                    this.DirectionField = value;
                    this.RaisePropertyChanged("Direction");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Size {
            get {
                return this.SizeField;
            }
            set {
                if ((this.SizeField.Equals(value) != true)) {
                    this.SizeField = value;
                    this.RaisePropertyChanged("Size");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ReportCenter.GZBService.ProcedureParameter.DBType Type {
            get {
                return this.TypeField;
            }
            set {
                if ((this.TypeField.Equals(value) != true)) {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public object Value {
            get {
                return this.ValueField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueField, value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
        [System.Runtime.Serialization.DataContractAttribute(Name="ProcedureParameter.DBType", Namespace="http://schemas.datacontract.org/2004/07/DBWcfService")]
        public enum DBType : int {
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Bit = 0,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Char = 1,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            VarChar = 2,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            NVarChar = 3,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            NClob = 4,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Float = 5,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Int = 6,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            BigInt = 7,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Date = 8,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            DateTime = 9,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Cursor = 10,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Unknown = 11,
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GZBService.IDBService")]
    public interface IDBService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDBService/ExecuteNonQuery", ReplyAction="http://tempuri.org/IDBService/ExecuteNonQueryResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ReportCenter.GZBService.ProcedureParameter[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ReportCenter.GZBService.ProcedureParameter))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ReportCenter.GZBService.ProcedureParameter.DBType))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Data.ParameterDirection))]
        object ExecuteNonQuery(string procedureName, ReportCenter.GZBService.ProcedureParameter[] procedureParameter, string outputParameterName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDBService/ExecuteStoredProcedure", ReplyAction="http://tempuri.org/IDBService/ExecuteStoredProcedureResponse")]
        System.Data.DataSet ExecuteStoredProcedure(string procedureName, ReportCenter.GZBService.ProcedureParameter[] procedureParameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDBService/GetWebConfigsByProject", ReplyAction="http://tempuri.org/IDBService/GetWebConfigsByProjectResponse")]
        System.Data.DataSet GetWebConfigsByProject(string project);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDBService/GetHomeSiteIP", ReplyAction="http://tempuri.org/IDBService/GetHomeSiteIPResponse")]
        string GetHomeSiteIP();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDBService/GetAlternativeSiteIP", ReplyAction="http://tempuri.org/IDBService/GetAlternativeSiteIPResponse")]
        string GetAlternativeSiteIP();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDBServiceChannel : ReportCenter.GZBService.IDBService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DBServiceClient : System.ServiceModel.ClientBase<ReportCenter.GZBService.IDBService>, ReportCenter.GZBService.IDBService {
        
        public DBServiceClient() {
        }
        
        public DBServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DBServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DBServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DBServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public object ExecuteNonQuery(string procedureName, ReportCenter.GZBService.ProcedureParameter[] procedureParameter, string outputParameterName) {
            return base.Channel.ExecuteNonQuery(procedureName, procedureParameter, outputParameterName);
        }
        
        public System.Data.DataSet ExecuteStoredProcedure(string procedureName, ReportCenter.GZBService.ProcedureParameter[] procedureParameter) {
            return base.Channel.ExecuteStoredProcedure(procedureName, procedureParameter);
        }
        
        public System.Data.DataSet GetWebConfigsByProject(string project) {
            return base.Channel.GetWebConfigsByProject(project);
        }
        
        public string GetHomeSiteIP() {
            return base.Channel.GetHomeSiteIP();
        }
        
        public string GetAlternativeSiteIP() {
            return base.Channel.GetAlternativeSiteIP();
        }
    }
}