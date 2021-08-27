using System;
using System.Collections.Concurrent;
using E.Bots.Configuration;
using Microsoft.Extensions.Configuration;

namespace E.Bots
{
    public class BotConfigurationFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, IBotConfiguration>> _botConfigurations;
        private static readonly ConcurrentDictionary<Type, string> Names;

        static BotConfigurationFactory()
        {
            Names = new ConcurrentDictionary<Type, string>();
        }

        public BotConfigurationFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _botConfigurations = new ConcurrentDictionary<Type, ConcurrentDictionary<string, IBotConfiguration>>();
            
        }
        
        public static string AddOrUpdateConfigurationName<T>(string configName)
        {
            var name = Names.AddOrUpdate(typeof(T), configName, (k, v) => configName);
            return name;
        }

        public T Get<T>(string tenantId) where T : IBotConfiguration
        {
            
            var configs = _botConfigurations.GetOrAdd(typeof(T), key =>
            {
                var name = AddOrUpdateConfigurationName<T>(typeof(T).Name.Replace("Configuration", ""));
                var botConfigs = new ConcurrentDictionary<string, T>();
                _configuration.Bind(name, botConfigs);

                var result = new ConcurrentDictionary<string, IBotConfiguration>();
                foreach (var botConfiguration in botConfigs)
                {
                    result.AddOrUpdate(botConfiguration.Key, botConfiguration.Value, (k, v) => v);
                }
                return result;
            });

            if (configs.ContainsKey(tenantId))
            {
                if (configs.TryGetValue(tenantId,out var config))
                {
                    return (T)config;
                }
            }

            return default;
        }
    }
}
