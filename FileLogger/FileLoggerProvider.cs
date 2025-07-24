using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Options;

namespace FileLogger;

public sealed class FileLoggerProvider : ILoggerProvider
{
	private readonly IDisposable? _onChangeToken;
	private FileLoggerConfiguration _currentConfig;
	private FileLoggerConfiguration GetCurrentConfig() => _currentConfig;
	private readonly ConcurrentDictionary<string, FileLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

	public FileLoggerProvider(IOptionsMonitor<FileLoggerConfiguration> config)
	{
		_currentConfig = config.CurrentValue;
		_onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
	}


	public ILogger CreateLogger(string categoryName)
	{
		return _loggers.GetOrAdd(categoryName, name => new FileLogger(name, GetCurrentConfig));
	}

	public void Dispose()
	{
		_loggers.Clear();
		_onChangeToken?.Dispose();
	}
}

public class FileLoggerConfiguration
{
	public required string LogsPath { get; set; }
	public required LogLevel minLogLevel { get; set; }
}