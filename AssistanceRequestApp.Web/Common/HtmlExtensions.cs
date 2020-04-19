namespace AssistanceRequestApp.Web.Common
{
    using System;
    using System.Linq;
    using ServiceStack;
    public static class HtmlExtensions
    {
        public static IHtmlString DisplayFormattedData(this System.Web.WebPages.Html.HtmlHelper htmlHelper, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return new ServiceStack.MiniProfiler.HtmlString(string.Empty);
            }

            var result = string.Join(
                "<br/>",
                data
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                    .Select(htmlHelper.Encode)
            );
            return new ServiceStack.MiniProfiler.HtmlString(result);
        }
    }
}
