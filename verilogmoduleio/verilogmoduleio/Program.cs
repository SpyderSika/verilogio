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

                // comment remove test
                var result = vp.commentRemove(code);
                Console.WriteLine(result);
                Console.WriteLine("-----------------");

                // module split test
                var modules = vp.moduleSplit(result);
                foreach(var s in modules)
                {
                    Console.WriteLine("===========");
                    Console.WriteLine(s);
                    Console.WriteLine("===========");
                }
                Console.WriteLine("-----------------");

                // extract signal test
                var signals = vp.extractSignalDefinition(modules[0], "input");
                foreach(var  s in signals)
                {
                    Console.WriteLine(String.Format("{0},{1},{2}", s.Value.signalName, s.Value.signalWidth, s.Value.signalProperty[0]));
                }



            }

        }
    }
}
