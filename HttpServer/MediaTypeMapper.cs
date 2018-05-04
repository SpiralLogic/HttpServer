using System.Collections.Generic;
using System.IO;

namespace HttpServer
{
    public class MediaTypeMapper
    {
        private static IDictionary<string, string> _extensionToMediaTypeMap;

        static MediaTypeMapper()
        {
            AddMediaTypesToMap();
        }

        private static void AddMediaTypesToMap()
        {
            _extensionToMediaTypeMap = new Dictionary<string, string>
            {
                {".jpeg", "image/jpeg"}, 
                {".png", "image/png"}, 
                {".gif", "image/gif"}, 
                {".txt", "text/plain"}
            };
        }

        public static string MediaTypeFromFile(string file)
        {
            var extension = Path.GetExtension(file);

            if (_extensionToMediaTypeMap.TryGetValue(extension, out var mimeType))
            {
                return mimeType;
            }

            return "unknown";
        }
    }
}