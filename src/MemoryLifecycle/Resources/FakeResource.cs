namespace MemoryLifecycle.Resources;

/// <summary>
/// Ресурс без IDisposable: освобождается только финализатором.
/// </summary>
public sealed class FakeResource
{
    private static int counter;
    private readonly int id = ++counter;

    public FakeResource()
    {
        Console.WriteLine($"[FakeResource {id}] открыт");
    }

    ~FakeResource()
    {
        Console.WriteLine($"[FakeResource {id}] финализатор");
    }
}
