namespace HttpServer.RequestHandlers
{
    public class RequestTypes
    {
        internal static RequestType Unknown = new RequestType("UNKNOWN");
        internal static RequestType Options = new RequestType("OPTIONS");
        
        public static RequestType Get = new RequestType("GET");
        public static RequestType Head = new RequestType("HEAD");
        public static RequestType Post = new RequestType("POST");
        public static RequestType Patch = new RequestType("PATCH");
        public static RequestType Put = new RequestType("PUT");
        public static RequestType Delete = new RequestType("DELETE");
    }
}