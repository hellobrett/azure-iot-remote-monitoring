using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIoT.Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                Device device = new Device();
                device.Start();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("Unexpected exception:");
                foreach (var e in ae.InnerExceptions)
                    Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception: " + ex.Message);
            }
        }
    }
}
