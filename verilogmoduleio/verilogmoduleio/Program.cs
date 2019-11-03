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
                /*
                var signals = vp.extractSignalDefinition(modules[0], "input");
                foreach(var  s in signals)
                {
                    Console.WriteLine(String.Format("{0},{1},{2}", s.Value.signalName, s.Value.signalWidth, s.Value.signalProperty[0]));
                }
                Console.WriteLine("-----------------");
                */

                // extract module test
                foreach( var m in modules)
                {
                    var mdata = vp.extractmodulePorts(m);

                    Console.WriteLine(":::::::::::::::::::::::::");
                    Console.WriteLine(mdata.moduleName);

                    foreach( var s in mdata.signalDic)
                    {
                        Console.WriteLine(String.Format("{0},{1},{2},{3}",s.Value.signalName,s.Value.signalIO.ToString(),s.Value.signalWidth,s.Value.signalType.ToString()));
                    }

                    var diagram = vp.drawmoduleIO(mdata);

                    Console.WriteLine(diagram);


                }




            }

        }
    }
}
