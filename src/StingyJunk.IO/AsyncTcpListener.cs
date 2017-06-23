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
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class AsyncTcpListener
    {
        #region "fields, events, and props"

        public const int SUCCESS = 0;
        public const int ERROR = -1;

        // ReSharper disable CollectionNeverQueried.Local
        private readonly List<TcpListener> _tcpListeners = new List<TcpListener>();
        // ReSharper restore CollectionNeverQueried.Local

        /// <summary>
        ///     Giff naem for logging/diagnostic
        /// </summary>
        public string Name { get; set; }

        public int Port { get; set; }

        public IPAddress[] IpAddresses { get; set; }
        public int Timeout { get; set; } = 1000;
        private int _totalClientCount;
        private int _activeClientCount;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        //private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        //private TaskScheduler _taskScheduler = TaskScheduler.Current;

        #endregion //#region "fields, events, and props"

        #region "ctor"

        /// <summary>
        ///     Creates a new listener
        /// </summary>
        /// <param name="name">Friendly name to use for diagnostics, etc</param>
        /// <param name="port">the TCP port</param>
        /// <param name="requestedIpAddress">
        ///     A specific IP to use. If not provided, all the InterNetwork 
        ///     addresses that are returned from Dns.GetHostEntry are used.
        /// </param>
        public AsyncTcpListener(string name, int port, IPAddress requestedIpAddress = null)
        {
            Name = name;
            Port = port;
            var hostName = Dns.GetHostName();
            var ipHostInfo = Dns.GetHostEntry(hostName);

            if (requestedIpAddress != null)
            {
                IpAddresses = new[] {requestedIpAddress};
            }
            else
            {
                IpAddresses = ipHostInfo.AddressList.Where(i => i.AddressFamily == AddressFamily.InterNetwork).ToArray();
            }

            if (IpAddresses == null || IpAddresses.Length == 0)
            {
                throw new InvalidOperationException("No IP address for server");
            }
        }

        #endregion //#region "ctor"

        #region "control"

        public int Run()
        {
            var returnCode = ERROR;

            var tasks = new List<Task>();
            try
            {
                Parallel.ForEach(IpAddresses, i => { tasks.Add(SpawnListenerAsync(_cancellationTokenSource, i, Port)); });
                Task.WaitAll(tasks.ToArray(), _cancellationTokenSource.Token);
                returnCode = SUCCESS;
            }
            catch (OperationCanceledException)
            {
                //do nothing
            }
            catch (Exception e)
            {
                Error($"General Failure", e);
            }
            finally
            {
                if (_cancellationTokenSource.IsCancellationRequested == false)
                {
                    Stop();
                }
            }

            return returnCode;
        }

        private async Task SpawnListenerAsync(CancellationTokenSource cts, IPAddress ipAddress, int port)
        {
            var tcpListener = new TcpListener(ipAddress, port);
            _tcpListeners.Add(tcpListener);
            tcpListener.Start();

            Info($"{nameof(AsyncTcpListener)} {Name} is now listening on {ipAddress}:{port}");


            //just fire and forget. We break from the "forgotten" async loops
            //in AcceptClientsAsync using a CancellationToken from `cts`

            try
            {
                await AcceptClientsAsync(tcpListener, cts.Token);
            }
            catch (Exception ex)
            {
                Error(ex.Message, ex);
                throw;
            }
        }

        public void Stop()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                Info("Stop requested, cancellation already in progress");
            }
            else
            {
                Info("Stop requested, setting cancellation");
                _cancellationTokenSource.Cancel();
            }
        }

        #endregion //#region "control"

        #region "worker methods"

        private async Task AcceptClientsAsync(TcpListener listener, CancellationToken ct)
        {
            _totalClientCount = 0;
            while (!ct.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync().ConfigureAwait(true);
                var clientId = Interlocked.Increment(ref _totalClientCount);
                Interlocked.Increment(ref _activeClientCount);
                NotifyForConnStatChanges();

                //once again, just fire and forget, and use the CancellationToken
                //to signal to the "forgotten" async invocation.
                // ^ not sure why one is "forgotten"

#pragma warning disable 4014
                //ProcessAsync(client, clientId, ct).ConfigureAwait(false);
                ProcessAsync(client, clientId, ct).ConfigureAwait(true);
                //Task.Run(async () => await ProcessAsync2(client, clientId, ct), ct);
#pragma warning restore 4014
            }
        }

        private async Task ProcessAsync(TcpClient tcpClient, int clientId,
            CancellationToken ct)
        {
            var clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
            Debug($"ClientId {clientId} requested connection from {clientEndPoint}");

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
                    Debug($"\t ClientId {clientId} sent no value.");
                    return;
                }

                var dataFromClient = result.ToString();
                Debug($"\t ClientId {clientId} received data from client: {dataFromClient}");
                var response = Respond(dataFromClient);

                Debug($"\t ClientId {clientId} responding with {response}");
                await writer.WriteLineAsync(response).ConfigureAwait(false);
                writer.Flush();
            }
            catch (Exception e)
            {
                Error($"\t ClientId {clientId} timed out waiting for data", e);
                throw;
            }
            finally
            {
                Interlocked.Decrement(ref _activeClientCount);
                NotifyForConnStatChanges();
                reader.Dispose();
                writer.Dispose();
            }

            Debug($"\t ClientId {clientId} disconnected");
        }

        private string Respond(string request)
        {
            var rea = new MessageRequestResponseEventArgs {RequestMessage = request};
            OnResponseEventHandler(rea);
            return rea.ResponseMessage;
        }

        #endregion //#region "worker methods"

        #region  "events and utils"

        /// <summary>
        ///     Subscribe to receive diagnostic messages
        /// </summary>
        public event EventHandler<MessageEventArgs> DebugMessageEventHandler;

        /// <summary>
        ///     Subscribe to receive connection info messages
        /// </summary>
        public event EventHandler<MessageEventArgs> InfoMessageEventHandler;

        /// <summary>
        ///     Subscribe to receive connection info messages
        /// </summary>
        public event EventHandler<ConnectionStatsMessageEventArgs> ConnectionInfoMessageEventHandler;

        /// <summary>
        ///     Subscribe to receive error messages
        /// </summary>
        public event EventHandler<ErrorMessageEventArgs> ErrorMessageEventHandler;

        /// <summary>
        ///     Subscribe to handle replies to requests
        /// </summary>
        public event EventHandler<MessageRequestResponseEventArgs> ResponseEventHandler;

        protected virtual void OnDebugMessageEventHandler(MessageEventArgs e)
        {
            DebugMessageEventHandler?.Invoke(this, e);
        }

        protected virtual void OnInfoMessageEventHandler(MessageEventArgs e)
        {
            InfoMessageEventHandler?.Invoke(this, e);
        }

        protected virtual void OnConnectionInfoMessageEventHandler(ConnectionStatsMessageEventArgs e)
        {
            ConnectionInfoMessageEventHandler?.Invoke(this, e);
        }

        protected virtual void OnErrorMessageEventHandler(ErrorMessageEventArgs e)
        {
            ErrorMessageEventHandler?.Invoke(this, e);
        }

        protected virtual void OnResponseEventHandler(MessageRequestResponseEventArgs e)
        {
            ResponseEventHandler?.Invoke(this, e);
        }

        /// <summary>
        ///     Writes an info message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="methodName"></param>
        private void Info(string message, [CallerMemberName] string methodName = null)
        {
            OnInfoMessageEventHandler(new MessageEventArgs {Message = message, Timestamp = DateTime.Now, SourceName = methodName});
        }

        /// <summary>
        ///     Writes the current connection stats
        /// </summary>
        /// <param name="methodName"></param>
        private void NotifyForConnStatChanges([CallerMemberName] string methodName = null)
        {
            var connStats = new ConnectionStats {ClientConnectionsCount = _totalClientCount, ActiveClientCount = _activeClientCount};
            var connStatsEventArgs = new ConnectionStatsMessageEventArgs {Timestamp = DateTime.Now, SourceName = methodName, Stats = connStats};

            OnConnectionInfoMessageEventHandler(connStatsEventArgs);
        }

        /// <summary>
        ///     Writes a debug message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="methodName"></param>
        private void Debug(string message, [CallerMemberName] string methodName = null)
        {
            OnDebugMessageEventHandler(new MessageEventArgs {Message = message, Timestamp = DateTime.Now, SourceName = methodName});
        }

        /// <summary>
        ///     Writes an error mesage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="methodName"></param>
        private void Error(string message, Exception exception, [CallerMemberName] string methodName = null)
        {
            OnErrorMessageEventHandler(new ErrorMessageEventArgs {Message = message, Timestamp = DateTime.Now, SourceName = methodName, Exception = exception});
        }

        #endregion //#region  "events and utils"
    }
}