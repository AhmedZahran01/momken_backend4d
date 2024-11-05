using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace momken_backend.Services
{
    public class PdfService
    {
            private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public PdfService(IConverter converter, ICompositeViewEngine viewEngine,
                                     ITempDataProvider tempDataProvider,
                                     IServiceProvider serviceProvider)
    {
        _converter = converter;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

    public byte[] GeneratePdf(string htmlContent)
    {
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings =
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
            Objects =
            {
                new ObjectSettings
                {
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
        };
            return _converter.Convert(doc);
    }
        public async Task<string> RenderViewToStringAsync(ControllerContext context, string viewName, object model)
        {
            var viewPath = Path.Combine("Views", "Pdf.cshtml");
            //var  System.IO.File.Exists(viewPath);
            var viewEngineResult = _viewEngine.GetView( "", viewPath, false);
            //var viewEngineResult2 = _viewEngine.GetView("", "Pdf", false);

            if (!viewEngineResult.Success)
            {
                throw new FileNotFoundException($"The view '{viewName}' was not found.");
            }

            var view = viewEngineResult.View;

            using (var stringWriter = new StringWriter())
            {
                var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var tempData = new TempDataDictionary(context.HttpContext, _tempDataProvider);

                var viewContext = new ViewContext(
                    context,
                    view,
                    viewData,
                    tempData,
                    stringWriter,
                    new HtmlHelperOptions()
                );

                await view.RenderAsync(viewContext);

                return stringWriter.ToString();
            }
        }


















    }
}
