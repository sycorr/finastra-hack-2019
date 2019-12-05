using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace Finastra.Hackathon.Platform
{
    public class RazorEngine
    {
        protected readonly IRazorEngineService RazorEngineService;
        private readonly IList<string> _cachedFilePaths = new List<string>();

        public RazorEngine()
        {
            var emailLayoutTemplate = GetManifestResource("Finastra.Hackathon.Emails.Templates.EmailLayout.cshtml");
            var pdfLayoutTemplate = GetManifestResource("Finastra.Hackathon.Reports.Templates.ReportLayout.cshtml");

            var config = new TemplateServiceConfiguration()
            {
                DisableTempFileLocking = true,
                CachingProvider = new DefaultCachingProvider(t => { _cachedFilePaths.Add(t); }),
                EncodedStringFactory = new RawStringFactory()
            };

            RazorEngineService = global::RazorEngine.Templating.RazorEngineService.Create(config);

            RazorEngineService.AddTemplate("EmailLayout", emailLayoutTemplate);
            RazorEngineService.AddTemplate("ReportLayout", pdfLayoutTemplate);
        }

        ~RazorEngine()
        {
            Cleanup();

            try
            {
                RazorEngineService.Dispose();
            }
            catch{ }
        }

        private static readonly RazorEngine Current = new RazorEngine();

        public static void AddTemplate(string templateId, string templateSource)
        {
            if (!Current.RazorEngineService.IsTemplateCached(templateId, null))
            {
                Current.RazorEngineService.AddTemplate(templateId, templateSource);
                Current.RazorEngineService.Compile(templateId);
            }
        }

        public static string Run(string templateId, object model)
        {
            if (!Current.RazorEngineService.IsTemplateCached(templateId, null))
                throw new ArgumentException("The razor template " + templateId + " is missing from cache. Call AddTemplate before making a Run call.");

            Current.Cleanup();

            return Current.RazorEngineService.Run(templateId, null, model, null);
        }

        protected void Cleanup()
        {
            foreach (var cfp in _cachedFilePaths)
            {
                try
                {
                    Directory.Delete(cfp, true);
                }
                catch { }
            }

            _cachedFilePaths.Clear();
        }

        private static string GetManifestResource(string path)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            };
        }
    }
}