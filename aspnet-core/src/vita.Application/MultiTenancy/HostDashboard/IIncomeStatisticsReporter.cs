using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vita.MultiTenancy.HostDashboard.Dto;

namespace vita.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}