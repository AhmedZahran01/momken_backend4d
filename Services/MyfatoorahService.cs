using momken_backend.Dtos.Myfatoorah;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace momken_backend.Services
{
    public class MyfatoorahService: IMyfatoorahService
    {
        private readonly HttpClient _httpClient;
        private readonly MyfatoorahConfiguration _myfatoorahConfiguration;
        public MyfatoorahService(IConfiguration configuration)
        {
           _myfatoorahConfiguration=  configuration.GetSection("Myfatoorah").Get<MyfatoorahConfiguration>();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_myfatoorahConfiguration.BaseUrl),
              
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _myfatoorahConfiguration.Token);
        }
        public async Task<ExecutePaymentResBodyV2> GetUrlFromExecutePayment(string callBackUrl, string errorUrl, 
                                           int paymentMethodId, int invoiceValue, List<InvoiceItem> invoiceItems)
        {

            var response = await _httpClient.PostAsJsonAsync<ExecutePaymentV2>("/v2/ExecutePayment", new ExecutePaymentV2
            {
                CallBackUrl = callBackUrl,
                ErrorUrl = errorUrl,
                PaymentMethodId = paymentMethodId,
                InvoiceValue = invoiceValue,
                InvoiceItems = invoiceItems,
                ExpiryDate = DateTime.Now.AddHours(10).ToString("yyyy-MM-ddTHH:mm:ssZ")


            });
            var dataJson = await response.Content.ReadFromJsonAsync<ExecutePaymentResBodyV2>();
           return dataJson;
        }

      public async   Task<GetPaymentStatusV2ResDataDto> GetPaymentStatusV2WithInvoiceId(string invoiceId)
      {
            var response = await _httpClient.PostAsJsonAsync("/v2/GetPaymentStatus", new
                {
                 Key= invoiceId,
                 KeyType= "InvoiceId"

                });
            var dataJson = await response.Content.ReadFromJsonAsync<GetPaymentStatusV2ResDataDto>();
            return dataJson;
            
      }
    }

    public interface IMyfatoorahService
    {
        Task<ExecutePaymentResBodyV2> GetUrlFromExecutePayment(string callBackUrl,string errorUrl,int paymentMethodId,int invoiceValue, List<InvoiceItem> invoiceItems);

        Task<GetPaymentStatusV2ResDataDto> GetPaymentStatusV2WithInvoiceId(string invoiceId);
    }
}
