using HtmlAgilityPack;
using HtmlParser.UIMappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.Rules
{
    public class RuleSet1Aand1B
    {
        HtmlNode elmpro;
        Xml_Reader uimappingelements;
        private string htmldom = string.Empty;
        private string ep = string.Empty;
        public RuleSet1Aand1B(string doc,string xpath)
       {           
           uimappingelements = new Xml_Reader();
           List<string> distincttags = LoadDocument(doc, xpath);
          Rule1(distincttags,elmpro);
        }
        private List<string> LoadDocument(string doc, string xpath)
        {

          HtmlDocument  htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(doc);
            HtmlNode elmpro = htmldoc.DocumentNode.SelectSingleNode(xpath);
            htmldom = elmpro.OuterHtml;
            this.elmpro = elmpro;
            return GetAllTags(elmpro);
        }
        private List<string> GetAllTags(HtmlNode elmprocess)
        {
            List<string> lshtmltags = new List<string>();
            var htmlbody = from htmltag in elmprocess.DescendantsAndSelf() where htmltag.Name != "#text" && htmltag.Name != "#comment"  select htmltag.Name;

            foreach (var tag in htmlbody.Distinct())
            {
                lshtmltags.Add(tag);
            }

            return lshtmltags;
        }
        private void Rule1(List<string> distincttags, HtmlNode elmpro)
        {
            foreach (var tag in distincttags)
            {
                HtmlNode[] nodes = elmpro.ParentNode.SelectNodes(".//" + tag).ToArray();

                foreach (var node in nodes)
                {
                    Rule1A(node);
                    Rule1B(node);
                }
            }
            ep = elmpro.OuterHtml;
        }
        private HtmlNode Rule1B(HtmlNode node)
        {           //if tag x has 
            string constantattribute = " and realname=value"; //this attribute needs to be constant inorder to preserve the value.
            string rulestatement = "attr0=val0 and attr1=val1 and attr2=val2 and attr3=val3" + constantattribute;//[attr1=val1 and att2=val2 and att3=val3 and.....]
            string tempattrname = string.Empty;
            string tempattrvalue = string.Empty;
            bool flag = false;            
            int index = 0;

            string[] splitruletoarray = rulestatement.Split(' ').Where(z => !z.Equals("and")).ToArray();
            string[,] attrnamevalu = new string[splitruletoarray.Length, 2];
            for (int i = 0; i < splitruletoarray.Length; i++)
            {
                string[] splt = splitruletoarray[i].Split('=');
                attrnamevalu[i, 0] = splt[0];
                attrnamevalu[i, 1] = splt[1];
            }
            var attributes = from attr in node.Attributes select attr;
            
            if (node.Attributes.Contains("class") | node.Attributes.Contains("id"))
            {
                foreach (var attr in attributes)
                {
                    try
                    {
                        attrnamevalu[index, 0] = attr.Name;
                        attrnamevalu[index, 1] = attr.Value;
                        index++;
                    }
                    catch
                    {
                        flag = true;
                        break;
                    }                    
                }
                if (flag == true)
                {    
                   node.Name = "custom" + node.Name;  //then tag x becomes tag z
                }

            }


            return node;
        }
        private HtmlNode Rule1A(HtmlNode curnode)
        {

            string tempattr = curnode.Name;
            curnode.Attributes.Add("RealName", tempattr);
            if (curnode.Name == "input")
            {
                string attr = (from attrs in curnode.Attributes where attrs.Name == "type" select attrs.Value).FirstOrDefault();
                if (attr == null)
                {
                    curnode.Name = uimappingelements.BusinessUIElement(tempattr);
                }
                else
                {
                    curnode.Name = uimappingelements.BusinessUIElement(attr);  
                }                                
            }
            else
            {
                curnode.Name = uimappingelements.BusinessUIElement(tempattr);
            }
           
            return curnode;
        }
        public  Dictionary<string, List<string>> getbusinesselement
        {
            get
            {
                return uimappingelements.getbusinesselement;
            }

        }

        public string DOM
        {
            get
            {
                return htmldom;
            }
        }

        public string ElementryProcess
        {
            get
            {
                return ep;
            }
        }
    }
}
