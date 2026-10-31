using Html_Serializer;
using System.Text.RegularExpressions;

Console.WriteLine("enter link");
string link= Console.ReadLine();
var html = await Load(link);
var cleanHtml = new Regex("\\s+").Replace(html," ");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Select(s => s.Trim()).Where(s => s.Length > 0);
HtmlElement tree =BuildTree(htmlLines);
Console.WriteLine("enter selector");
string ask = Console.ReadLine();
var selector = Selector.FromStringToSelector(ask);
var result = tree.ElementsCriteria(selector);
Console.WriteLine("results: " + result.Count);

foreach (var element in result)
{
    Console.WriteLine($"<{element.Name}> " +
        $"id= {element.Id} |" +
        $" classes={string.Join(",", element.Classes)} |"+
        $" text={ element.InnerHtml}");
}
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
HtmlElement BuildTree(IEnumerable<string> htmlLines)
{
    var root = new HtmlElement { Name = "root" };
    var current = root;
    foreach (var line in htmlLines)
    {
        var firstWord = line.Split(' ')[0];
        var rest = line.Substring(firstWord.Length).Trim();
        if (firstWord == "/html")
            break;
        else if (firstWord[0] == '/')
        {
            if (HtmlHelper.Instance.HtmlTags.Contains(firstWord.Substring(1)) && current.Parent != null)
                current = current.Parent;
        }
        else if (HtmlHelper.Instance.HtmlTags.Contains(firstWord))
        {
            var element = new HtmlElement { Name = firstWord, Parent = current };
            var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(rest);
            foreach (Match attribute in attributes)
            {
                element.Attributes.Add(attribute.Value);
                if (attribute.Groups[1].Value == "class")
                    element.Classes.AddRange(attribute.Groups[2].Value.Split());
                else if (attribute.Groups[1].Value == "id")
                    element.Id = attribute.Groups[2].Value;
            }
            current.Children.Add(element);
            if (!line.EndsWith('/') && !HtmlHelper.Instance.HtmlVoidTags.Contains(firstWord))
                current = element;
        }
        else
            current.InnerHtml = line;
    }
    return root;
}