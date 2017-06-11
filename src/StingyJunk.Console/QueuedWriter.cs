namespace StingyJunk.Console
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using Extensions;

    internal class QueuedWriter
    {
        private bool _acceptNewItems;
        private bool _keepWriting;
        private bool _terminationRequested ;
        private Thread _queueDrainer;
        private readonly object _groupLock = new object();
        private readonly ConcurrentQueue<ConsoleMessage> _queuedMessages = new ConcurrentQueue<ConsoleMessage>();

        public QueuedWriter()
        {
            StartWorking();
        }

        private void StartWorking()
        {
            _acceptNewItems = true;
            _keepWriting = true;
            _terminationRequested = false;
            if (_queueDrainer == null || _queueDrainer.IsAlive == false)
            {
                _queueDrainer = new Thread(QueueWriter);
            }
            _queueDrainer.Start();
        }

        public void WriteMessage(Position writePosition, string message, Flair flair)
        {
            Dwl($"start queue for write {message}");
            if (_acceptNewItems == false)
            {
                Dwl($"ignored request to write {message}");
                return;
            }
            lock (_groupLock)
            {
                _queuedMessages.Enqueue(new ConsoleMessage(message, flair, writePosition));
            }
        }


        public void WriteMessages(IEnumerable<ConsoleMessage> consoleMessages)
        {
            var messages = consoleMessages as ConsoleMessage[] ?? consoleMessages.ToArray();
            Dwl($"start request to write {messages.Select(m => m.Message).ToList().ToNewLineList()}");

            if (_acceptNewItems == false)
            {
                Dwl($"ignored request to write message group");
                return;
            }

            lock (_groupLock)
            {
                foreach (var cm in messages)
                {
                    _queuedMessages.Enqueue(cm);
                }
            }
        }


        public void Restart()
        {
            StartWorking();
        }

        public void Halt(bool flushFirst = true)
        {
            _acceptNewItems = false;
            //this will stop writes immediately if set to false. 
            //otherwise it will empty the queue
            _keepWriting = flushFirst;
            _terminationRequested = true;
        }

        private void QueueWriter()
        {
            while (_keepWriting)
            {
                ConsoleMessage cm;
                //lock (_groupLock)
                //{

                // ReSharper disable InconsistentlySynchronizedField
                if (_queuedMessages.TryDequeue(out cm) == false)
                    // ReSharper restore InconsistentlySynchronizedField
                {
                    if (_terminationRequested)
                    {
                        _keepWriting = false;
                        break;
                    }
                    Thread.Sleep(100);
                    continue;
                }
                //}

                Console.SetCursorPosition(cm.WritePosition.Left, cm.WritePosition.Top);
                Console.ForegroundColor = cm.Flair.ForegroundColor;
                Console.BackgroundColor = cm.Flair.BackgroundColor;
                var msgToWrite = cm.Message;
                var width = ConsoleWidth();
                Dwl($"{nameof(msgToWrite)} ({msgToWrite.Length}), console window width {width}. console buffer width {Console.BufferWidth}");
                if (msgToWrite.Length < width)
                {
                    var padding = new string(' ', width -1 - cm.Message.Length);
                    var whitespaceCleanedMessage = $"{cm.Message}{padding}";
                    Dwl($"{nameof(msgToWrite)} ({msgToWrite.Length}) is less than the console window width ({width}), padding with {padding.Length} spaces to be {whitespaceCleanedMessage.Length}");
                    
                    msgToWrite = whitespaceCleanedMessage;

                }

                Dwl($"{cm.WritePosition} : '{msgToWrite}'");
                
                Console.WriteLine(msgToWrite);
            }
        }


        private static int ConsoleWidth()
        {
            return Console.WindowWidth;
        }

        private void Dwl(string message)
        {
            Debug.WriteLine($"{nameof(QueuedWriter)} - {DateTime.Now.TimeOfDay} - {message} ");
        }

    }
}