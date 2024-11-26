using System.Web;

namespace Application.Helpers;

public static class HttpExtensions
{
    public static Uri AddQueryParam(this Uri uri, string name, string value)
    {
        var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

        httpValueCollection.Remove(name);

        httpValueCollection.Add(name, value);

        var uriBuilder = new UriBuilder(uri)
        {
            Query = httpValueCollection.ToString() ?? string.Empty,
        };

        return uriBuilder.Uri;
    }
}