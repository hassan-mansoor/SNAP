using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.Tree
{
    public class ConcreteElement : Element
    {
        public ConcreteElement(string nodeval)
            : base(nodeval)
        {
          
        }      
        public override void Add(Element element)
        {
            throw new InvalidOperationException("ConcreteElements may not contain Child nodes. Perhaps you intended to add this to a Composite");
        }
    }

}
