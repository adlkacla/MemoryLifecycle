using MemoryLifecycle.Resources;

namespace MemoryLifecycle.Demos;

public static class ResourceDemos
{
    public static void RunFinalizerDemo()
    {
        Console.WriteLine("1. Финализатор: момент вызова не контролируется");
        CreateFakeResources();
        Console.WriteLine("Метод завершён, финализаторы ещё не выполнялись");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("После GC.Collect + WaitForPendingFinalizers");
    }

    public static void RunUsingDemo()
    {
        Console.WriteLine();
        Console.WriteLine("2. using: детерминированное освобождение");
        using (var resource = new ManagedResource())
        {
            Console.WriteLine("Работа внутри using-блока");
        }

        Console.WriteLine("Строка после using-блока");
    }

    public static void RunForgottenDisposeDemo()
    {
        Console.WriteLine();
        Console.WriteLine("3. Забытый Dispose: ресурс живёт до сборки");
        CreateManagedResourceWithoutDispose();
        Console.WriteLine("Метод завершён, Dispose не вызывался");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("После сборки (ресурс уже недостижим)");
    }

    public static void RunLogFileDemo(string path)
    {
        Console.WriteLine();
        Console.WriteLine("4. LogFile: корректный и некорректный сценарии");

        using (var log = new LogFile(path))
        {
            log.Write("Корректная запись через using");
        }

        Console.WriteLine("После корректного using-блока");

        CreateLogFileWithoutDispose(path);
        Console.WriteLine("Метод с забытым Dispose завершён");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("После сборки: сработал финализатор");
    }

    private static void CreateFakeResources()
    {
        _ = new FakeResource();
        _ = new FakeResource();
    }

    private static void CreateManagedResourceWithoutDispose()
    {
        var resource = new ManagedResource();
        Console.WriteLine($"Ресурс создан, IsDisposed = {resource.IsDisposed}");
        GC.KeepAlive(resource);
    }

    private static void CreateLogFileWithoutDispose(string path)
    {
        var log = new LogFile(path);
        log.Write("Запись без using: Dispose не вызван");
        GC.KeepAlive(log);
    }
}
