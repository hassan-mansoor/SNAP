using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HtmlParser.UIMappings
{
    public class Xml_Reader
    {
      private  String filename = @"C:\Users\ChHassan\Documents\SNAPUserInterfaceDesign\SNAPUIDesign\HtmlParser\UIMappings\UIElements.xml";
      private  XmlDocument xmldocument;
     // private  Hashtable uimappingelm = new Hashtable();
      private Dictionary<string,List<string>> uimappingelm = new Dictionary<string,List<string>>();
      private List<string> lstuielmnt;
     public Xml_Reader()
      {
          xmldocument = new XmlDocument();
          XmlReaderSettings settings = new XmlReaderSettings();
          settings.IgnoreComments = true;
          if (File.Exists(filename))
          {
              XmlReader reader = XmlReader.Create(filename, settings);
              xmldocument.Load(reader);
              XmlNode rootnode = xmldocument.DocumentElement;
              XmlNodeList businessuielmnts = rootnode.ChildNodes;
             
              foreach (XmlElement businesselmnt in businessuielmnts)
              {
                 XmlNodeList uielmnts = businesselmnt.ChildNodes;
                 lstuielmnt = new List<string>();
                 foreach (XmlElement uielmnt in uielmnts)
                 {
                         lstuielmnt.Add(uielmnt.Name); 
                 }

                 uimappingelm.Add(businesselmnt.Name, lstuielmnt);            
              }

          }

      }
        
     public string BusinessUIElement(string uielmnt)
     {
         string businesselement = string.Empty;
         foreach (var keyval in uimappingelm)
         {
             var vlaues = (List<string>)keyval.Value;
             if (vlaues.Contains(uielmnt))
             {
                 businesselement= keyval.Key;
                 break;
             }
         }
         if (businesselement == string.Empty)
         {
             return uielmnt;
         }
         else
         {
             return businesselement;
         }        
        
     }

     public  Dictionary<string, List<string>> getbusinesselement
     {
         get{
             return uimappingelm;
         }             
     
     }


    }
}
