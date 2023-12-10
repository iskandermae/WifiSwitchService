namespace HWork.WifiSwitchService;

public class WifiSwitchackgroundService : BackgroundService
{
    private readonly ILogger<WifiSwitchackgroundService> _logger;

    public WifiSwitchackgroundService(ILogger<WifiSwitchackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try {
            while (!stoppingToken.IsCancellationRequested) {
                string joke = _jokeService.GetJoke();
                _logger.LogWarning("{Joke}", joke);

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
