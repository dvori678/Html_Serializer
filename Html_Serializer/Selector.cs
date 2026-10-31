using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class Selector
    {
        public string TagName{ get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new();
        public Selector Parent { get; set; }
        public Selector Child { get; set; } 

        public static Selector FromStringToSelector(string ask)
        {
            Selector selector = new Selector();
            Selector current = selector;
            string []levels=ask.Split();
            for (int i=0;i<levels.Length;i++)
            {
                var parts = Regex.Split(levels[i],"(?=[#.])");
                foreach (var part in parts)
                {
                    if (part.StartsWith('#'))
                        current.Id = part.Substring(1);
                    else if (part.StartsWith('.'))
                        current.Classes.Add(part.Substring(1));
                    else if(HtmlHelper.Instance.HtmlTags.Contains(part)) 
                        current.TagName = part;
                }
                if (i != levels.Length - 1)
                {
                    current.Child = new Selector() { Parent = current };
                    current = current.Child;
                }
            }
            return selector;
        }
    }
}
