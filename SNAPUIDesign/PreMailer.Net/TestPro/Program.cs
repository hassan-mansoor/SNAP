using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreMailer;

using System.IO;
using System.Text.RegularExpressions;

namespace TestPro
{
    class Program
    {
        static void Main(string[] args)
        {
            string css=string.Empty;
            string htmlSource = File.ReadAllText(@"C:\Users\ChHassan\Desktop\SNAP\sample.html");//C:\Users\ChHassan\Downloads\Sport & Fitness.html
            string stylesheet;
            css += File.ReadAllText(@"C:\Users\ChHassan\Desktop\SNAP\css\style.css");
            stylesheet = Regex.Replace(css, @"[\r\n]", string.Empty); // remove newlines
            while (stylesheet.Contains("  "))
            {
                stylesheet = stylesheet.Replace("  ", " ");
            }
            var result = PreMailer.Net.PreMailer.MoveCssInline(htmlSource,false,null,stylesheet);
           string str= result.Html;         // Resultant HTML, with CSS in-lined.
           //result.Warnings;    // string[] of any warnings that occurred during processing.

           Console.WriteLine(str);
           Console.ReadKey();

        }
    }
}
