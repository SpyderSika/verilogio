using System;
using System.IO;

namespace verilogmoduleio
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("verilogio");

            if (args.Length < 2)
            {
                Console.WriteLine("verilogio option target.v");
                Console.WriteLine("option: -d  draw module IO diagram");

            }

            var option = args[0].ToLower();

            switch (option)
            {
                case "-d":
                    drawDiagram(args);
                    break;
            }


        }

        private static void drawDiagram(string[] args)
        {
            // read verilog file
            using (StreamReader fr = new StreamReader(args[1]))
            {
                var code = fr.ReadToEnd();

                verilogparse vp = new verilogparse();
                var commentRemoved = vp.commentRemove(code);

                var modulecodearray = vp.moduleSplit(commentRemoved);

                // extract module 
                foreach (var m in modulecodearray)
                {
                    var mdata = vp.extractmodulePorts(m);
                    var diagram = vp.drawmoduleIO(mdata);

                    Console.WriteLine(diagram);
                }
            }
        }
    }
}