using HtmlAgilityPack;
using CommandLine;

public class Options
{
    [Option('t', "temperature", Required = false, HelpText = "get temperature.")]
    public bool Temperature { get; set; } = false;

    [Option('h', "humidity", Required = false, HelpText = "get humidity.")]
    public bool Humidity { get; set; } = false;
}
class Program
{
    private const string url = "https://www.ach-du-schan.de/";

    static void Main(string[] args)
    {
        var parser = new CommandLine.Parser(with => with.HelpWriter = Console.Out);
        var result = parser.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                string html = GetHtml(url);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                if (o.Temperature)
                {
                    string temperature = GetTemperature(htmlDoc).ToString();
                    Console.WriteLine($"Temperature: {temperature}");
                }

                if (o.Humidity)
                {
                    string humidity = GetHumidity(htmlDoc).ToString();
                    Console.WriteLine($"Humidity: {humidity}");
                }
            });

    }
    static string GetHtml(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return responseBody;
        }
    }

    static double GetTemperature(HtmlDocument htmlDoc)
    {
        var singlenodecontent = SingleNodeContent(htmlDoc, "//font[@face='Arial'][@color='#FF0000'][@size='7']");
        return Double.Parse(singlenodecontent.Replace("�C", ""));
    }
    static double GetHumidity(HtmlDocument htmlDoc)
    {
        string singlenodecontent = SingleNodeContent(htmlDoc, "//font[@face='Arial'][@color='#0080FF'][@size='7']");
        return Double.Parse(singlenodecontent.Replace("%", ""));
    }

    static string SingleNodeContent(HtmlDocument htmlDoc, string singleNode)
    {
        var node = htmlDoc.DocumentNode.SelectSingleNode(singleNode);
        return node.InnerHtml;
    }
}
