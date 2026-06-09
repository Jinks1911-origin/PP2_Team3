using System.Net;
using System.Text;
using System.Text.Json;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // ← これを追加！
        };


        HttpListener listener = new();
        listener.Prefixes.Add("http://localhost:8080/api/");
        listener.Start();

        Console.WriteLine("Listen.");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;
            var response = context.Response;

            var url = request.Url?.AbsolutePath;
            var segments = url?.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var method = request.HttpMethod;
            Console.WriteLine($"Request Recieved.Url={url},Method={method}");

            if (segments is null || segments.Length < 2)
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();
                Console.WriteLine(HttpStatusCode.NotFound);
                continue;
            }

            var status = HttpStatusCode.OK;
            switch (segments[1])
            {
                case "events":

                    break;
                case "taxis":
                    if (segments.Length != 3)
                    {
                        status = HttpStatusCode.NotFound;
                        break;
                    }

                    if (string.Equals(segments[2], "status"))
                    {
                        string taxiStatusList = """
                        [
                          {
                            "id": 1,
                            "name": "Idle"
                          },
                          {
                            "id": 2,
                            "name": "Reserved"
                          },
                          {
                            "id": 3,
                            "name": "Occupied"
                          },
                          {
                            "id": 4,
                            "name": "OffDuty"
                          }
                        ]
                        """;
                        string cleanJson = taxiStatusList.Replace("\r\n", " ").Trim();

                        SetResponseBody(response, cleanJson);
                        break;
                    }


                    if (!string.Equals(segments[2], "TX001"))
                    {
                        status = HttpStatusCode.NotFound;
                        var error = """
                            { "error": "TAXI_NOT_FOUND" }
                            """;
                        SetResponseBody(response, error);
                        break;
                    }

                    switch (method)
                    {
                        case "GET":
                            string taxiInfo = """
                        {
                          "taxiId": "TX001",
                          "status": "Reserved",
                          "driverName": "佐藤　一郎",
                          "jobId": "J20260609-0034",
                          "fromLoc": "新居浜駅",
                          "toLoc": "銅夢キッチン"
                        }
                        """;
                            string cleanJson = taxiInfo.Replace("\r\n", " ").Trim();

                            SetResponseBody(response, cleanJson);

                            break;
                        case "PUT":


                            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding ?? Encoding.UTF8))
                            {
                                var jsonBody = reader.ReadToEnd();

                                var taxiStatus = JsonSerializer.Deserialize<PutBody>(jsonBody, options);
                                if (taxiStatus is null || taxiStatus.StatusId < 1 || taxiStatus.StatusId > 4)
                                {
                                    status = HttpStatusCode.BadRequest;
                                    var error = """
                                        { "error": "INVALID_STATUS" }
                                        """;
                                    SetResponseBody(response, error);
                                    break;
                                }

                                Console.WriteLine(taxiStatus.StatusId switch
                                {
                                    1 => "Idle",
                                    2 => "Reserved",
                                    3 => "Occupied",
                                    _ => "OffDuty"
                                });
                            }

                            status = HttpStatusCode.NoContent;

                            break;
                        default:
                            status = HttpStatusCode.MethodNotAllowed;
                            break;
                    }


                    break;
                default:
                    status = HttpStatusCode.NotFound;
                    break;
            }

            response.StatusCode = (int)status;
            response.Close();
            Console.WriteLine(status);
        }


        static void SetResponseBody(HttpListenerResponse response, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            response.ContentType = "application/json; charset=utf-8";
            response.ContentLength64 = buffer.Length;

            using var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
        }
    }
}

record PutBody(int StatusId);