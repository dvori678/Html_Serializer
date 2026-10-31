using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal static class HtmlElementExtensions
    {
        public static HashSet<HtmlElement> ElementsCriteria(this HtmlElement element, Selector selector)
        {
            HashSet<HtmlElement> collection=new HashSet<HtmlElement>();
            ElementsCriteria(element, selector, collection);
            return collection;
        }
        private static void ElementsCriteria(HtmlElement element, Selector selector, HashSet<HtmlElement> collection)
        {
            IEnumerable<HtmlElement> offspring = element.Descendants();//רשימת הצאצאים של אלמנט
            List<HtmlElement> sort=new List<HtmlElement>();
            bool flag = false; ;
            foreach (var child in offspring) {
                if (child == element)
                    continue;
                if ((selector.TagName == null || selector.TagName == child.Name) && (selector.Id==null||selector.Id==child.Id))
                {
                    flag = true;
                    foreach(var classs in selector.Classes)
                        if (!child.Classes.Contains(classs))
                        {
                            flag = false;
                            break;
                        }
                    if (flag)
                        sort.Add(child);
                }
            }
            if (selector.Child == null)
            {
                collection.UnionWith(sort);
                return;
            }
            else
                foreach (var child in sort)
                    ElementsCriteria(child, selector.Child, collection);
        }
    }
}
