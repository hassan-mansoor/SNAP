using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.Tree
{
    class TestJson
    {
    }
    public class Elements
    {
        public string EP_name { get; set; }
        public ArrayList EP_screens { set; get; }
        public List<string> Errors { set; get; }
    }
    public class EP_screens
    {
        public string HTML_file { get; set; }
        public List<string> CSS_files { get; set; }
        public List<string> XPaths { get; set; }
        public ArrayList UI_elements { get; set; }
    }

    public class UI_elements
    {
        public string UI_element_name { get; set; }
        public ArrayList properties { get; set; }
    }

    public class Properties
    {
        public string property_name { get; set; }
        public ArrayList property_values { get; set; }
    }

    public class Property_Values
    {
        public string value { get; set; }
        public List<string> HTML_fragments { get; set; }
    }
}
