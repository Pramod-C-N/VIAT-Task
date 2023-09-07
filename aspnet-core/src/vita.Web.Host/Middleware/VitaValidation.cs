using Abp.Dependency;
using Abp.EntityFrameworkCore;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System;
using System.Threading.Tasks;
using vita.EntityFrameworkCore;
using JsonFlatten;
using System.Data.SqlClient;
using System.IO;
using vita.Sales.Dtos;
using Abp.UI;
using System.Net;
using Abp.Domain.Uow;
using System.Transactions;
using vita.Debit.Dtos;
using vita.Credit.Dtos;
using NPOI.SS.Formula.Functions;
using System.Linq;
using System.Collections.Generic;
using PayPalCheckoutSdk.Orders;
using vita.EInvoicing.Dto;
using System.Xml.Serialization;
using UblSharp;
using Tweetinvi.Core.Models;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using vita.Filters;
using k8s.KubeConfigModels;
using Tweetinvi.Models.V2;
using Twilio.TwiML.Messaging;
using ValidationFilter = vita.Filters.ValidationFilter;
using Abp.Timing.Timezone;
using static vita.Filters.VitaFilter_Validation;
using AutoMapper;

namespace vita.Web.Middleware
{
    public class VitaValidation : IMiddleware, ITransientDependency
    {
     

        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IMapper mapper;

        public IAbpSession AbpSession { get; set; }

        public VitaValidation(IDbContextProvider<vitaDbContext> dbContextProvider, IUnitOfWorkManager unitOfWorkManager, IAbpSession abpSession, ITimeZoneConverter timeZoneConverter,IMapper mapper)
        {
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;
            AbpSession = abpSession;
            _timeZoneConverter = timeZoneConverter;
            this.mapper = mapper;
        }

      


        public async System.Threading.Tasks.Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string message = null;
            List<VitaValidationDto> errors = null;
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<VitaFilter_Validation>();
            VitaFilter_ValidationType VitaFilter_ValidationType = VitaFilter_ValidationType.None ;
            if (attribute != null  && AbpSession.TenantId.HasValue)
            {
                VitaFilter_ValidationType = attribute.filter;
                string body = await new Shared().GetRequestBodyAsync(context.Request);

                if (VitaFilter_ValidationType == VitaFilter_Validation.VitaFilter_ValidationType.EinvoiceValidation)
                {
                    (message, errors) = await new ValidationFilter(_dbContextProvider, _unitOfWorkManager, AbpSession, _timeZoneConverter, mapper).EInvoiceAPIValidationAsync(body);
                    if (message != null)
                    {
                        await new Shared().UpdateResponseBodyAsync(context, 400, new
                        {
                            ValidationResults = new
                            {
                                Errors = errors
                            },
                            Message = message,
                            Status = "FAILED"
                        });
                    }
                    else
                    {
                        await next(context);
                    }
                }
                else if (VitaFilter_ValidationType == VitaFilter_Validation.VitaFilter_ValidationType.UnicoreValidation)
                {
                    (message, errors) = await new ValidationFilter(_dbContextProvider, _unitOfWorkManager, AbpSession, _timeZoneConverter, mapper).UnicoreAPIValidationAsync(body);
                    if (message != null)
                    {
                        await new Shared().UpdateResponseBodyAsync(context, 400, new
                        {
                            ValidationResults = new
                            {
                                Errors = errors
                            },
                            Message = message,
                            Status = "FAILED"
                        });
                    }
                    else
                    {
                        await next(context);
                    }
                }
                else if (VitaFilter_ValidationType == VitaFilter_Validation.VitaFilter_ValidationType.XmlValidation)
                {
                    string newbody = null;
                    (message, errors, newbody) = await new ValidationFilter(_dbContextProvider, _unitOfWorkManager, AbpSession, _timeZoneConverter, mapper).EInvoiceAPIValidationXMLAsync(body);
                    var requestData = Encoding.UTF8.GetBytes(newbody);
                    context.Request.Body = new MemoryStream(requestData);
                    context.Request.ContentLength = context.Request.Body.Length;
                    if (message != null)
                    {
                        await new Shared().UpdateResponseBodyAsync(context, 400, new
                        {
                            ValidationResults = new
                            {
                                Errors = errors
                            },
                            Message = message,
                            Status = "FAILED"
                        });
                    }
                    else
                    {
                        await next(context);
                    }
                }
                else if (VitaFilter_ValidationType == VitaFilter_Validation.VitaFilter_ValidationType.Debit)
                {
                    (message, errors) = await new ValidationFilter(_dbContextProvider, _unitOfWorkManager, AbpSession, _timeZoneConverter, mapper).DebitAPIValidationAsync(body);
                    if (message != null)
                    {
                        await new Shared().UpdateResponseBodyAsync(context, 400, new
                        {
                            ValidationResults = new
                            {
                                Errors = errors
                            },
                            Message = message,
                            Status = "FAILED"
                        });
                    }
                    else
                    {
                        await next(context);
                    }
                }
                else if (VitaFilter_ValidationType == VitaFilter_Validation.VitaFilter_ValidationType.Credit)
                {
                    (message, errors) = await new ValidationFilter(_dbContextProvider, _unitOfWorkManager, AbpSession, _timeZoneConverter, mapper).CreditAPIValidationAsync(body);
                   
                    if (message != null)
                    {
                        await new Shared().UpdateResponseBodyAsync(context, 400, new
                        {
                            ValidationResults = new
                            {
                                Errors = errors
                            },
                            Message = message,
                            Status = "FAILED"
                        });
                    }
                    else
                    {
                        await next(context);
                    }
                }
                else if (VitaFilter_ValidationType == VitaFilter_Validation.VitaFilter_ValidationType.Sales)
                {
                    (message, errors) = await new ValidationFilter(_dbContextProvider, _unitOfWorkManager, AbpSession, _timeZoneConverter, mapper).SalesAPIValidationAsync(body);
                   
                    if (message != null)
                    {
                        await new Shared().UpdateResponseBodyAsync(context, 400, new
                        {
                            ValidationResults = new
                            {
                                Errors = errors
                            },
                            Message = message,
                            Status = "FAILED"
                        });
                    }
                    else
                    {
                        await next(context);
                    }
                }
            }
            else
            {
                await next(context);
            }
            return;
        }

      

 

    }
}
