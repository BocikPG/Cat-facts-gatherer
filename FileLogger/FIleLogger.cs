
using System.Text;
using System.Text.Unicode;

namespace FileLogger;

public class FileLogger(
	string name,
	Func<FileLoggerConfiguration> getCurrentConfig) : ILogger, IDisposable
{
	string currentFileName;
	UTF8Encoding utf8 = new UTF8Encoding();



	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{

		return this;
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return getCurrentConfig().minLogLevel >= logLevel;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
	{
		if (!IsEnabled(logLevel))
		{
			return;
		}

		if (string.IsNullOrEmpty(currentFileName))
		{
			currentFileName = DateTime.UtcNow.ToString().Replace(".", "-").Replace(":", "");
		}

		try
		{
			using (var file = File.OpenWrite($"{Directory.GetCurrentDirectory()}{getCurrentConfig().LogsPath}/{currentFileName}.txt"))
			{
				byte[] buffer = utf8.GetBytes($"{formatter(state, exception)}\n");

				try
				{
					file.Seek(file.Length, SeekOrigin.Begin);
					file.Write(buffer);
				}
				catch (IOException ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}
		catch (DirectoryNotFoundException)
		{
			Directory.CreateDirectory(Directory.GetCurrentDirectory() + getCurrentConfig().LogsPath);
			Log(logLevel, eventId, state, exception, formatter);
			return;
		}



	}

	public void Dispose()
	{
	}
}