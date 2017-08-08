using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace sample.Common
{
	internal static class CommonUtil
	{
		public static string GetBackendAddress()
		{
			string backendAddress = System.Web.HttpContext.Current.Request.Url.Host;
			return $"http://{backendAddress}:51661";
		}

		public static T JsonDeserialize<T>(string jsonString)
		{
			var ser = new DataContractJsonSerializer(typeof(T));
			var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
			var obj = (T)ser.ReadObject(ms);
			return obj;
		}

		public static string GetResponse(String url, string data, string reqType)
		{
			var resResult = string.Empty;

			try
			{
				var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "GET";
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Headers.Add("X-Authorization", "4FDB2B04-89CE-4F0B-8B85-0E43F4C46F6E");
				httpWebRequest.Headers.Add("X-Forwarded-For", "127.0.0.1");
				httpWebRequest.Headers.Add("X-Platform", "GGPOKERDEV");

				if (reqType.Equals("POST"))
				{
					httpWebRequest.Method = "POST";

					var byteArray = Encoding.UTF8.GetBytes(data);
					var dataStream = httpWebRequest.GetRequestStream();

					dataStream.Write(byteArray, 0, byteArray.Length);
					dataStream.Close();
				}

				HttpWebResponse httpWebResponse;
				using (httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					var respPostStream = httpWebResponse.GetResponseStream();
					if (respPostStream == null)
						throw new NullReferenceException(nameof(respPostStream));

					var readerPost = new StreamReader(respPostStream, Encoding.GetEncoding("UTF-8"), true);
					resResult = readerPost.ReadToEnd();
				}
			}
			catch (WebException ex)
			{
				if (ex.Response != null)
				{
					using (var errorResponse = (HttpWebResponse)ex.Response)
					{
						using (var reader = new StreamReader(errorResponse.GetResponseStream()))
						{
							string error = reader.ReadToEnd();
							return error;
						}
					}
				}
			}
			return resResult;
		}
	}
}