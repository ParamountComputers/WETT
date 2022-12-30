using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Console;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;

namespace WETT.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetAsByteArray(this ISession session, string key, object toSerialize)
        {
            var memoryStream = new MemoryStream();
			System.Text.Json.JsonSerializer.Serialize(memoryStream, toSerialize);
            session.Set(key, memoryStream.ToArray());
        }

        public static List<string> GetAsList(this ISession session, string key)
        {
            var memoryStream = new MemoryStream();

            var objectBytes = session.Get(key) as byte[];
            memoryStream.Write(objectBytes, 0, objectBytes.Length);
            memoryStream.Position = 0;

			return (List<string>)System.Text.Json.JsonSerializer.Deserialize(memoryStream, typeof(List<string>));
		}
    }
}