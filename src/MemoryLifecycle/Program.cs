using MemoryLifecycle.Gc;
using System.Runtime;
using System.Text;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        //1
        /*
        PressureScenarios.RunAll();
        */

        //2
        GcModeScenarios.RunParallelAllocation(objSize: 100, objectsPerWorker: 300000);
    }
}