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
                Console.WriteLine("option  -c  clear all coment");
            }
            else
            {

                var option = args[0].ToLower();

                switch (option)
                {
                    case "-c":
                        removeComment(args);
                        break;

                    case "-d":
                        drawDiagram(args);
                        break;
                    case "-p"
                        drawPlantUML(args);
                        break;
                }

            }
        }

        private static void removeComment(string[] args)
        {
            using (StreamReader fr = new StreamReader(args[1]))
            {
                var code = fr.ReadToEnd();

                verilogparse vp = new verilogparse();
                var commentRemoved = vp.commentRemove(code);
            
                Console.WriteLine(commentRemoved);
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

        private static void drawPlantUML(string[] args)
        {
            moduleParam mP = new moduleParam();

            SortedDictionary<string,moduleParam> moduleParamDic = new SortedDictionary<string,moduleParam>();


            // read verilog file list
            using (StreamReader flist = new StreamReader(args[1]))
            {
                string file = String.Empty;

                while( (file = flist.ReadLine()) != null)
                {

                // read file
                // read verilog file
                    using (StreamReader fr = new StreamReader(file))
                    {
                        var code = fr.ReadToEnd();

                    }
                }

            }

        }


    }
}