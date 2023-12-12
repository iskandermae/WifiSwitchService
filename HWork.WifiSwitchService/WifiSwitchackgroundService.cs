using Windows.Devices.WiFi;

namespace HWork.WifiSwitchService;

public class WifiSwitchackgroundService : BackgroundService
{
    private readonly ILogger<WifiSwitchackgroundService> _logger;
    private readonly IConfiguration _configuration;

    public WifiSwitchackgroundService(ILogger<WifiSwitchackgroundService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string? allowed = ConfigurationBinder.GetValue<string?>(_configuration, "allowedwifi");
        if (string.IsNullOrWhiteSpace(allowed)) {
            _logger.LogError("specify command line parameter: allowedwifi=name1;name2;name3");
            Environment.Exit(1);
        }
        var access = await WiFiAdapter.RequestAccessAsync();
        if (access != WiFiAccessStatus.Allowed) {
            _logger.LogError("no access to WiFiAdapter");
            Environment.Exit(1);
        }
        string oldMessage = "";
        try {
            while (!stoppingToken.IsCancellationRequested) {
                string message = await WifiHelper.Switch(allowed);
                if (message != oldMessage) {
                    _logger.LogInformation(message);
                    oldMessage = message;
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        } catch (OperationCanceledException) {
            // When the stopping token is canceled, for example, a call made from services.msc,
            // we shouldn't exit with a non-zero exit code. In other words, this is expected...
        } catch (Exception ex) {
            _logger.LogError(ex, "{Message}", ex.Message);
            Environment.Exit(1);
        }
    }
}
