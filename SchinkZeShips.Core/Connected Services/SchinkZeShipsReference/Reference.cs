﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchinkZeShips.Core.SchinkZeShipsReference {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Game", Namespace="http://schemas.datacontract.org/2004/07/SchinkZeShips.Server")]
    public partial class Game : object, System.ComponentModel.INotifyPropertyChanged {
        
        private SchinkZeShips.Core.SchinkZeShipsReference.Player GameCreatorField;
        
        private SchinkZeShips.Core.SchinkZeShipsReference.Player GameParticipantField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public SchinkZeShips.Core.SchinkZeShipsReference.Player GameCreator {
            get {
                return this.GameCreatorField;
            }
            set {
                if ((object.ReferenceEquals(this.GameCreatorField, value) != true)) {
                    this.GameCreatorField = value;
                    this.RaisePropertyChanged("GameCreator");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public SchinkZeShips.Core.SchinkZeShipsReference.Player GameParticipant {
            get {
                return this.GameParticipantField;
            }
            set {
                if ((object.ReferenceEquals(this.GameParticipantField, value) != true)) {
                    this.GameParticipantField = value;
                    this.RaisePropertyChanged("GameParticipant");
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
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Player", Namespace="http://schemas.datacontract.org/2004/07/SchinkZeShips.Server")]
    public partial class Player : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string UsernameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Username {
            get {
                return this.UsernameField;
            }
            set {
                if ((object.ReferenceEquals(this.UsernameField, value) != true)) {
                    this.UsernameField = value;
                    this.RaisePropertyChanged("Username");
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
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SchinkZeShipsReference.ISchinkZeShips")]
    public interface ISchinkZeShips {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISchinkZeShips/GetAllGames", ReplyAction="http://tempuri.org/ISchinkZeShips/GetAllGamesResponse")]
        System.IAsyncResult BeginGetAllGames(System.AsyncCallback callback, object asyncState);
        
        System.Collections.Generic.List<SchinkZeShips.Core.SchinkZeShipsReference.Game> EndGetAllGames(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISchinkZeShips/CreateGame", ReplyAction="http://tempuri.org/ISchinkZeShips/CreateGameResponse")]
        System.IAsyncResult BeginCreateGame(SchinkZeShips.Core.SchinkZeShipsReference.Player creator, System.AsyncCallback callback, object asyncState);
        
        SchinkZeShips.Core.SchinkZeShipsReference.Game EndCreateGame(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISchinkZeShips/JoinGame", ReplyAction="http://tempuri.org/ISchinkZeShips/JoinGameResponse")]
        System.IAsyncResult BeginJoinGame(SchinkZeShips.Core.SchinkZeShipsReference.Game gameToJoin, SchinkZeShips.Core.SchinkZeShipsReference.Player participant, System.AsyncCallback callback, object asyncState);
        
        void EndJoinGame(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISchinkZeShipsChannel : SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetAllGamesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetAllGamesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.Generic.List<SchinkZeShips.Core.SchinkZeShipsReference.Game> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.Generic.List<SchinkZeShips.Core.SchinkZeShipsReference.Game>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CreateGameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public CreateGameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public SchinkZeShips.Core.SchinkZeShipsReference.Game Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((SchinkZeShips.Core.SchinkZeShipsReference.Game)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SchinkZeShipsClient : System.ServiceModel.ClientBase<SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips>, SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips {
        
        private BeginOperationDelegate onBeginGetAllGamesDelegate;
        
        private EndOperationDelegate onEndGetAllGamesDelegate;
        
        private System.Threading.SendOrPostCallback onGetAllGamesCompletedDelegate;
        
        private BeginOperationDelegate onBeginCreateGameDelegate;
        
        private EndOperationDelegate onEndCreateGameDelegate;
        
        private System.Threading.SendOrPostCallback onCreateGameCompletedDelegate;
        
        private BeginOperationDelegate onBeginJoinGameDelegate;
        
        private EndOperationDelegate onEndJoinGameDelegate;
        
        private System.Threading.SendOrPostCallback onJoinGameCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public SchinkZeShipsClient(EndpointConfiguration endpointConfiguration) : 
                base(SchinkZeShipsClient.GetBindingForEndpoint(endpointConfiguration), SchinkZeShipsClient.GetEndpointAddress(endpointConfiguration)) {
        }
        
        public SchinkZeShipsClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(SchinkZeShipsClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress)) {
        }
        
        public SchinkZeShipsClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(SchinkZeShipsClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress) {
        }
        
        public SchinkZeShipsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetAllGamesCompletedEventArgs> GetAllGamesCompleted;
        
        public event System.EventHandler<CreateGameCompletedEventArgs> CreateGameCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> JoinGameCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips.BeginGetAllGames(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetAllGames(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.Generic.List<SchinkZeShips.Core.SchinkZeShipsReference.Game> SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips.EndGetAllGames(System.IAsyncResult result) {
            return base.Channel.EndGetAllGames(result);
        }
        
        private System.IAsyncResult OnBeginGetAllGames(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips)(this)).BeginGetAllGames(callback, asyncState);
        }
        
        private object[] OnEndGetAllGames(System.IAsyncResult result) {
            System.Collections.Generic.List<SchinkZeShips.Core.SchinkZeShipsReference.Game> retVal = ((SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips)(this)).EndGetAllGames(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetAllGamesCompleted(object state) {
            if ((this.GetAllGamesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetAllGamesCompleted(this, new GetAllGamesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetAllGamesAsync() {
            this.GetAllGamesAsync(null);
        }
        
        public void GetAllGamesAsync(object userState) {
            if ((this.onBeginGetAllGamesDelegate == null)) {
                this.onBeginGetAllGamesDelegate = new BeginOperationDelegate(this.OnBeginGetAllGames);
            }
            if ((this.onEndGetAllGamesDelegate == null)) {
                this.onEndGetAllGamesDelegate = new EndOperationDelegate(this.OnEndGetAllGames);
            }
            if ((this.onGetAllGamesCompletedDelegate == null)) {
                this.onGetAllGamesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetAllGamesCompleted);
            }
            base.InvokeAsync(this.onBeginGetAllGamesDelegate, null, this.onEndGetAllGamesDelegate, this.onGetAllGamesCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips.BeginCreateGame(SchinkZeShips.Core.SchinkZeShipsReference.Player creator, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginCreateGame(creator, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SchinkZeShips.Core.SchinkZeShipsReference.Game SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips.EndCreateGame(System.IAsyncResult result) {
            return base.Channel.EndCreateGame(result);
        }
        
        private System.IAsyncResult OnBeginCreateGame(object[] inValues, System.AsyncCallback callback, object asyncState) {
            SchinkZeShips.Core.SchinkZeShipsReference.Player creator = ((SchinkZeShips.Core.SchinkZeShipsReference.Player)(inValues[0]));
            return ((SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips)(this)).BeginCreateGame(creator, callback, asyncState);
        }
        
        private object[] OnEndCreateGame(System.IAsyncResult result) {
            SchinkZeShips.Core.SchinkZeShipsReference.Game retVal = ((SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips)(this)).EndCreateGame(result);
            return new object[] {
                    retVal};
        }
        
        private void OnCreateGameCompleted(object state) {
            if ((this.CreateGameCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CreateGameCompleted(this, new CreateGameCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CreateGameAsync(SchinkZeShips.Core.SchinkZeShipsReference.Player creator) {
            this.CreateGameAsync(creator, null);
        }
        
        public void CreateGameAsync(SchinkZeShips.Core.SchinkZeShipsReference.Player creator, object userState) {
            if ((this.onBeginCreateGameDelegate == null)) {
                this.onBeginCreateGameDelegate = new BeginOperationDelegate(this.OnBeginCreateGame);
            }
            if ((this.onEndCreateGameDelegate == null)) {
                this.onEndCreateGameDelegate = new EndOperationDelegate(this.OnEndCreateGame);
            }
            if ((this.onCreateGameCompletedDelegate == null)) {
                this.onCreateGameCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCreateGameCompleted);
            }
            base.InvokeAsync(this.onBeginCreateGameDelegate, new object[] {
                        creator}, this.onEndCreateGameDelegate, this.onCreateGameCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips.BeginJoinGame(SchinkZeShips.Core.SchinkZeShipsReference.Game gameToJoin, SchinkZeShips.Core.SchinkZeShipsReference.Player participant, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginJoinGame(gameToJoin, participant, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips.EndJoinGame(System.IAsyncResult result) {
            base.Channel.EndJoinGame(result);
        }
        
        private System.IAsyncResult OnBeginJoinGame(object[] inValues, System.AsyncCallback callback, object asyncState) {
            SchinkZeShips.Core.SchinkZeShipsReference.Game gameToJoin = ((SchinkZeShips.Core.SchinkZeShipsReference.Game)(inValues[0]));
            SchinkZeShips.Core.SchinkZeShipsReference.Player participant = ((SchinkZeShips.Core.SchinkZeShipsReference.Player)(inValues[1]));
            return ((SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips)(this)).BeginJoinGame(gameToJoin, participant, callback, asyncState);
        }
        
        private object[] OnEndJoinGame(System.IAsyncResult result) {
            ((SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips)(this)).EndJoinGame(result);
            return null;
        }
        
        private void OnJoinGameCompleted(object state) {
            if ((this.JoinGameCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.JoinGameCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void JoinGameAsync(SchinkZeShips.Core.SchinkZeShipsReference.Game gameToJoin, SchinkZeShips.Core.SchinkZeShipsReference.Player participant) {
            this.JoinGameAsync(gameToJoin, participant, null);
        }
        
        public void JoinGameAsync(SchinkZeShips.Core.SchinkZeShipsReference.Game gameToJoin, SchinkZeShips.Core.SchinkZeShipsReference.Player participant, object userState) {
            if ((this.onBeginJoinGameDelegate == null)) {
                this.onBeginJoinGameDelegate = new BeginOperationDelegate(this.OnBeginJoinGame);
            }
            if ((this.onEndJoinGameDelegate == null)) {
                this.onEndJoinGameDelegate = new EndOperationDelegate(this.OnEndJoinGame);
            }
            if ((this.onJoinGameCompletedDelegate == null)) {
                this.onJoinGameCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnJoinGameCompleted);
            }
            base.InvokeAsync(this.onBeginJoinGameDelegate, new object[] {
                        gameToJoin,
                        participant}, this.onEndJoinGameDelegate, this.onJoinGameCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips CreateChannel() {
            return new SchinkZeShipsClientChannel(this);
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_ISchinkZeShips)) {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.MaxReceivedMessageSize = int.MaxValue;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpsBinding_ISchinkZeShips)) {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_ISchinkZeShips)) {
                return new System.ServiceModel.EndpointAddress("http://schinkzeships.azurewebsites.net/SchinkZeShips.svc");
            }
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpsBinding_ISchinkZeShips)) {
                return new System.ServiceModel.EndpointAddress("https://schinkzeships.azurewebsites.net/SchinkZeShips.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private class SchinkZeShipsClientChannel : ChannelBase<SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips>, SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips {
            
            public SchinkZeShipsClientChannel(System.ServiceModel.ClientBase<SchinkZeShips.Core.SchinkZeShipsReference.ISchinkZeShips> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetAllGames(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("GetAllGames", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.Generic.List<SchinkZeShips.Core.SchinkZeShipsReference.Game> EndGetAllGames(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.Generic.List<SchinkZeShips.Core.SchinkZeShipsReference.Game> _result = ((System.Collections.Generic.List<SchinkZeShips.Core.SchinkZeShipsReference.Game>)(base.EndInvoke("GetAllGames", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginCreateGame(SchinkZeShips.Core.SchinkZeShipsReference.Player creator, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = creator;
                System.IAsyncResult _result = base.BeginInvoke("CreateGame", _args, callback, asyncState);
                return _result;
            }
            
            public SchinkZeShips.Core.SchinkZeShipsReference.Game EndCreateGame(System.IAsyncResult result) {
                object[] _args = new object[0];
                SchinkZeShips.Core.SchinkZeShipsReference.Game _result = ((SchinkZeShips.Core.SchinkZeShipsReference.Game)(base.EndInvoke("CreateGame", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginJoinGame(SchinkZeShips.Core.SchinkZeShipsReference.Game gameToJoin, SchinkZeShips.Core.SchinkZeShipsReference.Player participant, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = gameToJoin;
                _args[1] = participant;
                System.IAsyncResult _result = base.BeginInvoke("JoinGame", _args, callback, asyncState);
                return _result;
            }
            
            public void EndJoinGame(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("JoinGame", _args, result);
            }
        }
        
        public enum EndpointConfiguration {
            
            BasicHttpBinding_ISchinkZeShips,
            
            BasicHttpsBinding_ISchinkZeShips,
        }
    }
}
