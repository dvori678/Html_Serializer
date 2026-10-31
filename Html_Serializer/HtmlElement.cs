using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; } = new();
        public List<string> Classes { get; set; } = new();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new();


        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var child in current.Children)
                    queue.Enqueue(child);
                yield return current;
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this;
            while (current.Parent!=null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }

    }
}
