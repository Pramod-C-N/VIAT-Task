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
using static vita.Filters.VitaFilter_Authorization;

namespace vita.Web.Middleware
{
    public class VitaAuthorization : IMiddleware, ITransientDependency
    {
     

        private readonly IDbContextProvider<vitaDbContext> _dbContextProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITimeZoneConverter _timeZoneConverter;
        public IAbpSession AbpSession { get; set; }

        public VitaAuthorization(IDbContextProvider<vitaDbContext> dbContextProvider, IUnitOfWorkManager unitOfWorkManager, IAbpSession abpSession, ITimeZoneConverter timeZoneConverter)
        {
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;
             AbpSession = abpSession;
            _timeZoneConverter = timeZoneConverter;
        }


        public async System.Threading.Tasks.Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<VitaFilter_Authorization>();
            VitaFilter_ModuleName vitaFilter_ModuleName = VitaFilter_ModuleName.None ;
            if (attribute != null  && AbpSession.TenantId.HasValue)
            {
                vitaFilter_ModuleName = attribute.module;
               
                    var isAuthorized = await new AuthorizationFilter(_dbContextProvider,_unitOfWorkManager,AbpSession, _timeZoneConverter).CheckIsAuthorized(vitaFilter_ModuleName);
                    if (!isAuthorized)
                    {
                        await new Shared().UpdateResponseBodyAsync(context, 401, new
                        {
                           
                            Message = "Please upgrade your subscription to access this page",
                        });
                    }
                    else
                    {
                        await next(context);
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
