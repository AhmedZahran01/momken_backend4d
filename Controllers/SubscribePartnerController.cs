using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using momken_backend.Data;
using momken_backend.Dtos;
using momken_backend.Dtos.DataRespons;
using momken_backend.Dtos.Myfatoorah;
using momken_backend.Models;
using momken_backend.Services;
using System;
using System.Data;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace momken_backend.Controllers
{
    [Route("api/v1/subscribe_partner")]
    [ApiController]
    public class SubscribePartnerController : ControllerBase
    {
        private readonly IMyfatoorahService _myfatoorahService ;
        private readonly MyfatoorahConfiguration _myfatoorahConfiguration;
        private readonly AppDbContext _context;

        public SubscribePartnerController(IMyfatoorahService myfatoorahService,ITokenService tokenService,
                                             IConfiguration configuration, AppDbContext context)
        {
            _myfatoorahService = myfatoorahService;
            _myfatoorahConfiguration = configuration.GetSection("Myfatoorah").Get<MyfatoorahConfiguration>();
            _context = context;
        }

        [HttpPost("order")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Order([FromBody] SubscribeOrderDto subscribeOrderDto)
        {
            if (subscribeOrderDto.MonthCount < 1)
            {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "MonthCount Min is 1"
                });
            }
            
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null | nameIdentifierClaim.Value == null)
            {
                return Unauthorized(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "Not valid Token"
                });
            }

            var priceForMonth = 10;
         var dataTemp=   await _context.MyfatoorahTempDatas.AddAsync(new Models.MyfatoorahTempData
            {
                Content=new SubscribeOrderTempData
                {
                    InvoiceId = "",
                    userId= Guid.Parse(nameIdentifierClaim.Value),
                    MonthCount= subscribeOrderDto.MonthCount
                }
            });
           //var token = _tokenService.GenerateToken(Guid.Parse(nameIdentifierClaim.Value), subscribeOrderDto.MonthCount, _myfatoorahConfiguration.SingningKeyOrder);
            var callBackUrl = Url.Link("OrderSuccess", new {id= dataTemp.Entity.Id});
            var errorUrl = Url.Link("OrderSuccess", new {id= dataTemp.Entity.Id});
            var MyfaRes = await _myfatoorahService.GetUrlFromExecutePayment(callBackUrl, errorUrl, subscribeOrderDto.paymentMethodId, subscribeOrderDto.MonthCount* priceForMonth, new List<InvoiceItem>
            {
                     new InvoiceItem
                     {
                            ItemName = "Subscribe Partner",
                        Quantity = subscribeOrderDto.MonthCount,
                        UnitPrice = priceForMonth
                     }
            });
            dataTemp.Entity.Content = new SubscribeOrderTempData
            {
                InvoiceId = MyfaRes.Data.InvoiceId.ToString(),
                userId = Guid.Parse(nameIdentifierClaim.Value),
                MonthCount = subscribeOrderDto.MonthCount
            };
          await  _context.SaveChangesAsync();
            return Ok(new GlobalResponseDebugDto<ResBodyMyFaDto, ExecutePaymentResBodyV2>
            {
                success = true,
               message= "paymint url with myfatora",
               data=new ResBodyMyFaDto
               {
                   PaymentURL=MyfaRes.Data.PaymentURL.ToString()
               },
               debug=MyfaRes
            });
        }
        [HttpGet("order/success/{id}",Name = "OrderSuccess")]
        public async Task<IActionResult> OrderSuccess(Guid id)
        {
            try
            {
                var data = await _context.MyfatoorahTempDatas.FirstOrDefaultAsync(d => d.Id == id);
                if (data == null)
                {
                    return Unauthorized(new GlobalResponseNoBodyDebugDto<List<MyfatoorahTempData>>
                    {
                       success=false,
                       message= "Token Unauthorized",
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
                        debug=$"myFatoura Status  {myFaResData.Data.InvoiceStatus}"
                    });
                }
                var dataInvoiceTransactions = myFaResData.Data.InvoiceTransactions.FirstOrDefault();
                if (dataInvoiceTransactions == null)
                {
                    return Unauthorized(new GlobalResponseNoBodyDebugDto<GetPaymentStatusV2ResDataDto>
                    {
                        success = false,
                        message = "Token Unauthorized",
                        debug= myFaResData
                    });
                }
                var paymentGateway = dataInvoiceTransactions.PaymentGateway;
                var paidCurrency = dataInvoiceTransactions.PaidCurrency;
                var country = dataInvoiceTransactions.Country;
              var subscribePartnerFind = await _context.SubscribePartner.OrderByDescending(s=>s.StartFrom).FirstOrDefaultAsync(s=>s.PartnerId==data.Content.userId);
                var dateNow = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time")));

                var dateExpier = subscribePartnerFind?.StartFrom.AddDays(subscribePartnerFind.MonthCount*31);
                Models.SubscribePartner newSubscribePartnerEntity;
                if (dateExpier == null || dateExpier < dateNow)
                {
                    var newSubscribePartner = await _context.SubscribePartner.AddAsync(new Models.SubscribePartner
                    {
                        country = country,
                        currency = paidCurrency,
                        InvoiceId = data.Content.InvoiceId.ToString(),
                        PartnerId = data.Content.userId,
                        MonthCount = (int)data.Content.MonthCount,
                        amount = dataInvoiceTransactions.TransationValue,
                    paymentGateway = paymentGateway
                    });
                    newSubscribePartnerEntity = newSubscribePartner.Entity;
                }
                else
                {
                    var newSubscribePartner = await _context.SubscribePartner.AddAsync(new Models.SubscribePartner
                    {
                        country = country,
                        currency = paidCurrency,
                        InvoiceId = data.Content.InvoiceId.ToString(),
                        PartnerId = data.Content.userId,
                        MonthCount = (int)data.Content.MonthCount,
                        amount = dataInvoiceTransactions.TransationValue,
                        paymentGateway = paymentGateway,
                        StartFrom= dateExpier ?? DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time")))
                    });
                    newSubscribePartnerEntity = newSubscribePartner.Entity;
                }
                var usrId = data.Content.userId;
                var monthCount = data.Content.MonthCount;
                 _context.MyfatoorahTempDatas.Remove(data);
                await _context.SaveChangesAsync();
              var subscribePartner = await  _context.SubscribePartner.Include(s=>s.partner).FirstOrDefaultAsync(s=>s.Id== newSubscribePartnerEntity.Id);
              var allSubNotExpier =  await _context.SubscribePartner.Where(s => s.StartFrom.AddDays(s.MonthCount*31)> dateNow&&s.PartnerId== usrId).OrderByDescending(s => s.StartFrom).FirstOrDefaultAsync();
            int countOfDayBeforExpiry;
                DateTime expierAt;
            if (allSubNotExpier != null)
            {
                    expierAt = allSubNotExpier.StartFrom.AddDays(allSubNotExpier.MonthCount * 31).ToDateTime(TimeOnly.MinValue);
                    countOfDayBeforExpiry = (expierAt - dateNow.ToDateTime(TimeOnly.MinValue)).Days;

            }
            else
            {
                countOfDayBeforExpiry = monthCount*31;
                    expierAt = DateTime.UtcNow.AddDays(countOfDayBeforExpiry);
            }

            if(subscribePartner ==null)
                {
                    return Ok(new GlobalResponseDebugDto<SubscribePartnerOrderSuccessDto,string>
                    {
                        success = true,
                        message = $"success paymint month {monthCount}",
                        data = new SubscribePartnerOrderSuccessDto
                        {
                            countOfDayBeforExpiry = countOfDayBeforExpiry,
                            amount= newSubscribePartnerEntity.amount,
                            MonthCount= monthCount,
                            CreatedAt= newSubscribePartnerEntity.CreatedAt,
                            currency= newSubscribePartnerEntity.currency,
                            OrderId=1101+(int)newSubscribePartnerEntity.OrderId,
                            paymentGateway= newSubscribePartnerEntity.paymentGateway,
                            StartFrom = newSubscribePartnerEntity.StartFrom,
                            ExpierAt = expierAt,

                        },
                        debug = "subscribePartner is Null"
                    });
                }
                return Ok(new GlobalResponseDebugDto<SubscribePartnerOrderSuccessDto,Models.SubscribePartner> {
                    success= true,
                    message=$"success paymint month {monthCount}",
                    data= new SubscribePartnerOrderSuccessDto
                    {
                        amount = newSubscribePartnerEntity.amount,
                        CreatedAt = newSubscribePartnerEntity.CreatedAt,
                        currency = newSubscribePartnerEntity.currency,
                        ExpierAt = expierAt,
                        MonthCount = newSubscribePartnerEntity.MonthCount,
                        OrderId = newSubscribePartnerEntity.OrderId,
                        paymentGateway = newSubscribePartnerEntity.paymentGateway,
                        StartFrom = newSubscribePartnerEntity.StartFrom,
                        countOfDayBeforExpiry = countOfDayBeforExpiry,
                    },
                    debug= subscribePartner
                });
                
            }
            catch (Exception e)
            {
                return Unauthorized("Token Unauthorized" + e.InnerException + e.Message);
            }

        }
        [HttpGet("order/fail/{id}", Name = "Order")]
        public IActionResult OrderFail(string id)
        {
            try
            {

                
                return Ok(new GlobalResponseNoDataDto
                {
                    success=false,
                    message= "Order Fail"
                });
            }
            catch (Exception e)
            {
                return Unauthorized("Token Unauthorized");
            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("order")]
        public async Task<IActionResult> GetAllOrder() {
           var NameIdentifier = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (NameIdentifier == null) { 
                return Unauthorized();
            }
            var userId = Guid.Parse(NameIdentifier.Value);
            var allOrders = await _context.SubscribePartner.Where(s => s.PartnerId == userId).Select(s=>new SubscribePartnerOrderGetAllDto
            {
                Id = s.Id,
               CreatedAt=s.CreatedAt,
               OrderId =1101+(int) s.OrderId,
               amount=s.amount
              
            }).ToListAsync();

            return Ok(new GlobalResponseDebugDto<List<SubscribePartnerOrderGetAllDto>,string> {
                success=true,
                message="All orders",
                data=allOrders,
                debug="No data to debug"
            });
        } 
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOneOrder(Guid id) {
           var NameIdentifier = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (NameIdentifier == null) { 
                return Unauthorized();
            }
            var userId = Guid.Parse(NameIdentifier.Value);
            var subscribePartnerFind = await _context.SubscribePartner.Where(s=>s.Id==id).FirstOrDefaultAsync(s => s.PartnerId == userId);
            var dateNow = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time")));

            if (subscribePartnerFind == null) { 
            return NotFound(new GlobalResponseNoDataDto
            {
                success= false,
                message= "Not found"
            });
            }
            //var subscribePartner = await _context.SubscribePartner.Include(s => s.partner).FirstOrDefaultAsync(s => s.Id == id);
            //var allSubNotExpier = await _context.SubscribePartner.Where(s => s.StartFrom.AddDays(s.MonthCount * 30) > dateNow && s.PartnerId == userId).OrderByDescending(s => s.StartFrom).FirstOrDefaultAsync();
            int countOfDayBeforExpiry;
            var startFrom = subscribePartnerFind.StartFrom;
               var  expierAt = startFrom.AddDays(subscribePartnerFind.MonthCount * 31).ToDateTime(TimeOnly.MinValue);
                countOfDayBeforExpiry = (expierAt - dateNow.ToDateTime(TimeOnly.MinValue)).Days;


           
            return Ok(new GlobalResponseDebugDto<SubscribePartnerOrderSuccessDto,string>
            {
                success = true,
                message="Order data",
                data = new SubscribePartnerOrderSuccessDto
                {
                     amount= subscribePartnerFind.amount,
                     countOfDayBeforExpiry= countOfDayBeforExpiry,
                     CreatedAt= subscribePartnerFind.CreatedAt ,
                     currency=subscribePartnerFind.currency,
                     ExpierAt= expierAt,
                     MonthCount= subscribePartnerFind.MonthCount,
                     OrderId= 1101+(int) subscribePartnerFind.OrderId,
                     paymentGateway= subscribePartnerFind.paymentGateway,
                     StartFrom = subscribePartnerFind.StartFrom

                }
            });

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("status")]
        public async Task<IActionResult> GetUserStatus()
        {
            var NameIdentifier = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (NameIdentifier == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(NameIdentifier.Value);
            var subscribePartnerFind = await _context.SubscribePartner.OrderByDescending(s=>s.CreatedAt).FirstOrDefaultAsync(s => s.PartnerId == userId);
            var dateNow = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time")));

            int countOfDayBeforExpiry;
            DateOnly startFrom;
            DateTime? expierAt=null;
            string expierAtDateString = "لم يبداء الاشتراك بعد";
            string status;
            if (subscribePartnerFind == null)
            {
                 status = Enums.SubscribePartnerStatus.Pending.ToString().ToLower();
                countOfDayBeforExpiry = 0;
            }
            else
            {
                 status = Enums.SubscribePartnerStatus.Active.ToString().ToLower();
             startFrom = subscribePartnerFind.StartFrom;
             expierAt = startFrom.AddDays(subscribePartnerFind.MonthCount * 31).ToDateTime(TimeOnly.MinValue);
            countOfDayBeforExpiry = (expierAt - dateNow.ToDateTime(TimeOnly.MinValue))?.Days??0;
                expierAtDateString = expierAt?.ToString("yyyy/MM/dd");
            }

            if (countOfDayBeforExpiry <= 0 && expierAt == null)
            {
                countOfDayBeforExpiry = 0;
                status = Enums.SubscribePartnerStatus.Pending.ToString().ToLower();
            }
            else if(countOfDayBeforExpiry ==0 && expierAt != null)
            {
                status = Enums.SubscribePartnerStatus.Finished.ToString().ToLower();
            }
            else if (countOfDayBeforExpiry <= 15)
            {

                status = Enums.SubscribePartnerStatus.Almost.ToString().ToLower();
            }

       


            return Ok(new GlobalResponseDebugDto<SubscribePartnerStatusDto, string>
            {
                success = true,
                message = "Order data",
                data = new SubscribePartnerStatusDto
                {
                 DateExpiry= expierAtDateString ?? "لم يبداء الاشتراك بعد",
                 DayCountBeforExpiry=countOfDayBeforExpiry,
                 Status= status

                }
            });


        }
    }
}
