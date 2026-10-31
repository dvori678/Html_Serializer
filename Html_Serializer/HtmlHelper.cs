using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace Html_Serializer
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance=new HtmlHelper();
        public static HtmlHelper Instance=>_instance;
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        private HtmlHelper()
        {
            HtmlTags = Deserialize(File.ReadAllText("JSON Files/HtmlTags.json"));
            HtmlVoidTags = Deserialize(File.ReadAllText("JSON Files/HtmlVoidTags.json"));
        }
        private string[] Deserialize(string json)
        {
            return JsonSerializer.Deserialize<string[]>(json);
        }
    }
}
