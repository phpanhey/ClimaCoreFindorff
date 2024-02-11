using HtmlAgilityPack;

string url = "https://www.ach-du-schan.de/";

string html = getHtml(url);
double temperature = getTemperature(html);
Console.WriteLine(temperature);

double getTemperature(string html)
{
    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);
    var fontNode = htmlDoc.DocumentNode.SelectSingleNode("//font[@face='Arial'][@color='#FF0000'][@size='7']");
    return Double.Parse(fontNode.InnerHtml.Replace("�C", ""));
}

string getHtml(string url)
{
    using (HttpClient client = new HttpClient())
    {
        HttpResponseMessage response = client.GetAsync(url).Result;
        response.EnsureSuccessStatusCode();
        string responseBody = response.Content.ReadAsStringAsync().Result;
        return responseBody;
    }
}
