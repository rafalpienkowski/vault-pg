using Microsoft.Extensions.Configuration;

namespace VaultClient.Vault
{
    public class VaultConfigurationSource : IConfigurationSource
    {
        private readonly VaultOptions _vaultOptions;
        private readonly string _key;

        public VaultConfigurationSource(VaultOptions vaultOptions, string key)
        {
            _vaultOptions = vaultOptions;
            _key = key;
        }
        
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new VaultConfigurationProvider(_vaultOptions, _key);
        }
    }
}