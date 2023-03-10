using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testing
{
    internal class Class1
    {
        public const int N = 10;
        public static void Main()
        {
            Console.WriteLine("CUDAfy Example\nCollecting necessary resources...");

            CudafyModes.Target = eGPUType.Cuda; // To use OpenCL, change this enum
            CudafyModes.DeviceId = 0;
            CudafyTranslator.Language = CudafyModes.Target == eGPUType.OpenCL ? eLanguage.OpenCL : eLanguage.Cuda;

            //Check for available devices
            if (CudafyHost.GetDeviceCount(CudafyModes.Target) == 0)
                throw new System.ArgumentException("No suitable devices found.", "original");

            //Init device
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            Console.WriteLine("Running example using {0}", gpu.GetDeviceProperties(false).Name);

            //Load module for GPU
            CudafyModule km = CudafyTranslator.Cudafy();
            gpu.LoadModule(km);

            //Define local arrays
            int[] a = new int[N];
            int[] b = new int[N];
            int[] c = new int[N];

            // allocate the memory on the GPU
            int[] dev_c = gpu.Allocate<int>(c);

            // fill the arrays 'a' and 'b' on the CPU
            for (int i = 0; i < N; i++)
            {
                a[i] = i;
                b[i] = i * i;
            }

            // copy the arrays 'a' and 'b' to the GPU
            int[] dev_a = gpu.CopyToDevice(a);
            int[] dev_b = gpu.CopyToDevice(b);

            gpu.Launch(1, N).add(dev_a, dev_b, dev_c);

            // copy the array 'c' back from the GPU to the CPU
            gpu.CopyFromDevice(dev_c, c);

            // display the results
            for (int i = 0; i < N; i++)
                Console.WriteLine("{0} + {1} = {2}", a[i], b[i], c[i]);

            // free the memory allocated on the GPU
            gpu.FreeAll();

            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        [Cudafy]
        public static void add(GThread thread, int[] a, int[] b, int[] c)
        {
            int tid = thread.threadIdx.x;
            if (tid < N)
                c[tid] = a[tid] + b[tid];
        }
    }
}
