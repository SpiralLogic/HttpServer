namespace HttpServer.RequestHandlers
{
    public enum RequestType
    {
        UNKNOWN,
        GET,
        HEAD,
        POST,
        PATCH,
        PUT,
        DELETE,
        OPTIONS
    }
}