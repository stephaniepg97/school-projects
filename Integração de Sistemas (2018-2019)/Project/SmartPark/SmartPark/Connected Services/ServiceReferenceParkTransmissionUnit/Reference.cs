﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParkSS.ServiceReferenceParkTransmissionUnit {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReferenceParkTransmissionUnit.IServiceParkTransmissionUnit")]
    public interface IServiceParkTransmissionUnit {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParkTransmissionUnit/LoadConfigFile", ReplyAction="http://tempuri.org/IServiceParkTransmissionUnit/LoadConfigFileResponse")]
        bool LoadConfigFile(string configFileXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParkTransmissionUnit/LoadConfigFile", ReplyAction="http://tempuri.org/IServiceParkTransmissionUnit/LoadConfigFileResponse")]
        System.Threading.Tasks.Task<bool> LoadConfigFileAsync(string configFileXML);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParkTransmissionUnit/GetParks", ReplyAction="http://tempuri.org/IServiceParkTransmissionUnit/GetParksResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        object[] GetParks();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParkTransmissionUnit/GetParks", ReplyAction="http://tempuri.org/IServiceParkTransmissionUnit/GetParksResponse")]
        System.Threading.Tasks.Task<object[]> GetParksAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParkTransmissionUnit/GetParkingSpots", ReplyAction="http://tempuri.org/IServiceParkTransmissionUnit/GetParkingSpotsResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        object[] GetParkingSpots();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParkTransmissionUnit/GetParkingSpots", ReplyAction="http://tempuri.org/IServiceParkTransmissionUnit/GetParkingSpotsResponse")]
        System.Threading.Tasks.Task<object[]> GetParkingSpotsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParkTransmissionUnit/GetProviders", ReplyAction="http://tempuri.org/IServiceParkTransmissionUnit/GetProvidersResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        object[] GetProviders();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParkTransmissionUnit/GetProviders", ReplyAction="http://tempuri.org/IServiceParkTransmissionUnit/GetProvidersResponse")]
        System.Threading.Tasks.Task<object[]> GetProvidersAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceParkTransmissionUnitChannel : ParkSS.ServiceReferenceParkTransmissionUnit.IServiceParkTransmissionUnit, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceParkTransmissionUnitClient : System.ServiceModel.ClientBase<ParkSS.ServiceReferenceParkTransmissionUnit.IServiceParkTransmissionUnit>, ParkSS.ServiceReferenceParkTransmissionUnit.IServiceParkTransmissionUnit {
        
        public ServiceParkTransmissionUnitClient() {
        }
        
        public ServiceParkTransmissionUnitClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceParkTransmissionUnitClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceParkTransmissionUnitClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceParkTransmissionUnitClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool LoadConfigFile(string configFileXML) {
            return base.Channel.LoadConfigFile(configFileXML);
        }
        
        public System.Threading.Tasks.Task<bool> LoadConfigFileAsync(string configFileXML) {
            return base.Channel.LoadConfigFileAsync(configFileXML);
        }
        
        public object[] GetParks() {
            return base.Channel.GetParks();
        }
        
        public System.Threading.Tasks.Task<object[]> GetParksAsync() {
            return base.Channel.GetParksAsync();
        }
        
        public object[] GetParkingSpots() {
            return base.Channel.GetParkingSpots();
        }
        
        public System.Threading.Tasks.Task<object[]> GetParkingSpotsAsync() {
            return base.Channel.GetParkingSpotsAsync();
        }
        
        public object[] GetProviders() {
            return base.Channel.GetProviders();
        }
        
        public System.Threading.Tasks.Task<object[]> GetProvidersAsync() {
            return base.Channel.GetProvidersAsync();
        }
    }
}
