using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class SearchModel
    {
        public string Domain { get; private set; }
        public Dictionary<string, string> Results = new Dictionary<string, string>();
        private Dictionary<string, string> resultsFromDB = new Dictionary<string, string>();

        public void GetLinks(string url, int depth)
        {
            var pos = url.IndexOf('/', 8);
            if (pos < 0) pos = url.Length;
            Domain = url.Substring(0, pos);
            resultsFromDB = Database.SelectByDomain(Domain);
            GetLinks(url, 0, depth);
        }

        private void GetLinks(string url, int current, int depth)
        {
            if (current > depth) return;
            var html = new HtmlDocument();
            try
            {
                html = new HtmlWeb().Load(url);
            }
            catch
            {
                return;
            }
            if (resultsFromDB.ContainsKey(url))
                Results[url] = resultsFromDB[url];
            else Results[url] = GetBody(html);
            foreach (var node in html.DocumentNode.SelectNodes("//a"))
            {
                if (node.Attributes["href"] != null)
                {
                    var link = node.Attributes["href"].Value;
                    if (!link.Contains(Domain))
                        if (link.Contains(Domain.Split('/')[2]))
                            link = Domain.Substring(0, Domain.LastIndexOf('/')) + link;
                        else link = Domain + link;
                    if (current < depth && !Results.ContainsKey(link))
                        GetLinks(link, current + 1, depth);
                }
            }
        }

        private string GetBody(HtmlDocument html)
        {
            var text = new Regex(@"\s+").Replace(new Regex(@"<.*>").Replace(html.Text, "").Replace('\r', ' ').Replace('\n', ' '), " ");
            if (text.Length > 500) return text.Substring(0, 500);
            return text;
        }
    }
}
