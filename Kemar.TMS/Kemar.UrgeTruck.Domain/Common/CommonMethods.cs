using Kemar.UrgeTruck.Domain.ResponseModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kemar.UrgeTruck.Domain.Common
{
    public static class ResultCode
    {
        public static int Success = 200;
        public static int SuccessfullyUpdated = 204;
        public static int SuccessfullyCreated = 201;
        public static int NotAllowed = 405;
        public static int Invalid = 400;
        public static int Unauthorized = 401;
        public static int ExceptionThrown = 202;
        public static int DuplicateRecord = 409;
        public static int RecordNotFound = 404;
    }

    public class CommonMethods
    {
        private static ConcurrentDictionary<string, ApplicationConfigurationResponse> _appConfig =
             new ConcurrentDictionary<string, ApplicationConfigurationResponse>(StringComparer.OrdinalIgnoreCase);

        public static string GetVehilceTransactionType(int tranType)
        {
            string transactionType = string.Empty;

            return transactionType;
        }

        public static void AddNewAppConfigItem(List<ApplicationConfigurationResponse> data)
        {
            _appConfig.Clear();
            data.ForEach(x => _appConfig.TryAdd(x.AppConfigId.ToString(), x));
        }

        public static void UpdateAppConfigItem(ApplicationConfigurationResponse currentAppConfig)
        {
            ApplicationConfigurationResponse existingConfig;

            bool isValue = _appConfig.TryGetValue(currentAppConfig.AppConfigId.ToString(), out existingConfig);
            if (isValue)
                _appConfig.TryUpdate(currentAppConfig.AppConfigId.ToString(), currentAppConfig, existingConfig); //Returns true
            else
                _appConfig.TryAdd(currentAppConfig.AppConfigId.ToString(), currentAppConfig);
        }

        public static List<ApplicationConfigurationResponse> GetAllAppConfigItems()
        {
            List<ApplicationConfigurationResponse> configsList = new List<ApplicationConfigurationResponse>();
            ApplicationConfigurationResponse config;
            foreach (var item in _appConfig)
            {
                config = new ApplicationConfigurationResponse();
                configsList.Add(config);
            }
            return configsList;
        }
    }
}
