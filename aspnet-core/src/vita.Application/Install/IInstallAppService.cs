﻿using System.Threading.Tasks;
using Abp.Application.Services;
using vita.Install.Dto;

namespace vita.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}