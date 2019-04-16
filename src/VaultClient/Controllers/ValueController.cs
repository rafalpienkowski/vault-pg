using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VaultClient.Settings;
using VaultClient.Vault;

namespace VaultClient.Controllers
{
    [Route("api/values")]
    public class ValueController : ControllerBase
    {
        private readonly IOptionsSnapshot<AppSettings> _appSettings;
        private readonly IConfigurationRoot _configurationRoot;

        public ValueController(IOptionsSnapshot<AppSettings> appSettings, IConfigurationRoot configurationRoot)
        {
            _appSettings = appSettings;
            _configurationRoot = configurationRoot;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var values = new List<string> {"My first Value"};
            if (!string.IsNullOrEmpty(_appSettings?.Value.Secret))
            {
                values.Add($"My secret: {_appSettings?.Value.Secret}");
            }

            if (_appSettings?.Value?.SomeInt != null)
            {
                values.Add($"Some int: {_appSettings.Value.SomeInt}");
            }
            return Ok(values);
        }

        [HttpGet]
        [Route("refresh")]
        public IActionResult Refresh()
        {
            _configurationRoot.Reload();
            return Ok("Refreshed");
        }
    }
}