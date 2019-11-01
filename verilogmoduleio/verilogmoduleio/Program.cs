using System;
using System.IO;

namespace verilogmoduleio
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // read verilog file
            using (StreamReader fr = new StreamReader(args[0]))
            {
                var code = fr.ReadToEnd();
                verilogparse vp = new verilogparse();

                var result = vp.commentRemove(code);

                Console.WriteLine(result);

                Console.WriteLine("-----------------");

                var modules = vp.moduleSplit(result);

                foreach(var s in modules)
                {
                    Console.WriteLine("===========");
                    Console.WriteLine(s);
                    Console.WriteLine("===========");
                }


            }

        }
    }
}
