using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.Tree
{
    public abstract class Element
    {
        protected string _nodeval;        
        public Element(string nodeval)
        {
            _nodeval = nodeval;

        }
        public abstract void Add(Element element);
        public string Value { get { return _nodeval; } }

    }
}
