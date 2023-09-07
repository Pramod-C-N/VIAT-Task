using System;
using System.Collections.Generic;
using System.Text;
using vita.Sales.Dtos;
using UblSharp;
using Abp.Application.Services;
using vita.Credit.Dtos;
using vita.Debit.Dtos;
using System.Threading.Tasks;
using vita.UblSharp.Dtos;

namespace vita.UblSharp
{
    public interface IGenerateXmlAppService :IApplicationService
    {
        Task<bool> GenerateXmlRequest<T>(T input, XMLRequestParam param);
      
    }
}
