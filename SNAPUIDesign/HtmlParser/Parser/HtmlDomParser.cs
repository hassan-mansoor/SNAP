using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HtmlParser.Tree;
using Newtonsoft.Json;
using HtmlParser.Rules;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;
namespace HtmlParser.Parser
{
    public class HtmlDomParser
    {
       private HtmlDocument htmldoc;
       RuleSet1Aand1B ruleset_1a_and_1b;
       private List<string> ErrorsLog;
       private List<string> cssfile = new List<string>();
       private List<string> XPath = new List<string>();
       string htmlfile = string.Empty;
        public HtmlDomParser(string htmlfilename,string cssfilename ,string xpath)
        {
            ErrorsLog = new List<string>();

            htmlfilename = @"C:\Users\ChHassan\Desktop\SNAP\Validation+Files+Screens\Github\JoinGitHub.html";
            cssfilename = @"C:\Users\ChHassan\Desktop\SNAP\Validation+Files+Screens\Github\Join GitHub · GitHub_files\github-33dd01e395889488eb5f48a92d7382a3e68eb0904904f217320e8876c028c7de.css";

            xpath = "//div[@class='setup-wrapper']";
            cssfile.Add(cssfilename);
            XPath.Add(xpath);
            htmlfile = htmlfilename;
           
            if (File.Exists(htmlfilename))
            {
                try
                {
                    string htmlsource = File.ReadAllText(htmlfilename);
                    if (File.Exists(htmlfilename))
                    {
                        string css = File.ReadAllText(cssfilename);
                        var inlinehtmlcss = PreMailer.Net.PreMailer.MoveCssInline(htmlsource, false, null, css);
                        string htmldocument = inlinehtmlcss.Html;
                        htmldoc = new HtmlDocument();

                        HtmlNode.ElementsFlags.Remove("form");
                        //Supply Rules set 
                        ruleset_1a_and_1b = new RuleSet1Aand1B(htmldocument, xpath);
                        htmldoc.LoadHtml(ruleset_1a_and_1b.ElementryProcess);
                        HtmlNode elmprocess = htmldoc.DocumentNode.SelectSingleNode("//*");
                        //////////////////////////
                        GetHtmlTags(elmprocess);
                    }
                    Console.WriteLine("Success");
                }
                catch
                {

                    ErrorsLog.Add("Error: Invalid XPath supplied.\n");
                    Console.WriteLine("Fail");
                }                
            }
            else
            {
                ErrorsLog.Add("Error: File doesnot exist.\n");
            }
        }


        private void GetHtmlTags(HtmlNode elmprocess)
            {
                List<string> lshtmltags = new List<string>();
                try
                {
                    var htmlbody = from htmltag in elmprocess.DescendantsAndSelf() where htmltag.Name != "#text" && htmltag.Name != "#comment" select htmltag.Name;

                    foreach (var tag in htmlbody.Distinct())
                    {
                        if (ruleset_1a_and_1b.getbusinesselement.ContainsKey(tag))
                        {
                            lshtmltags.Add(tag);
                        }

                    }

                    GetTagAttributes(lshtmltags, elmprocess);
                }
                catch
                {
                    ErrorsLog.Add("Exception: No Parent Node Found.\n");
                }
                
            }

        private void GetTagAttributes(List<string> lshtmltags,HtmlNode elmprocess)
            {
                string Root = string.Empty;
                ArrayList arr_ui_elements = new ArrayList();
                EP_screens ep_screens;
                ArrayList arr_ep_screens = new ArrayList();  
                var lsattr = new List<Tuple<string, string>>();
                List<HtmlNode> lsnodes=new List<HtmlNode>();
                List<Tuple<string, string>> templsnodes = new List<Tuple<string, string>>();
                Composite SNAP2_1_UIDesign = new Composite("SNAP2_1_UIDesign");
                Composite Main = new Composite("DOMTree");                
                foreach (var htmltag in lshtmltags)
                {
                    HtmlNode[] nodes =elmprocess.ParentNode.SelectNodes(".//"+htmltag).ToArray();
                    Root = htmltag; //Root Node                   
                    foreach(var node in nodes)
                    {
                       lsnodes.Add(node);
                       string name = (from n in node.Attributes where n.Name == "realname" select n.Value).FirstOrDefault();
                       templsnodes.Add(Tuple.Create(node.OuterHtml, name));
                       node.Attributes.Remove("class");
                       node.Attributes.Remove("id");                     
                        var attributes = from nodeattributes in node.Attributes select nodeattributes;
                        
                        foreach (var attribute in attributes)
                        {
                           
                            if (attribute.Name == "realname")
                            {
                                continue;
                            }
                            if (attribute.Name == "style")
                            {                              
                                foreach (var nameval in StyleAttribute(attribute.Value))
                                {
                                    lsattr.Add(Tuple.Create(nameval.Item1, nameval.Item2));
                                }
                            }
                            else
                            {
                                lsattr.Add(Tuple.Create(attribute.Name, attribute.Value));
                            }                        
                        }                       
                    }
                   arr_ui_elements.Add(AddToTree(lsattr, Root, lsnodes,""));
                    //Add to Tree  
                    Main.Add(AddToTree(lsattr, Root,lsnodes));                   
                    lsattr.Clear();
                    lsnodes.Clear();
                }

                ep_screens = new EP_screens { CSS_files = cssfile, HTML_file = htmlfile, XPaths = XPath, UI_elements = arr_ui_elements };
                arr_ep_screens.Add(ep_screens);
                Elements e = new Elements { EP_name = "Join GitHub · GitHub.html", EP_screens = arr_ep_screens, Errors = this.Errors(ErrorsLog) };
                WriteTOJson(e);
                SNAP2_1_UIDesign.Add(Main);
                Composite HTMLDOM = new Composite("HTMLDOM");
               HTMLDOM.Add(new ConcreteElement(ruleset_1a_and_1b.DOM.ToString()));
               SNAP2_1_UIDesign.Add(HTMLDOM);
                Composite Errors = new Composite("ERRORS");
                Errors.Add(new ConcreteElement(LogErrors(ErrorsLog)));
                SNAP2_1_UIDesign.Add(Errors);
                WriteTOJson(SNAP2_1_UIDesign);               
            }


        private List<Tuple<string, string>> StyleAttribute(string styleattribute)
            {
              
                var stylattrnameval = new List<Tuple<string, string>>();
               
                try
                {
                    var mapattr = styleattribute
        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(xx => xx.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries))
        .ToDictionary(p => p[0], p => p[1]);


                    foreach (var attr in mapattr)
                    {
                       
                        stylattrnameval.Add(Tuple.Create(attr.Key.ToString(), attr.Value.ToString()));
                    }
                }
                catch (Exception e)
                {
                    ErrorsLog.Add("Error:Pelease check correctly your CSS file because some properties may not be extracted due to incorrect CSS.\n");
                }
                return stylattrnameval;
            }

        /// <summary>
        /// Obsulete Method
        /// </summary>
        /// <param name="attributelist"></param>
        /// <param name="rootnode"></param>
        /// <param name="lsnodes"></param>
        /// <returns></returns>
        /// 

        #region old AddtoTree
        private Composite AddToTree(List<Tuple<string, string>> attributelist, string rootnode, List<HtmlNode> lsnodes)
        {

            Composite Root = new Composite(rootnode);
            Composite Child = null;
            Composite Leaf = null;
            var attributename = from attr in attributelist select attr.Item1;
            foreach (var attrname in attributename.Distinct())
            {
                Child = new Composite(attrname.ToString());
                var attrvals = from vals in attributelist where vals.Item1 == attrname select vals.Item2;

                foreach (var values in attrvals.Distinct())
                {
                    Leaf = new Composite(values.ToString());
                    List<string> domnodes = ProcessUIElementNodes(lsnodes, values.ToString(), attrname.ToString());
                    foreach (var leafnode in domnodes.Distinct())
                    {
                        Leaf.Add(new ConcreteElement(leafnode));
                    }

                    Child.Add(Leaf);
                }

                Root.Add(Child);

            }

            return Root;
        }
        #endregion

        private UI_elements AddToTree(List<Tuple<string, string>> attributelist, string rootnode, List<HtmlNode> lsnodes,string s)
        {
            Property_Values property_values;
            Properties properties;
            UI_elements ui_elements;
            ArrayList arr_properties = new ArrayList(); 
           
            var attributename = from attr in attributelist select attr.Item1;
            foreach (var attrname in attributename.Distinct())
            {
                ArrayList arr_property_values = new ArrayList();
                        
                var attrvals = from vals in attributelist where vals.Item1 == attrname select vals.Item2;

                foreach (var values in attrvals.Distinct())
                {
                    List<string> ls_html_fragments = new List<string>();                  
                    List<string> htmlfragments = ProcessUIElementNodes(lsnodes, values.ToString(), attrname.ToString());
                   
                    foreach (var htmlfragment in htmlfragments.Distinct())
                    {
                        ls_html_fragments.Add(htmlfragment);                        
                    }
                    property_values = new Property_Values { value = values.ToString(), HTML_fragments = ls_html_fragments };
                    arr_property_values.Add(property_values);                   
                }
                properties = new Properties { property_name = attrname.ToString(), property_values = arr_property_values };
                arr_properties.Add(properties);
            }

            ui_elements = new UI_elements { UI_element_name = rootnode, properties = arr_properties };           
            return ui_elements;
        }


        private void WriteTOJson(Composite DOMTree)
        {
           string Tree= JsonConvert.SerializeObject(DOMTree, Formatting.Indented, new CompositeConverter());
         
           using (StreamWriter writer =

         new StreamWriter("JsonTreeUI.json"))
           {
               writer.Write(Tree.Replace("\\n",""));

           }
           
        }

        private void WriteTOJson(Elements DOMTree)
        {
            
            string Tree = JsonConvert.SerializeObject(DOMTree, Formatting.Indented);

            using (StreamWriter writer =

          new StreamWriter("JsonTree.json"))
            {
                writer.Write(Tree.Replace("\\n", ""));

            }

        }

        /// <summary>
        /// This method is obsulete
        /// </summary>
        /// <param name="lsnodes"></param>
        /// <returns></returns>
        /// 
        #region Composite ProcessUIElements obsulete method
        private Composite ProcessUIElementNodes(List<HtmlNode> lsnodes)
        {
            Composite Child=null;
           string tempnodename ;
            foreach (HtmlNode node in lsnodes)
            {
                string attr = (from nodename in node.Attributes where nodename.Name == "realname" select nodename.Value).FirstOrDefault();
                tempnodename = node.Name;
                node.Name = attr.ToString();

                string htmlnode = Regex.Replace(node.OuterHtml, "realname=\"(.*?)\"", string.Empty);                
                Child = new Composite("DOM Node");
                Child.Add(new ConcreteElement(htmlnode));
                node.Name = tempnodename;
                
            }

            return Child;
        }

#endregion


        private List<string> ProcessUIElementNodes(List<HtmlNode> lsnodes,string attrvalue,string attrname)
        {
            List<string> domnodes = new List<string>();
            string tempnodename;
            string htmlnode=string.Empty;
            foreach (HtmlNode node in lsnodes)
            {
                string attr = (from nodename in node.Attributes where nodename.Name == "realname" select nodename.Value).FirstOrDefault();
                tempnodename = node.Name;
                node.Name = attr.ToString();
                string value = (from nodename in node.Attributes where nodename.Name == attrname select nodename.Value).FirstOrDefault();
                if (value == null)
                {
                    if (node.Attributes.Contains("style") || node.Attributes.Contains("style"))
                    {
                        string checkinstyle = (from nodename in node.Attributes where nodename.Name == "style" || nodename.Name == "Style" select nodename.Value).Single();
                        List<Tuple<string, string>> lsstyleattr = StyleAttribute(checkinstyle);
                        if (lsstyleattr.Any(t => t.Item1 == attrname && t.Item2 == attrvalue))
                        {
                            htmlnode = Regex.Replace(node.OuterHtml, "realname=\"(.*?)\"", string.Empty);
                            htmlnode = Regex.Replace(htmlnode, tempnodename, attr.ToString());
                            domnodes.Add(htmlnode);
                    }                   
                        
                    }
                }
                else if (value == attrvalue)
                {
                    htmlnode = Regex.Replace(node.OuterHtml, "realname=\"(.*?)\"", string.Empty);
                    htmlnode = Regex.Replace(htmlnode, tempnodename, attr.ToString());                  
                    domnodes.Add(htmlnode);
                }
               
                node.Name = tempnodename;

            }

            return domnodes;
        }


        private string LogErrors(List<string> errors)
        {
            string _error = string.Empty;
            
            foreach(var error in errors.Distinct())
            {
                _error += error;
            }
            return _error;
        }

        private List<string> Errors(List<string> errors)
        {
            List<string> e = new List<string>();
            foreach (var error in errors.Distinct())
            {
                e.Add(error);
            }
            return e;
        }

    }
}
