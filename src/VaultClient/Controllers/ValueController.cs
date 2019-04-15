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

        public ValueController(IOptionsSnapshot<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var values = new List<string> {"My first Value"};
            if (!string.IsNullOrEmpty(_appSettings?.Value.Secret))
            {
                values.Add($"My secret: {_appSettings?.Value.Secret}");
            }
            return Ok(values);
        }

        [HttpGet]
        [Route("refresh")]
        public IActionResult Refresh()
        {
            return Ok("ok");
        }
    }
}