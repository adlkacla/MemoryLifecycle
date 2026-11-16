using MemoryLifecycle.Demos;
using MemoryLifecycle.Gc;
using MemoryLifecycle.Resources;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        //1
        /*
        PressureScenarios.RunAll();
        */

        //2
        //GcModeScenarios.RunParallelAllocation(objSize: 100, objectsPerWorker: 300000);
    
        //3
        //ResourceDemos.RunLogFileDemo(Path.Combine(Path.GetTempPath(), "memory-lifecycle.log"));

        //4
        using (var db = new DatabaseConnection())
        {
            db.Execute("SELECT * FROM lolkek");
        }
        await using (var db2 = new DatabaseConnection())
        {
            db2.Execute("SELECT * FROM lolkek2");
        }

        using (var file = new FileBuffer())
        {
            file.Write(new byte[] { 1, 2, 3, 4 });
        }
        await using (var file2 = new FileBuffer())
        {
            file2.Write(new byte[] { 5, 6, 7 });
        }

        await DisposeDemos.RunSyncAndAsyncDemoAsync();
        await DisposeDemos.RunFileBufferDemoAsync();
    }
}
