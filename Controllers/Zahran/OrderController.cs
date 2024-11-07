using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using momken_backend.Data;
using momken_backend.Dtos.DataRespons;
using momken_backend.Dtos.Myfatoorah;
using momken_backend.Dtos.Zahran;
using momken_backend.Models;
using momken_backend.Services;
using System.Security.Claims;

namespace momken_backend.Controllers.Zahran
{
    [Route("api/v1/client/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "client")]
    public class OrderController : ControllerBase
    {
        #region Constructor Region

        private readonly AppDbContext _context;
        private readonly IMyfatoorahService _myfatoorahService;
        private readonly MyfatoorahConfiguration _myfatoorahConfiguration;
        public OrderController(AppDbContext context, IMyfatoorahService myfatoorahService, IConfiguration configuration)
        {
            _context = context;
            _myfatoorahService = myfatoorahService;
            _myfatoorahConfiguration = configuration.GetSection("Myfatoorah").Get<MyfatoorahConfiguration>();
            _context = context;
        }


        #endregion


        #region Make Order Region


        [HttpPost("MakeOrder")]
        public async Task<IActionResult> MakeOrder([FromBody] List<OrderCartClientDto> clientOrder, int paymentMethodId)
        {
            decimal Total = 0;

            #region client ID Region

            string? findStringClientId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (findStringClientId == null)
            {
                return Unauthorized(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "Not valid Token"
                });
            }


            Guid? findGuidClientId = Guid.Parse(findStringClientId);
            var clientcheckId = await _context.Clients
               .FirstOrDefaultAsync(p => p.Id == findGuidClientId);

            #endregion

            #region Cart Data and calculate total Price Of Order

            if (clientOrder != null)
            {

                var products = new List<Product>();
                foreach (var item in clientOrder)
                {
                    int productsPrice = await _context.Products.Select(p => p.Price).FirstOrDefaultAsync();

                    products = await _context.Products.Where(p => p.Id == item.Id).ToListAsync();
                    Total += productsPrice * item.Quantity;
                }

                if (Total > 0)
                {
                    OrdersClient AddedordersClient = new OrdersClient() { OrderTotalPrice = Total, OrderproductsWithItsQuantity = products };
                    _context.Set<OrdersClient>().Add(AddedordersClient);
                    await _context.SaveChangesAsync();

                    #region Check respone Region
                    
                    //return Ok(new GlobalResponseDebugDto<List<Product>, string>
                    //{
                    //    success = true,
                    //    message = $"Get products Data and total is {Total}",
                    //    data = products,
                    //    debug = "No data for debug"
                    //}); 
                   
                    #endregion
                }
            }
            else
            {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "There is no products in The Cart "
                });

            }


            #endregion

            #region Myfatoorah payment Region


            var dataTemp = await _context.MyfatoorahTempDatas.AddAsync(new Models.MyfatoorahTempData
            {
                Content = new SubscribeOrderTempData
                {
                    InvoiceId = "",
                    userId = Guid.Parse(findStringClientId),

                }
            });

            var callBackUrl = Url.Link("OrderSuccessClient", new { id = dataTemp.Entity.Id });
            var errorUrl = Url.Link("OrderFailClient", new { id = dataTemp.Entity.Id });
            var MyfaRes = await _myfatoorahService.GetUrlFromExecutePayment(callBackUrl, errorUrl, paymentMethodId, Total,
                new List<InvoiceItem>
                {
                            new InvoiceItem
                            {
                               ItemName = "Subscribe Partner",
                               Quantity =   5,
                               UnitPrice = 250
                            }

                });

            #endregion

            dataTemp.Entity.Content = new SubscribeOrderTempData
            {
                InvoiceId = MyfaRes.Data.InvoiceId.ToString(),
                userId = Guid.Parse(findStringClientId),

            };

            await _context.SaveChangesAsync();

            return Ok(new GlobalResponseDebugDto<ResBodyMyFaDto, ExecutePaymentResBodyV2>
            {
                success = true,
                message = "paymint url with myfatora",
                data = new ResBodyMyFaDto
                {
                    PaymentURL = MyfaRes.Data.PaymentURL.ToString()
                },
                debug = MyfaRes
            });


        }

        #endregion


        #region OrderSuccess Region




        [HttpGet("order/client/success/{id}", Name = "OrderSuccessclient")]
        public async Task<IActionResult> OrderSuccessClient(Guid id)
        {
            try
            {
                var data = await _context.MyfatoorahTempDatas.FirstOrDefaultAsync(d => d.Id == id);
                if (data == null)
                {
                    return Unauthorized(new GlobalResponseNoBodyDebugDto<List<MyfatoorahTempData>>
                    {
                        success = false,
                        message = "Token Unauthorized",
                        debug = await _context.MyfatoorahTempDatas.ToListAsync()
                    });
                }

                var myFaResData = await _myfatoorahService.GetPaymentStatusV2WithInvoiceId(data.Content.InvoiceId);
                var isPaid = myFaResData.Data.InvoiceStatus == "Paid";

                if (!isPaid)
                {
                    return Unauthorized(new GlobalResponseNoBodyDebugDto<string>
                    {
                        success = false,
                        message = "Token Unauthorized",
                        debug = $"myFatoura Status  {myFaResData.Data.InvoiceStatus}"
                    });
                }


                var dataInvoiceTransactions = myFaResData.Data.InvoiceTransactions.FirstOrDefault();
                if (dataInvoiceTransactions == null)
                {
                    return Unauthorized(new GlobalResponseNoBodyDebugDto<GetPaymentStatusV2ResDataDto>
                    {
                        success = false,
                        message = "Token Unauthorized",
                        debug = myFaResData
                    });
                }


                var paymentGateway = dataInvoiceTransactions.PaymentGateway;
                var paidCurrency = dataInvoiceTransactions.PaidCurrency;
                var country = dataInvoiceTransactions.Country;



                return Ok(new GlobalResponseDebugDto<SubscribePartnerOrderSuccessDto, Models.SubscribePartner>
                {
                    success = true,
                    message = $"success paymint month  ",
                    data = new SubscribePartnerOrderSuccessDto
                    {

                    }

                });

            }
            catch (Exception e)
            {
                return Unauthorized("Token Unauthorized" + e.InnerException + e.Message);
            }

        }


        #endregion


        #region Order Fail Region


        [HttpGet("order/client/fail/{id}", Name = "OrderFailClient")]
        public IActionResult OrderFailClient(string id)
        {
            try
            {


                return Ok(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "Order Fail"
                });
            }
            catch (Exception e)
            {
                return Unauthorized("Token Unauthorized");
            }

        }


        #endregion
    }

    #region Comment
//    public class OrderController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public OrderController(AppDbContext context)
//        {
//            _context = context;
//        }

//        #region Make Order Region


//        [HttpPost("MakeOrder")]
//        public async Task<IActionResult> MakeOrder([FromBody] List<OrderCartClientDto> clientOrder)
//        {

//            #region client ID Region

//            string? findStringClientId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

//            Guid? findGuidClientId = Guid.Parse(findStringClientId);
//            var clientcheckId = await _context.Clients
//               .FirstOrDefaultAsync(p => p.Id == findGuidClientId);

//            #endregion


//            if (clientOrder != null)
//            {
//                decimal Total = 0;
//                var products = new List<Product>();
//                foreach (var item in clientOrder)
//                {
//                    products = await _context.Products.Where(p => p.Id == item.Id).ToListAsync();
//                    Total += item.ProductPrice * item.Quantity;
//                }

//                if (Total > 0)
//                {
//                    OrdersClient AddedordersClient = new OrdersClient() { OrderTotalPrice = Total, OrderproductsWithItsQuantity = products };
//                    _context.Set<OrdersClient>().Add(AddedordersClient);
//                    await _context.SaveChangesAsync();
//                    return Ok(new GlobalResponseDebugDto<List<Product>, string>
//                    {
//                        success = true,
//                        message = $"Get products Data and total is {Total}",
//                        data = products,
//                        debug = "No data for debug"
//                    });
//                }
//            }

//            return BadRequest(new GlobalResponseNoDataDto
//            {
//                success = false,
//                message = "There is no products in The Cart "
//            });

//        }


//        #endregion


//    }
//}

#endregion
}

