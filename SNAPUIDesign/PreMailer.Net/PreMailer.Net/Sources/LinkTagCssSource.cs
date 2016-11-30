using System;
using System.Linq;
using AngleSharp.Dom;
using PreMailer.Net.Downloaders;

namespace PreMailer.Net.Sources
{
	public class LinkTagCssSource : ICssSource
	{
      //  private readonly Uri _downloadUri;// Uri _downloadUri;
     private readonly string _downloadUri;
		private string _cssContents;

		public LinkTagCssSource(IElement node, Uri baseUri)
		{
			// There must be an href
            var href = node.Attributes.First(a => a.Name.Equals("href", StringComparison.OrdinalIgnoreCase)).Value;

            if (Uri.IsWellFormedUriString(href, UriKind.Relative) && baseUri != null)
            {
              //  _downloadUri = new Uri(baseUri, href);
                _downloadUri = href;
            }
            else
            {
                // Assume absolute
                _downloadUri = href;
            }

            
        }
        
		public string GetCss()
		{
			return _cssContents ?? (_cssContents = WebDownloader.SharedDownloader.DownloadString(_downloadUri));
		}
	}
}