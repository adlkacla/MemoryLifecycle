namespace MemoryLifecycle.Resources;

/// <summary>
/// Ресурс с детерминированным освобождением через IDisposable.
/// </summary>
public sealed class ManagedResource : IDisposable
{
    private static int counter;
    private readonly int id = ++counter;
    private bool disposed;

    public ManagedResource()
    {
        Console.WriteLine($"[ManagedResource {id}] открыт");
    }

    public bool IsDisposed => disposed;

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        Console.WriteLine($"[ManagedResource {id}] Dispose вызван");
    }
}
