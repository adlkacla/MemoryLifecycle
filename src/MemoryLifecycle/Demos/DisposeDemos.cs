using MemoryLifecycle.Resources;

namespace MemoryLifecycle.Demos;

public static class DisposeDemos
{
    public static async Task RunSyncAndAsyncDemoAsync()
    {
        Console.WriteLine("1. Синхронный сценарий using");
        using (var connection = new DatabaseConnection())
        {
            connection.Execute("SELECT * FROM Students");
        }

        Console.WriteLine();
        Console.WriteLine("2. Асинхронный сценарий await using");
        await using (var connection = new DatabaseConnection())
        {
            connection.Execute("SELECT * FROM Topics");
            await Task.Delay(1);
        }

        VerifySuppressFinalize();

        Console.WriteLine();
        Console.WriteLine("4. Финализатор без Dispose");
        CreateConnectionWithoutDispose();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine(
            "После сборки: финализатор освободил только нативный ресурс");
    }

    public static async Task RunFileBufferDemoAsync()
    {
        Console.WriteLine();
        Console.WriteLine("5. FileBuffer: using и await using");

        using (var buffer = new FileBuffer())
        {
            buffer.Write(new byte[] { 1, 2, 3, 4 });
        }

        await using (var buffer = new FileBuffer())
        {
            buffer.Write(new byte[] { 5, 6, 7 });
            await Task.Delay(1);
        }
    }

    public static void VerifySuppressFinalize()
    {
        Console.WriteLine();
        Console.WriteLine("3. Проверка GC.SuppressFinalize после using");
        using (var connection = new DatabaseConnection())
        {
            connection.Execute("SELECT 1");
        }

        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("После сборки: сообщение финализатора отсутствует");
    }

    private static void CreateConnectionWithoutDispose()
    {
        var connection = new DatabaseConnection();
        connection.Execute("UPDATE Students SET Name = 'Test'");
    }
}
