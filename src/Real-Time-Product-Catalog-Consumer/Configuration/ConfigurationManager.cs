using Microsoft.Extensions.Options;

namespace Real_Time_Product_Catalog_Consumer.Configuration;

public class ConfigurationManager
{
    private readonly AppSettings _appSettings;

    public ConfigurationManager(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public void OnGet()
    {
        
    }

}