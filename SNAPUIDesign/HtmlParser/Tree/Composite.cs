using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.Tree
{
    public class Composite : Element
    {
        public Composite(string nodeval)
            : base(nodeval)
        {
            Elements = new List<Element>();
        }

        private List<Element> Elements { get; set; }
        public override void Add(Element element)
        {
            Elements.Add(element);
        }
    }

}
