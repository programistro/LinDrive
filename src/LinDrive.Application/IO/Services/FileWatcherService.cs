using Microsoft.Extensions.Logging;

namespace LinDrive.Application.IO.Services;

public class FileWatcherService : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private readonly ILogger<FileWatcherService> _logger;

    public FileWatcherService(ILogger<FileWatcherService> logger)
    {
        string filter = "*.*";
        string path = "";
        
        _logger = logger;
        _watcher = new FileSystemWatcher(path, filter)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName,
            EnableRaisingEvents = true,
        };
        
        _watcher.Created += WatcherOnCreated;
    }

    private void WatcherOnCreated(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation($"FileWatcherService: Created: {e.FullPath}");
        _logger.LogInformation($"FileWatcherService: Created: {e.Name}");
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}