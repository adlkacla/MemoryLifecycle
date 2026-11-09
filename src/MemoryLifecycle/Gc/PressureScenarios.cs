using System.Diagnostics;

namespace MemoryLifecycle.Gc;

/// <summary>
/// Сценарии, создающие разное давление на управляемую кучу.
/// </summary>
public static class PressureScenarios
{
    public static ScenarioResult RunSmallObjects(int count = 300_000, int size = 100)
    {
        var before = GcSnapshot.Capture();
        var stopwatch = Stopwatch.StartNew();

        long checksum = 0;
        for (var i = 0; i < count; i++)
        {
            var buffer = new byte[size];
            buffer[0] = (byte)i;
            checksum += buffer[0];
        }

        stopwatch.Stop();
        var after = GcSnapshot.Capture();
        GC.KeepAlive(checksum);

        return new ScenarioResult("Мелкие объекты (byte[100])",
            stopwatch.Elapsed, after.Diff(before));
    }

    public static ScenarioResult RunLargeObjects(int count = 10, int size = 100000)
    {
        var before = GcSnapshot.Capture();
        var stopwatch = Stopwatch.StartNew();

        var retained = new List<byte[]>(count);
        for (var i = 0; i < count; i++)
        {
            retained.Add(new byte[size]);
        }

        stopwatch.Stop();
        var after = GcSnapshot.Capture();
        GC.KeepAlive(retained);

        return new ScenarioResult("Крупные объекты (byte[100 000])",
            stopwatch.Elapsed, after.Diff(before));
    }

    public static ScenarioResult RunLongLived(int count = 1_000)
    {
        var before = GcSnapshot.Capture();
        var stopwatch = Stopwatch.StartNew();

        var retained = new List<byte[]>(count);
        for (var i = 0; i < count; i++)
        {
            retained.Add(new byte[512]);
        }

        stopwatch.Stop();
        var after = GcSnapshot.Capture();
        GC.KeepAlive(retained);

        return new ScenarioResult("Удерживаемые объекты (byte[512])",
            stopwatch.Elapsed, after.Diff(before));
    }

    public static ScenarioResult RunForcedGen2()
    {
        var before = GcSnapshot.Capture();
        var stopwatch = Stopwatch.StartNew();

        GC.Collect(2, GCCollectionMode.Forced, blocking: true);

        stopwatch.Stop();
        var after = GcSnapshot.Capture();

        return new ScenarioResult("Принудительный Gen2 (blocking)",
            stopwatch.Elapsed, after.Diff(before));
    }

    public static void RunMyScenario()
    {
        GcObserver.PrintMemoryInfo("Память до эксперимента");
        RunSmallObjects(200000, 50);
        RunLargeObjects(90000, 5);
        GcObserver.PrintMemoryInfo("Память после эксперимента");
    }


    public static void RunAll()
    {
        RunMyScenario();
        /*
        Console.WriteLine("Сценарии давления на GC");
        GcObserver.PrintMemoryInfo("Состояние кучи до экспериментов");
        Console.WriteLine(new string('-', 92));
        Console.WriteLine(
            "{0,-34}{1,10}{2,6}{3,6}{4,6}{5,11}{6,12}",
            "Сценарий", "Время, мс", "Gen0", "Gen1", "Gen2",
            "Alloc, МБ", "Heap Δ, МБ");
        Console.WriteLine(new string('-', 92));

        var results = new[]
        {
            RunSmallObjects(),
            RunLargeObjects(),
            RunLongLived(),
            RunForcedGen2()
        };

        foreach (var result in results)
        {
            Console.WriteLine(
                "{0,-34}{1,10:F2}{2,6}{3,6}{4,6}{5,11:F2}{6,12:F2}",
                result.Name,
                result.Elapsed.TotalMilliseconds,
                result.Delta.Gen0,
                result.Delta.Gen1,
                result.Delta.Gen2,
                result.Delta.AllocatedMegabytes,
                result.Delta.HeapDeltaMegabytes);
        }

        Console.WriteLine(new string('-', 92));
        GcObserver.PrintMemoryInfo("Состояние кучи после экспериментов");
        Console.WriteLine(
            "Значения зависят от машины, режима GC и момента запуска. " +
            "Важна не абсолютная величина, а соотношение счётчиков.");
        */
    }
}

public readonly record struct ScenarioResult(
    string Name, TimeSpan Elapsed, GcDelta Delta);
