using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VaultClient.Settings;

namespace VaultClient.Controllers
{
    [Route("api/values")]
    public class ValueController : ControllerBase
    {
        private readonly IOptions<AppSettings> _appSettings;

        public ValueController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var values = new List<string> {"My first Value"};
            if (!string.IsNullOrEmpty(_appSettings.Value?.Secret))
            {
                values.Add($"My secret: {_appSettings.Value.Secret}");
            }
            return Ok(values);
        }
    }
}