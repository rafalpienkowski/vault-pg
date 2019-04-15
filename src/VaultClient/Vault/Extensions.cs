using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace VaultClient.Vault
{
    public static class Extensions
    {
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }

        public static IWebHostBuilder UseVault(this IWebHostBuilder builder, string key = null)
            => builder.ConfigureServices(s =>
                {
                    IConfiguration configuration;
                    using (var serviceProvider = s.BuildServiceProvider())
                    {
                        configuration = serviceProvider.GetService<IConfiguration>();
                    }

                    var options = configuration.GetOptions<VaultOptions>("vault");
                    s.AddSingleton(options);
                    s.AddTransient<IVaultStore, VaultStore>();
                })
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    var options = cfg.Build().GetOptions<VaultOptions>("vault");
                    cfg.AddVault(options, key);
                });

        private static IConfigurationBuilder AddVault(this IConfigurationBuilder builder,
            VaultOptions options, string key)
        {
            var client = new VaultStore(options);
            var secret = string.IsNullOrWhiteSpace(key)
                ? client.GetDefaultAsync().GetAwaiter().GetResult()
                : client.GetAsync(key).GetAwaiter().GetResult();
            var parser = new JsonParser();
            var data = parser.Parse(JObject.FromObject(secret));
            var source = new MemoryConfigurationSource {InitialData = data};
            builder.Add(source);

            return builder;
        }

    }
}

    