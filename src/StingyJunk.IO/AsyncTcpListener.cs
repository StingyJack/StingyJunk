namespace StingyJunk.IO
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     A Task async pattern tcp socket listener  
    /// </summary>
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class AsyncTcpListener
    {
        #region "fields, events, and props"

        public const int SUCCESS = 0;
        public const int ERROR = -1;

        private TcpListener _tcpListener;

        /// <summary>
        ///     Subscribe to receive diagnostic messages
        /// </summary>
        public event EventHandler<DiagMessageEventArgs> DiagMessage;

        public event EventHandler<InfoMessageEventArgs> InfoMessage;

        /// <summary>
        ///     Subscribe to handle replies
        /// </summary>
        public event EventHandler<MessageRequestResponseEventArgs> ResponseHandler;

        /// <summary>
        ///     Giff naem for logging/diagnostic
        /// </summary>
        public string Name { get; set; }

        public int Port { get; set; }
        
        public IPAddress[] IpAddresses { get; set; }
        public int Timeout { get; set; } = 1000;
        private int _clientCounter;

        #endregion //#region "fields, events, and props"

        #region "ctor"

        /// <summary>
        ///     Creates a new listener
        /// </summary>
        /// <param name="name">Friendly name to use for diagnostics, etc</param>
        /// <param name="port">the TCP port</param>
        /// <param name="requestedIpAddress">
        ///     A specific IP to use. If not provided, the first InterNetwork 
        ///     address that is returned from Dns.GetHostEntry is used.
        /// </param>
        public AsyncTcpListener(string name, int port, IPAddress requestedIpAddress = null)
        {
            Name = name;
            Port = port;
            var hostName = Dns.GetHostName();
            var ipHostInfo = Dns.GetHostEntry(hostName);

            if (requestedIpAddress != null)
            {
                IpAddresses = new [] {requestedIpAddress};
            }
            else
            {
                IpAddresses = ipHostInfo.AddressList.Where(i => i.AddressFamily == AddressFamily.InterNetwork).ToArray();
            }

            if (IpAddresses == null || IpAddresses.Length == 0)
            {
                throw new Exception("No IP address for server");
            }
        }

        #endregion //#region "ctor"

        #region "control"

        public int Run()
        {
            var cts = new CancellationTokenSource();
            var tasks = new List<Task>();
            Parallel.ForEach(IpAddresses, i =>
            {
                tasks.Add(SpawnListenerAsync(cts, i, Port));
            });

            Task.WaitAll(tasks.ToArray());

            return SUCCESS;
        }

        private async Task SpawnListenerAsync(CancellationTokenSource cts, IPAddress ipAddress, int port)
        {
            _tcpListener = new TcpListener(ipAddress, port);
            _tcpListener.Start();

            Im($"{nameof(AsyncTcpListener)} {Name} is now running on {ipAddress}:{port}");


            //just fire and forget. We break from the "forgotten" async loops
            //in AcceptClientsAsync using a CancellationToken from `cts`

            try
            {
                await AcceptClientsAsync(_tcpListener, cts.Token);
            }
            catch (Exception ex)
            {
                Dm(ex.Message);
                throw;
            }
        }

        public void Stop()
        {
            _tcpListener?.Stop();
        }

        #endregion //#region "control"

        #region "worker methods"

        private async Task AcceptClientsAsync(TcpListener listener, CancellationToken ct)
        {
            _clientCounter = 0;
            while (!ct.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync()
                    .ConfigureAwait(false);
                var clientId = Interlocked.Increment(ref _clientCounter);

                //once again, just fire and forget, and use the CancellationToken
                //to signal to the "forgotten" async invocation.
                // ^ not sure why one is "forgotten"

#pragma warning disable 4014
                //ProcessAsync(client, clientId, ct).ConfigureAwait(false);
                ProcessAsync(client, clientId, ct).ConfigureAwait(false);
                //Task.Run(async () => await ProcessAsync2(client, clientId, ct), ct);
#pragma warning restore 4014
            }
        }

        #region "former implementation"

        /*
        private async Task ProcessAsync(TcpClient tcpClient, int clientId,
            CancellationToken ct)
        {
            var clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
            Dm($"ClientId {clientId} requested connection from {clientEndPoint}");

            using (tcpClient)
            {
                var networkStream = tcpClient.GetStream();
                var reader = new StreamReader(networkStream);
                var writer = new StreamWriter(networkStream) { AutoFlush = true };
                while (!ct.IsCancellationRequested)
                {
                    // if (stream.CanTimeout) {stream.ReadTimeout = Timeout;} probably also OK here.
                    //  but the completedTask lets whichever comes due first "win".
                    var timeoutTask = Task.Delay(TimeSpan.FromSeconds(Timeout), ct);
                    //var amountReadTask = stream.ReadAsync(buf, 0, buf.Length, ct);
                    var amountReadTask = reader.ReadLineAsync();

                    var completedTask = await Task.WhenAny(timeoutTask, amountReadTask).ConfigureAwait(false);
                    if (completedTask == timeoutTask)
                    {
                        Dm($"\t ClientId {clientId} timed out waiting for data");
                        break;
                    }
                    //now we know that the amountTask is complete so
                    //we can ask for its Result without blocking
                    var dataFromClient = amountReadTask.Result;
                    if (string.IsNullOrWhiteSpace(dataFromClient))
                    {
                        Dm($"\t ClientId {clientId} sent no value.");
                        break;
                    }

                    Dm($"\t ClientId {clientId} received data from client: {dataFromClient}");
                    var response = Respond(dataFromClient);
                    Dm($"\t ClientId {clientId} responding with {response}");
                    await writer.WriteLineAsync(response).ConfigureAwait(false);
                    writer.Flush();
                }
            }
            Dm($"\t ClientId {clientId} disconnected");
        }
        */

        #endregion //#region "former implementation"

        private async Task ProcessAsync(TcpClient tcpClient, int clientId,
            CancellationToken ct)
        {
            var clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
            Dm($"ClientId {clientId} requested connection from {clientEndPoint}");

            var networkStream = tcpClient.GetStream();
            if (networkStream.CanTimeout && networkStream.ReadTimeout > Timeout)
            {
                networkStream.ReadTimeout = Timeout;
            }

            var result = new StringBuilder();

            var reader = new StreamReader(networkStream);
            var writer = new StreamWriter(networkStream) {AutoFlush = true};
            try
            {
                while (reader.Peek() >= 0)
                {
                    if (ct.IsCancellationRequested == true)
                    {
                        break;
                    }
                    var buffer = new char[1];
                    await reader.ReadAsync(buffer, 0, 1);
                    result.Append(buffer);
                }

                if (result.Length == 0)
                {
                    Dm($"\t ClientId {clientId} sent no value.");
                    return;
                }

                var dataFromClient = result.ToString();
                Dm($"\t ClientId {clientId} received data from client: {dataFromClient}");
                var response = Respond(dataFromClient);

                Dm($"\t ClientId {clientId} responding with {response}");
                await writer.WriteLineAsync(response).ConfigureAwait(false);
                writer.Flush();
            }
            catch (Exception e)
            {
                Dm($"\t ClientId {clientId} timed out waiting for data");

                Dm(e.Message);
                throw;
            }
            finally
            {
                reader.Dispose();
                writer.Dispose();
            }

            Dm($"\t ClientId {clientId} disconnected");
        }

        private string Respond(string request)
        {
            var rea = new MessageRequestResponseEventArgs {RequestMessage = request};
            OnResponseHandler(rea);
            return rea.ResponseMessage;
        }

        #endregion //#region "worker methods"

        #region  "events and utils"

        protected virtual void OnDiagMessage(DiagMessageEventArgs e)
        {
            DiagMessage?.Invoke(this, e);
        }

        protected virtual void OnInfoMessage(InfoMessageEventArgs e)
        {
            InfoMessage?.Invoke(this, e);
        }

        protected virtual void OnResponseHandler(MessageRequestResponseEventArgs e)
        {
            ResponseHandler?.Invoke(this, e);
        }

        private void Im(string message, [CallerMemberName] string methodName = null)
        {
            OnInfoMessage(new InfoMessageEventArgs {InfoMessage = message, Timestamp = DateTime.Now, SourceName = methodName});
        }


        private void Dm(string message, [CallerMemberName] string methodName = null)
        {
            OnDiagMessage(new DiagMessageEventArgs {DiagnosticMessage = message, Timestamp = DateTime.Now, SourceName = methodName});
        }

        #endregion //#region  "events and utils"
    }
}