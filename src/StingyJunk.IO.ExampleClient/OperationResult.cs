namespace StingyJunk.IO.ExampleClient
{
    using StingyJunk.Extensions;
    using System;

    internal class OperationResult
    {
        public int ClientId { get; set; }
        public long ElapsedMs { get; set; }
        public string RequestMessage { get; set; }
        public DataExchangeResult DataExchangeResult { get; set; }
        public Exception Ex { get; set; }

        public override string ToString()
        {
            var state = (Ex != null && DataExchangeResult.Errors.Count == 0) ? "Success" : "ERROR";
            return $"{nameof(ClientId)} '{ClientId}' - {nameof(ElapsedMs)} {ElapsedMs} - {state}" +
                $" - {nameof(RequestMessage)} '{RequestMessage}'" +
                $" - {nameof(DataExchangeResult.ResponseMessage)} : {DataExchangeResult.ResponseMessage}";
        }

        public string WithErrors()
        {
            var current = ToString();
            var exDetail = Ex == null ? string.Empty : Ex.ToString();
            var errors = DataExchangeResult?.Errors.Count == 0 ? string.Empty : DataExchangeResult?.Errors.ToCsl();
            return $"{current} - {exDetail} - {errors}";
        }
    }
}