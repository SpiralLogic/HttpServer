namespace HttpServer.Responses
{
    public class HttpStatusCodes
    {
        public static readonly HttpStatusCode Ok = new HttpStatusCode(200, "Ok");
        public static readonly HttpStatusCode Created = new HttpStatusCode(201, "Created");
        public static readonly HttpStatusCode NoContent = new HttpStatusCode(204, "No Content");
        public static readonly HttpStatusCode PartialContent = new HttpStatusCode(206, "Partial Content");

        public static readonly HttpStatusCode Found = new HttpStatusCode(302, "Found");

        public static readonly HttpStatusCode BadRequest = new HttpStatusCode(400, "Bad Request");
        public static readonly HttpStatusCode NotAuthorized = new HttpStatusCode(401, "Not Authorized");
        public static readonly HttpStatusCode NotFound = new HttpStatusCode(404, "Not Found");
        public static readonly HttpStatusCode RangeNotSatisfiable = new HttpStatusCode(416, "Requested Range Not Satisfiable");
        public static readonly HttpStatusCode Teapot = new HttpStatusCode(418, "I'm a Teapot");

        public static readonly HttpStatusCode MethodNotAllowed = new HttpStatusCode(405, "Method Not Allowed");
        public static readonly HttpStatusCode NotImplemented = new HttpStatusCode(501, "Not Implemented");
    }
}