using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Abp;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Notifications;
using vita.Authorization.Users;
using vita.MultiTenancy;

namespace vita.Notifications
{
    public class AppNotifier : vitaDomainServiceBase, IAppNotifier, ITransientDependency
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public AppNotifier(INotificationPublisher notificationPublisher, IUnitOfWorkManager unitOfWorkManager)
        {
            _notificationPublisher = notificationPublisher;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task WelcomeToTheApplicationAsync(User user)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.WelcomeToTheApplication,
                new MessageNotificationData(L("WelcomeToTheApplicationNotificationMessage")),
                severity: NotificationSeverity.Success,
                userIds: new[] {user.ToUserIdentifier()}
            );
        }
        public Task SomeInvoicesCouldntBeImported(UserIdentifier user, string fileToken, string fileType, string fileName)
        {
            return SendNotificationAsync(AppNotificationNames.DownloadInvalidImportUsers, user,
                new LocalizableString(
                    "ClickToSeeInvalidInvoices",
                    vitaConsts.LocalizationSourceName
                ),
                new Dictionary<string, object>
                {
                    { "fileToken", fileToken },
                    { "fileType", fileType },
                    { "fileName", fileName }
                });

        }

        public async Task NewUserRegisteredAsync(User user)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewUserRegisteredNotificationMessage",
                    vitaConsts.LocalizationSourceName
                )
            );

            notificationData["userName"] = user.UserName;
            notificationData["emailAddress"] = user.EmailAddress;

            await _notificationPublisher.PublishAsync(AppNotificationNames.NewUserRegistered, notificationData,
                tenantIds: new[] {user.TenantId});
        }

        public async Task NewTenantRegisteredAsync(Tenant tenant)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewTenantRegisteredNotificationMessage",
                    vitaConsts.LocalizationSourceName
                )
            );

            notificationData["tenancyName"] = tenant.TenancyName;
            await _notificationPublisher.PublishAsync(AppNotificationNames.NewTenantRegistered, notificationData);
        }

        public async Task GdprDataPrepared(UserIdentifier user, Guid binaryObjectId)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "GdprDataPreparedNotificationMessage",
                    vitaConsts.LocalizationSourceName
                )
            );

            notificationData["binaryObjectId"] = binaryObjectId;

            await _notificationPublisher.PublishAsync(AppNotificationNames.GdprDataPrepared, notificationData,
                userIds: new[] {user});
        }

        //This is for test purposes
        public async Task SendMessageAsync(UserIdentifier user, string message,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.SimpleMessage,
                new MessageNotificationData(message),
                severity: severity,
                userIds: new[] {user}
            );
        }
        
        public async Task SendMessageAsync(string notificationName, string message, UserIdentifier[] userIds = null,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            var tenants = NotificationPublisher.AllTenants;
            await _notificationPublisher.PublishAsync(
                notificationName : notificationName,
                new MessageNotificationData(message),
                severity: severity,
                userIds: userIds
            );
        }

        public Task SendMessageAsync(UserIdentifier user, LocalizableString localizableMessage,
            IDictionary<string, object> localizableMessageData = null,
            NotificationSeverity severity = NotificationSeverity.Info)
        {

                return SendNotificationAsync(AppNotificationNames.SimpleMessage, user, localizableMessage,
                localizableMessageData, severity);

        }

        protected async Task SendNotificationAsync(string notificationName, UserIdentifier user,
            LocalizableString localizableMessage, IDictionary<string, object> localizableMessageData = null,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
        using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
        {
            var notificationData = new LocalizableMessageNotificationData(localizableMessage);
            if (localizableMessageData != null)
            {
                foreach (var pair in localizableMessageData)
                {
                    notificationData[pair.Key] = pair.Value;
                }
            }

            await _notificationPublisher.PublishAsync(notificationName, notificationData, severity: severity,
                userIds: new[] { user });
           await     uow.CompleteAsync();
        }
        }

        public Task TenantsMovedToEdition(UserIdentifier user, string sourceEditionName, string targetEditionName)
        {
            return SendNotificationAsync(AppNotificationNames.TenantsMovedToEdition, user,
                new LocalizableString(
                    "TenantsMovedToEditionNotificationMessage",
                    vitaConsts.LocalizationSourceName
                ),
                new Dictionary<string, object>
                {
                    {"sourceEditionName", sourceEditionName},
                    {"targetEditionName", targetEditionName}
                });
        }

        public Task<TResult> TenantsMovedToEdition<TResult>(UserIdentifier argsUser, int sourceEditionId,
            int targetEditionId)
        {
            throw new NotImplementedException();
        }

        public Task SomeUsersCouldntBeImported(UserIdentifier user, string fileToken, string fileType, string fileName)
        {
            return SendNotificationAsync(AppNotificationNames.DownloadInvalidImportUsers, user, 
                new LocalizableString(
                    "ClickToSeeInvalidUsers",
                    vitaConsts.LocalizationSourceName
                ), 
                new Dictionary<string, object>
                {
                    { "fileToken", fileToken },
                    { "fileType", fileType },
                    { "fileName", fileName }
                });
        }

        public async Task SendMassNotificationAsync(string message, UserIdentifier[] userIds = null,
             NotificationSeverity severity = NotificationSeverity.Info,
            Type[] targetNotifiers = null
        )
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.MassNotification,
                new MessageNotificationData(message),
                severity: severity,
                userIds: userIds,
                targetNotifiers: targetNotifiers
            );
        }
    }
}
