using System.Diagnostics;
using System.Runtime;

namespace MemoryLifecycle.Gc;

/// <summary>
/// Многопоточная аллокация для сравнения Workstation и Server GC.
/// </summary>
public static class GcModeScenarios
{
    public static void RunParallelAllocation(
        int workerCount = 4, int objectsPerWorker = 100_000, int objSize = 256)
    {
        Console.WriteLine("Многопоточная аллокация");
        Console.WriteLine($"Server GC: {GCSettings.IsServerGC}");
        Console.WriteLine($"Latency mode: {GCSettings.LatencyMode}");
        Console.WriteLine(
            $"Потоков: {workerCount}, объектов на поток: {objectsPerWorker:N0}");
        MeasureForcedGen2Pause();
        Console.WriteLine();

        var before = GcSnapshot.Capture();
        var stopwatch = Stopwatch.StartNew();

        var tasks = new Task<long>[workerCount];
        for (var worker = 0; worker < workerCount; worker++)
        {
            tasks[worker] = Task.Run(() =>
            {
                long checksum = 0;
                for (var i = 0; i < objectsPerWorker; i++)
                {
                    var buffer = new byte[objSize];
                    buffer[0] = (byte)i;
                    checksum += buffer[0];
                }

                return checksum;
            });
        }

        var checksums = Task.WhenAll(tasks).GetAwaiter().GetResult();
        stopwatch.Stop();

        var after = GcSnapshot.Capture();
        var delta = after.Diff(before);

        Console.WriteLine($"Время: {stopwatch.Elapsed.TotalMilliseconds:F2} мс");
        Console.WriteLine(
            $"Gen0: {delta.Gen0}, Gen1: {delta.Gen1}, Gen2: {delta.Gen2}");
        Console.WriteLine($"Аллоцировано: {delta.AllocatedMegabytes:F2} МБ");
        Console.WriteLine($"Контрольная сумма: {checksums.Sum()}");
        Console.WriteLine();
    }

    public static void MeasureForcedGen2Pause()
    {
        var stopwatch = Stopwatch.StartNew();
        GC.Collect(2, GCCollectionMode.Forced, blocking: true);
        stopwatch.Stop();
        Console.WriteLine(
            $"Принудительный блокирующий Gen2: " +
            $"{stopwatch.Elapsed.TotalMilliseconds:F2} мс");
    }
}
