using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
namespace ParallelProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            int row = 400;
            int column = 400;
            int col3 = column * 3;
            int[] rgb = new int[row * column * 3];
            float flipCol = 1 / column;
            float flipRow = 1 / row;

            for (int i = 0; i < row; i++)
            {
                Task t1 = Task.Factory.StartNew(() => {
                    for (int j1 = 0; j1 < col3; j1 += 3)

                    {
                        rgb[i * column + j1] = (int)Math.Round(i * flipRow * 256);
                    }
                });
                Task t2 = Task.Factory.StartNew(() => {
                    for (int j2 = 1; j2 < col3; j2++)
                    {
                        rgb[i * column + j2] = (int)Math.Round((j2 - 1) * flipCol * 256);
                        j2 += 2;
                    }
                });
                Task t3 = Task.Factory.StartNew(() => {
                    for (int j3 = 2; j3 < col3; j3 += 3)
                    {
                        rgb[i * column + j3] = (int)Math.Round(0.5F * 256);
                    }
                });
                Task.WaitAll(t1, t2, t3);
            }

            string output = ("P3\n" + row + " " + column + "\n" + 255 + "\n") + String.Join(" ", rgb);

            try { File.WriteAllText("output_threads2.ppm", output); }
            catch { Console.WriteLine("Error writing to file"); }

            stopwatch.Stop();
            TimeSpan sw = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                sw.Hours, sw.Minutes, sw.Seconds, sw.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }
    }
}



//SIMD Method
/*using System;
using System.IO;
using System.Numerics;
using System.Diagnostics;

namespace ParallelProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int row = 400;
            int column = 400;
            string output = "";
            output += ("P3\n" + row + " " + column + "\n" + 255 + "\n");

            for (float i = 0; i < row; i++)
            {
                for (float j = 0; j < column; j++)
                {
                    var v1 = new Vector3(i, j, 0.5F);
                    var v2 = new Vector3(row, column, 1);
                    var Vresult = (v1 / v2) * 256;
                    output += Vresult.X + " " + Vresult.Y + " " + Vresult.Z + "\n";
                }
            }

            try { File.WriteAllText("output_SIMD.ppm", output); }
            catch { Console.WriteLine("Error writing to file"); }

            stopwatch.Stop();
            TimeSpan sw = stopwatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            sw.Hours, sw.Minutes, sw.Seconds, sw.Milliseconds / 10);

            Console.WriteLine("RunTime " + elapsedTime);
        }
    }
}*/



//Threaded Method
/*using System;
using System.IO;
using System.Numerics;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int row = 400;
            int column = 400;
            string output = "";
            int r = 0, g = 0, b = 0;

            var vec = new Vector3(0.1F, 0.2F, 0.3F);
            output += ("P3\n" + row + " " + column + "\n" + 255 + "\n");

            for (float i = 0; i < row; i++)
            {
                for (float j = 0; j < column; j++)
                {
                    Task t1 = Task.Factory.StartNew(() => {
                        vec.X = i / row;
                        r = (int)(vec.X * 256);
                    });

                    Task t2 = Task.Factory.StartNew(() => {
                        vec.Y = j / column;
                        g = (int)(vec.Y * 256);
                    });

                    Task t3 = Task.Factory.StartNew(() => {
                        vec.Z = 0.5F;
                        b = (int)(vec.Z * 256);
                    });

                    Task.WaitAll(t1, t2, t3);

                    output += r + " " + g + " " + b + "\n";
                }
            }

            try { File.WriteAllText("output_threads.ppm", output); }
            catch { Console.WriteLine("Error writing to file"); }

            stopwatch.Stop();

            TimeSpan sw = stopwatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            sw.Hours, sw.Minutes, sw.Seconds, sw.Milliseconds / 10);

            Console.WriteLine("RunTime " + elapsedTime);

        }
    }
}*/



//Sequential Method

/*using System;
using System.IO;
using System.Numerics;
using System.Diagnostics;

namespace ParallelProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int row = 400;
            float column = 400;
            string output = "";
            int r, g, b;

            var vec = new Vector3(0.1F, 0.2F, 0.3F);
            output += ("P3\n" + row + " " + column + "\n" + 255 + "\n");

            for (float i = 0; i < row; i++)
            {
                for (float j = 0; j < column; j++)
                {
                    vec.X = i / row;
                    vec.Y = j / column;
                    vec.Z = 0.5F;

                    r = (int)(vec.X * 256);
                    g = (int)(vec.Y * 256);
                    b = (int)(vec.Z * 256);

                    output += r + " " + g + " " + b + "\n";
                }
            }

            try { File.WriteAllText("output_sequential.ppm", output); }
            catch
            {
                Console.WriteLine("Error writing to file");
            }

            stopwatch.Stop();

            TimeSpan sw = stopwatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            sw.Hours, sw.Minutes, sw.Seconds, sw.Milliseconds / 10);

            Console.WriteLine("RunTime " + elapsedTime);

        }
    }
}*/
