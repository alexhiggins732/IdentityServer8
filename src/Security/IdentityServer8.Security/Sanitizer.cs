using IdentityServer8.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Web;

namespace IdentityServer8.Security
{
    public enum SanitizerType
    {
        Unknown = 0,
        HtmlSanitizer,
        XmlSanitizer,
        JsonSanitizer,
        UrlSanitizer,
        CssSanitizer,
        ScriptSanitizer,
        StyleSanitizer,
        SqlSanitizer,
        LogSanitizer
    }
    public enum SanitizerMode
    {
        Debug,
        Clean,
        Mask,
        Full
    }
    public interface IInputSanitizer
    {
        public string? Sanitize(string? input, SanitizerMode mode = SanitizerMode.Clean);
    }
    public interface ISanitizerFactory
    {
        TSanitizer Create<TSanitizer>()
            where TSanitizer : IInputSanitizer;

        IInputSanitizer Create(SanitizerType type);
    }
    public interface IHtmlSanitizer : IInputSanitizer
    {
    }
    public interface IXmlSanitizer : IInputSanitizer
    {
    }
    public interface IJsonSanitizer : IInputSanitizer
    {
    }
    public interface IUrlSanitizer : IInputSanitizer
    {
    }
    public interface ICssSanitizer : IInputSanitizer
    {
    }
    public interface IScriptSanitizer : IInputSanitizer
    {
    }
    public interface IStyleSanitizer : IInputSanitizer
    {
    }
    public interface ISqlSanitizer : IInputSanitizer
    {
    }
    public interface ILogSanitizer : IInputSanitizer
    {
    }

    public interface ISanitizerService
    {
        string? Sanitize(string? input, SanitizerType type, SanitizerMode mode);
    }

    public abstract class SanitizerServiceBase : ISanitizerService
    {
        public abstract string? Sanitize(string? input, SanitizerType type, SanitizerMode mode);
    }
    public class SanitizerService : SanitizerServiceBase
    {
        private readonly ISanitizerFactory _sanitizerFactory;

        public SanitizerService(ISanitizerFactory sanitizerFactory)
        {
            _sanitizerFactory = sanitizerFactory;
        }
        public override string? Sanitize(string? input, SanitizerType type, SanitizerMode mode)
        {
            var sanitizer = _sanitizerFactory.Create(type);
            return sanitizer.Sanitize(input, mode);
        }


    }

    public class SanitizerBase : IInputSanitizer
    {
        private Func<string?, string?> _sanitize;

        public SanitizerBase() : this(HttpUtility.HtmlEncode)
        {
        }

        public SanitizerBase(Func<string?, string?> sanitizer)
        {
            _sanitize = sanitizer;
        }

        public virtual string? Sanitize(string? input, SanitizerMode mode = SanitizerMode.Debug)
        {
            switch (mode)
            {
                case SanitizerMode.Debug:
                    return input;
                case SanitizerMode.Clean:
                    return Clean(input);
                case SanitizerMode.Mask:
                case SanitizerMode.Full:
                    var result = _sanitize(input) ?? "";
                    switch (mode)
                    {
                        case SanitizerMode.Mask:
                            return Mask(result);
                        case SanitizerMode.Full:
                            return result;
                        default:
                            throw new NotImplementedException();
                    }
                default:
                    throw new NotImplementedException();
            }
        }


        public string? Mask(string? input, int unmaskedChars = 4, bool unmaskFirst = false)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            if (unmaskedChars == 0)
            {
                return "********";
            }

            input = Clean(input);
            if (input.Length <= unmaskedChars)
            {
                return new string('*', input.Length);
            }
            else if (unmaskFirst)
            {
                return input.Substring(0, unmaskedChars) + new string('*', input.Length - unmaskedChars);
            }
            else
            {
                return new string('*', input.Length - unmaskedChars) + input.Substring(input.Length - unmaskedChars);
            }
        }

        public string Clean(string? input)
        {
            input = input ?? string.Empty;
            input = input.Replace("\r", " ").Replace("\n", " ");
            var idx = input.IndexOf("  ");
            while (idx > -1)
            {
                input = input.Replace("  ", " ");
                idx = input.IndexOf("  ");
            }
            input = _sanitize(input) ?? "";

            //unescape ' and " and space
            input = input.Replace("&#39;", "'");
            input = input.Replace("&#34;", "\"");
            input = input.Replace("&#20;", " ");
            input = input.Replace("&apos;", "'");
            input = input.Replace("&quot;", "\"");
            input = input.Replace("&nbsp;", " ");



            return input.Trim() ?? "";
        }
    }


    public class HtmlSanitizer : SanitizerBase, IHtmlSanitizer
    {
        public HtmlSanitizer() : base()
        {

        }
    }
    public class XmlSanitizer : SanitizerBase, IXmlSanitizer
    {
        public XmlSanitizer() : base(HttpUtility.HtmlEncode)
        {

        }
    }
    public class JsonSanitizer : SanitizerBase, IJsonSanitizer
    {
        public JsonSanitizer() : base(HttpUtility.JavaScriptStringEncode)
        {

        }

    }
    public class UrlSanitizer : SanitizerBase, IUrlSanitizer
    {
        public UrlSanitizer() : base(x => Uri.EscapeUriString(x?.ToString() ?? ""))
        {

        }
    }
    public class CssSanitizer : SanitizerBase, ICssSanitizer
    {
        public CssSanitizer() : base()
        {

        }
    }
    public class ScriptSanitizer : SanitizerBase, IScriptSanitizer
    {
        public ScriptSanitizer() : base(x => Uri.EscapeDataString(x?.ToString() ?? ""))
        {

        }
    }
    public class StyleSanitizer : SanitizerBase, IStyleSanitizer
    {
        public StyleSanitizer() : base()
        {

        }
    }
    public class SqlSanitizer : SanitizerBase, ISqlSanitizer
    {
        public SqlSanitizer() : base()
        {

        }
    }
    public class LogSanitizer : SanitizerBase, ILogSanitizer
    {
        public LogSanitizer() : base(HttpUtility.HtmlEncode)
        {

        }
        public override string? Sanitize(string? input, SanitizerMode mode)
        {
            if (input is null)
                return input;

            switch (mode)
            {
                case SanitizerMode.Debug:
                    return base.Mask(input, input.Length);
                case SanitizerMode.Clean:
                    return base.Clean(input);
                case SanitizerMode.Mask:
                    return base.Mask(input);
                case SanitizerMode.Full:
                    return base.Mask(input, 0);
                default:
                    throw new NotImplementedException();
            }
        }
    }
    public class SanitizerFactory : ISanitizerFactory
    {
        public TInputSanitizer Create<TInputSanitizer>() where TInputSanitizer : IInputSanitizer
        {

            var type = Enum.Parse<SanitizerType>(typeof(TInputSanitizer).Name.Substring(1));
            return (TInputSanitizer) Create(type);
        }

        public IInputSanitizer Create(SanitizerType type)
        {
            switch (type)
            {
                case SanitizerType.HtmlSanitizer:
                    return new HtmlSanitizer();
                case SanitizerType.XmlSanitizer:
                    return new XmlSanitizer();
                case SanitizerType.JsonSanitizer:
                    return new JsonSanitizer();
                case SanitizerType.UrlSanitizer:
                    return new UrlSanitizer();
                case SanitizerType.CssSanitizer:
                    return new CssSanitizer();
                case SanitizerType.ScriptSanitizer:
                    return new ScriptSanitizer();
                case SanitizerType.StyleSanitizer:
                    return new StyleSanitizer();
                case SanitizerType.SqlSanitizer:
                    return new SqlSanitizer();
                case SanitizerType.LogSanitizer:
                    return new LogSanitizer();
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public interface ISanitizer
    {
        public IHtmlSanitizer Html { get; }
        public IXmlSanitizer Xml { get; }
        public IJsonSanitizer Json { get; }
        public IUrlSanitizer Url { get; }
        public ICssSanitizer Css { get; }
        public IScriptSanitizer Script { get; }
        public IStyleSanitizer Style { get; }
        public ISqlSanitizer Sql { get; }
        public ILogSanitizer Log { get; }

    }
    public class Sanitizer : ISanitizer
    {
        public Sanitizer(ISanitizerFactory sanitizerFactory)
        {
            Html = sanitizerFactory.Create<IHtmlSanitizer>();
            Xml = sanitizerFactory.Create<IXmlSanitizer>();
            Json = sanitizerFactory.Create<IJsonSanitizer>();
            Url = sanitizerFactory.Create<IUrlSanitizer>();
            Css = sanitizerFactory.Create<ICssSanitizer>();
            Script = sanitizerFactory.Create<IScriptSanitizer>();
            Style = sanitizerFactory.Create<IStyleSanitizer>();
            Sql = sanitizerFactory.Create<ISqlSanitizer>();
            Log = sanitizerFactory.Create<ILogSanitizer>();
        }


        public IHtmlSanitizer Html { get; }
        public IXmlSanitizer Xml { get; }
        public IJsonSanitizer Json { get; }
        public IUrlSanitizer Url { get; }
        public ICssSanitizer Css { get; }
        public IScriptSanitizer Script { get; }
        public IStyleSanitizer Style { get; }
        public ISqlSanitizer Sql { get; }
        public ILogSanitizer Log { get; }
    }


}

namespace Microsoft.DependencyInjection.Extensions
{
    public static class RedirectUrlServiceExtensions
    {
        public static bool IsAllowedRedirect(this string redirectUrl)
        {
            return Ioc.RedirectService.IsRedirectAllowed(redirectUrl);
        }
        public static bool IsAllowedRedirect(this Uri uri)
        {
            return Ioc.RedirectService.IsRedirectAllowed(uri.ToString());
        }

        public static void RedirectIfAllowed(this HttpResponse response, string url)
        {
            if (IsAllowedRedirect(url))
                response.Redirect(url.SanitizeForRedirect());
            else
                SetRedirectNotAllowed(response);
        }

        public static void SetRedirectNotAllowed(this HttpResponse response)
        {
            response.StatusCode = (int) HttpStatusCode.Forbidden;
        }

        public static void SetRedirectNotAllowed(this HttpContext ctx)
        {
            ctx.Response.SetRedirectNotAllowed();
        }

    }
    public static class SanitizerServiceExtensions
    {
        public static IServiceCollection AddSanitizers(this IServiceCollection services)
        {
            var factory = new SanitizerFactory();
            services.AddSingleton(factory);
            services.AddSingleton<ISanitizerFactory>(factory);
            services.AddSingleton<ISanitizerService, SanitizerService>();
            services.AddSingleton<ISanitizer, Sanitizer>();

            var props = typeof(ISanitizer).GetProperties();
            foreach (var prop in props)
            {
                var type = Enum.Parse<SanitizerType>(prop.PropertyType.Name.Substring(1));
                var sanitizer = factory.Create(type);
                services.AddSingleton(prop.PropertyType, sanitizer);
                services.AddSingleton(sanitizer);
            }

            return services;
        }

        public static string? SanitizeForLog(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            return Ioc.Sanitizer.Log.Sanitize(input?.ToString(), mode);
        }

        public static string? SanitizeForHtml(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            return Ioc.Sanitizer.Html.Sanitize(input?.ToString(), mode);
        }

        public static string? SanitizeForXml(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            return Ioc.Sanitizer.Xml.Sanitize(input?.ToString(), mode);
        }

        public static string? SanitizeForJson(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            return Ioc.Sanitizer.Json.Sanitize(input?.ToString(), mode);
        }

        public static string SanitizeForRedirect(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            var urlString = input?.ToString() ?? "";
            if (string.IsNullOrEmpty(urlString))
                return urlString;
            else
            {
                if (Uri.TryCreate(urlString, UriKind.Absolute, out var uri))
                {
                    return uri.ToString().SanitizeForLog() ?? "";
                }
                else
                {
                    throw new ArgumentException("Invalid URL", nameof(input));
                }
            }
           
        }

        public static string? SanitizeForUrl(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            return Ioc.Sanitizer.Url.Sanitize(input?.ToString(), mode);
        }

        public static string? SanitizeForCss(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            return Ioc.Sanitizer.Css.Sanitize(input?.ToString(), mode);
        }

        public static string? SanitizeForScript(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            return Ioc.Sanitizer.Script.Sanitize(input?.ToString(), mode);
        }

        public static string? SanitizeForStyle(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            return Ioc.Sanitizer.Style.Sanitize(input?.ToString(), mode);
        }

        public static string? SanitizeForSql(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            return Ioc.Sanitizer.Sql.Sanitize(input?.ToString(), mode);
        }

        public static string? SantizeForRedirect(this object? input, SanitizerMode mode = SanitizerMode.Clean)
        {
            var decoded = Uri.UnescapeDataString(input?.ToString() ?? "");
            decoded.SanitizeForHtml();
            var escaped = Uri.EscapeDataString(decoded);
            return escaped;
        }

    }

}
