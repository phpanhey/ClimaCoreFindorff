using HtmlAgilityPack;
using CommandLine;
using System.Text;

// Class to hold command line options
public class Options
{
    // Option for temperature
    [Option('t', "temperature", Required = false, HelpText = "get temperature.")]
    public bool Temperature { get; set; } = false;

    // Option for humidity
    [Option('h', "humidity", Required = false, HelpText = "get humidity.")]
    public bool Humidity { get; set; } = false;
}

class Program
{
    // URL to fetch data from
    private const string url = "https://www.ach-du-schan.de/";

    static void Main(string[] args)
    {
        // Initialize command line parser
        var parser = new CommandLine.Parser(with => with.HelpWriter = Console.Out);
        var result = parser.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                // Fetch HTML from URL
                string html = GetHtml(url);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                StringBuilder sb = new();
                // If temperature option is set, get temperature
                if (o.Temperature)
                {
                    sb.Append(GetTemperature(htmlDoc));
                }

                // If humidity option is set, get humidity
                if (o.Humidity)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(GetHumidity(htmlDoc));
                }
                
                // Print result
                Console.WriteLine(sb.ToString());
            });
    }

    // Method to fetch HTML from a URL
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

    // Method to get temperature from HTML document
    static double GetTemperature(HtmlDocument htmlDoc)
    {
        var singlenodecontent = SingleNodeContent(htmlDoc, "//font[@face='Arial'][@color='#FF0000'][@size='7']");
        return Double.Parse(singlenodecontent.Replace("�C", ""));
    }

    // Method to get humidity from HTML document
    static double GetHumidity(HtmlDocument htmlDoc)
    {
        string singlenodecontent = SingleNodeContent(htmlDoc, "//font[@face='Arial'][@color='#0080FF'][@size='7']");
        return Double.Parse(singlenodecontent.Replace("%", ""));
    }

    // Method to get content of a single node from HTML document
    static string SingleNodeContent(HtmlDocument htmlDoc, string singleNode)
    {
        var node = htmlDoc.DocumentNode.SelectSingleNode(singleNode);
        return node.InnerHtml;
    }
}