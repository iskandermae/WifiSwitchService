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
            _logger.LogError("allowedwifi is not set");
            Environment.Exit(1);
        }
        try {
            while (!stoppingToken.IsCancellationRequested) {
                await Task.Run(() => WifiHelper.Switch(allowed), stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
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
