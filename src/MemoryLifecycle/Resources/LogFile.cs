namespace MemoryLifecycle.Resources;

/// <summary>
/// Учебная обёртка над файлом журнала. Финализатор только сообщает
/// об утечке, а не пытается закрыть управляемый StreamWriter.
/// </summary>
public sealed class LogFile : IDisposable
{
    private static int counter;
    private readonly int id = ++counter;
    private readonly string path;
    private readonly StreamWriter writer;
    private bool disposed;

    public LogFile(string path)
    {
        this.path = path;
        writer = new StreamWriter(path, append: true) { AutoFlush = true };
        Console.WriteLine($"[LogFile {id}] файл открыт: {path}");
    }

    public void Write(string message)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        writer.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {message}");
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        writer.Dispose();
        disposed = true;
        GC.SuppressFinalize(this);
        Console.WriteLine($"[LogFile {id}] файл закрыт");
    }

    ~LogFile()
    {
        Console.WriteLine($"[LogFile {id}] финализатор! — файл не был закрыт явно");
    }
}
