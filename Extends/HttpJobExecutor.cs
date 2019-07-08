using AsyncHttpJob.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AsyncHttpJob.Extends
{
    public static class HttpJobExecutor
    {
        [HttpJobFilter]
        public static void DoRequest(HttpJobDescriptor jobDestriptor)
        {
            var client = new RestClient(jobDestriptor.HttpUrl);
            var httpMethod = (object)Method.POST;
            if (!Enum.TryParse(typeof(Method), jobDestriptor.HttpMethod.ToUpper(), out httpMethod))
                throw new Exception($"不支持的HTTP动词：{jobDestriptor.HttpMethod}");
            var request = new RestRequest((Method)httpMethod);
            if (jobDestriptor.JobParameter != null)
            {
                var json = JsonConvert.SerializeObject(jobDestriptor.JobParameter);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
            }
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"调用接口{jobDestriptor.HttpUrl}失败，接口返回：{response.Content}");
        }
    }
}
