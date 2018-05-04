namespace HttpServer.Responses
{
    public class HttpStatusCode
    {
        internal int Code { get; }
        internal string Status { get; }

        protected internal HttpStatusCode(int code, string status)
        {
            Code = code;
            Status = status;
        }
    }
}