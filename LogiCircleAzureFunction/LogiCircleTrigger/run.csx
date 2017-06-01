using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("Logi Circle Control trigger activated");

    dynamic data = await req.Content.ReadAsAsync<object>();

    string power = data?.power;

    //Make this a loop discovering the number of cameras
    var camera1Response = await SignIn(power,System.Environment.GetEnvironmentVariable("CameraUrl1"));
    var camera2Response = await SignIn(power,System.Environment.GetEnvironmentVariable("CameraUrl2"));

    if (camera1Response.IsSuccessStatusCode && camera2Response.IsSuccessStatusCode)
    {
        log.Info("Trigger was successful, power is " + power);
        return req.CreateResponse(HttpStatusCode.OK, "power is " + power);
    }

    log.Info("Trigger failed");
    return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a power option in the request body");
}

public static async Task<HttpResponseMessage> SignIn(string power, string cameraUrl)
{
    using (var client = new HttpClient())
    {
        Dictionary<string, string> loginInformation = new Dictionary<string, string>()
        {
            {"email", System.Environment.GetEnvironmentVariable("LogiEmail")},
            {"password", System.Environment.GetEnvironmentVariable("LogiPassword")}
        };

        var json =BuildJson(loginInformation);
        
        await client.PostAsync(System.Environment.GetEnvironmentVariable("LogiLoginUrl"), json);
  
        var camera = new Dictionary<string, string>()
        {
            {"streamingMode", power}
        };

        return await client.PutAsync(cameraUrl, BuildJson(camera));
    }
}

public static HttpContent BuildJson(Dictionary<string, string> values)
{
    var json = JsonConvert.SerializeObject(values);

    return new StringContent(json, Encoding.UTF8, "application/json");
}