namespace HttpServer.Responses
{
    internal class HttpStatusCodes
    {
        internal static HttpStatusCode Ok = new HttpStatusCode(200, "Ok");
        internal static HttpStatusCode Created = new HttpStatusCode(201, "Created");
        internal static HttpStatusCode NoContent = new HttpStatusCode(204, "No Content");
        internal static HttpStatusCode PartialContent = new HttpStatusCode(206, "Partial Content");

        internal static HttpStatusCode Found = new HttpStatusCode(302, "Found");

        internal static HttpStatusCode BadRequest = new HttpStatusCode(400, "Bad Request");
        internal static HttpStatusCode NotAuthorized = new HttpStatusCode(401, "Not Authorized");
        internal static HttpStatusCode NotFound = new HttpStatusCode(404, "Not Found");
        internal static HttpStatusCode RangeNotSatisfiable = new HttpStatusCode(416, "Requested Range Not Satisfiable");
        internal static HttpStatusCode Teapot = new HttpStatusCode(418, "I'm a Teapot");

        internal static HttpStatusCode MethodNotAllowed = new HttpStatusCode(405, "Method Not Allowed");
        internal static HttpStatusCode NotImplemented = new HttpStatusCode(501, "Not Implemented");
    }
}