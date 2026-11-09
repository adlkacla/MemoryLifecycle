using MemoryLifecycle.Gc;
using System.Text;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        PressureScenarios.RunAll();

    }
}