using Server.Contexts;
using Server.Model;
using System.Net;
using System.Text.Json;

Dictionary<string, int> requestCounts = new();

var client = new HttpClient();
var listener = new HttpListener();

listener.Prefixes.Add("http://localhost:27001/");

listener.Start();

while (true)
{
    var context = await listener.GetContextAsync();

    var request = context.Request;

    if (request == null)
        continue;

    switch (request.HttpMethod)
    {
        case "GET":
            {
                var response = context.Response;

                var key = request.QueryString["key"];

                ArgumentNullException.ThrowIfNull(key, nameof(key));

                if (!requestCounts.ContainsKey(key))
                    requestCounts[key] = 0;


                var dbContext = new KeyValueDbContext();

                var temp = dbContext.Find<KeyValue>(key);

                if (temp is not null)
                {
                    response.ContentType = "application/json";

                    response.StatusCode = (int)HttpStatusCode.OK;

                    var keyValue = temp;
                    var jsonStr = JsonSerializer.Serialize(keyValue);

                    var writer = new StreamWriter(response.OutputStream);
                    await writer.WriteAsync(jsonStr);
                    writer.Flush();

                }
                else
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();

                break;
            }
        case "POST":
            {

                break;
            }

        case "PUT":
            {

                break;
            }

    }


}
