using Microsoft.Extensions.Logging;

namespace LinDrive.Application.IO.Services;

public class FileWatcherService : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private readonly ILogger<FileWatcherService> _logger;

    public FileWatcherService(ILogger<FileWatcherService> logger)
    {
        string filter = "*.*";
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        
        _logger = logger;
        _watcher = new FileSystemWatcher(path, filter)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName,
            EnableRaisingEvents = true,
        };
        
        _watcher.Created += WatcherOnCreated;
        _watcher.Changed += WatcherOnChanged;
        _watcher.Renamed += WatcherOnRenamed;
    }

    private void WatcherOnRenamed(object sender, RenamedEventArgs e)
    {
        _logger.LogInformation($"FileWatcherService: Renamed : {e.Name} {e.OldFullPath} {e.FullPath}");
    }

    private void WatcherOnChanged(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation($"FileWatcherService: Changed : {e.Name}");
    }

    private void WatcherOnCreated(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation($"FileWatcherService: Created: {e.FullPath}");
        _logger.LogInformation($"FileWatcherService: Created: {e.Name}");
    }

    public void Dispose() => _watcher.Dispose();
}