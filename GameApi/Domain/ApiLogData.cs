using System.Collections.Generic;
using System.Linq;

namespace GameApi.Domain
{
    public class ApiLogData
    {
        public string Method { get; set; }
        public string Uri { get; set; }
        public int Status { get; set; }
        public string ClientIp { get; set; }
        public Dictionary<string, List<string>> RequestHeaders { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }

        public string RequestHeadersToString()
        {
            return string.Join(",",
                RequestHeaders.Select(pair =>
                    "{" + pair.Key + "=" + "[" + pair.Value.Aggregate((x, y) => x + "," + y) + "]}"));
        }
    }
}