using System.Web.Mvc;
using System.Web.Routing;

namespace FileSortAndRename.Helpers
{
    public static class Helpers
    {
        public static string Image(this HtmlHelper helper, string name, string relativePath)
        {
            return Image(helper, name, relativePath, null);
        }

        public static string Image(this HtmlHelper helper, string name, string relativePath, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("img");
            tagBuilder.GenerateId(name);

            tagBuilder.Attributes["src"] = new UrlHelper(helper.ViewContext.RequestContext).Content(relativePath);
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return tagBuilder.ToString();
        }
    }
}