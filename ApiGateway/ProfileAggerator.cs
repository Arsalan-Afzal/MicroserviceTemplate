using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway
{
    public class ProfileAggregator : IDefinedAggregator
    {
        //public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        //{
        //    var contentBuilder = new StringBuilder();
        //    foreach (var context in responses)
        //    {
        //        contentBuilder.Append(await context.Items.DownstreamResponse().Content.ReadAsByteArrayAsync());
        //        contentBuilder.Append(",");
        //    }

        //    var stringContent = new StringContent(contentBuilder.ToString())
        //    {
        //        Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        //    };

        //    return new DownstreamResponse(stringContent, HttpStatusCode.OK,
        //    responses.SelectMany(x => x.Items.DownstreamResponse().Headers).ToList(), "OK");
        //}
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            List<Header> header = new List<Header>();
            try
            {
                var stringContent = new StringContent("");
                var contentBuilder = new StringBuilder();


                var headers = responses.SelectMany(x => x.Items.DownstreamResponse().Headers).ToList();

                for (int i = 0; i < responses.Count; i++)
                {
                    string jsonvalue = "";
                    var oneByteArray = await responses[i].Items.DownstreamResponse().Content.ReadAsByteArrayAsync();
                    var oneData = Decompress(oneByteArray);
                    string jsonString = Encoding.UTF8.GetString(oneData);
                    var oneObj = ConvertToJson(oneData);

                    if (i == 0)
                    {
                        jsonvalue = JsonConvert.SerializeObject(oneObj).Trim('}');
                        contentBuilder.Append(jsonvalue);
                    }
                    else
                    {
                        jsonvalue = JsonConvert.SerializeObject(oneObj).TrimStart('{');
                        contentBuilder.Append(jsonvalue);
                    }
                    if (i < responses.Count - 1)
                        contentBuilder.Append(",");
                    stringContent = new StringContent(contentBuilder.ToString(), Encoding.UTF8, "application/json");
                }

                return new DownstreamResponse(stringContent, HttpStatusCode.OK, headers, "OK");
            }
            catch (Exception ex)
            {
                return new DownstreamResponse(null, System.Net.HttpStatusCode.InternalServerError, header, null);
            }
        }

        private static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }

        private static JObject ConvertToJson(byte[] data)
        {
            JObject jObj;
            using (var ms = new MemoryStream(data))
            using (var streamReader = new StreamReader(ms))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                jObj = (JObject)JToken.ReadFrom(jsonReader);
            }
            return jObj;
        }
    }
}
