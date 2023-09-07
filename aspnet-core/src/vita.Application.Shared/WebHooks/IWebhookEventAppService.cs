﻿using System.Threading.Tasks;
using Abp.Webhooks;

namespace vita.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
