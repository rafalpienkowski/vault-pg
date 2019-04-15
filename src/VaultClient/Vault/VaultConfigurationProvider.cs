using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace VaultClient.Vault
{
    public class VaultConfigurationProvider : ConfigurationProvider
    {
        private readonly VaultOptions _options;
        private readonly string _key;

        public VaultConfigurationProvider(VaultOptions options, string key)
        {
            _options = options;
            _key = key;
        }

        public override void Load()
        {
            var client = new VaultStore(_options);
            var secret = string.IsNullOrWhiteSpace(_key)
                ? client.GetDefaultAsync().GetAwaiter().GetResult()
                : client.GetAsync(_key).GetAwaiter().GetResult();
            var parser = new JsonParser();
            Data = parser.Parse(JObject.FromObject(secret));
        }
    }
}