using System;

namespace PreMailer.Net.Downloaders
{
	public interface IWebDownloader
	{
		string DownloadString(Uri uri);

        string DownloadString(string _downloadUri);
    }
}
