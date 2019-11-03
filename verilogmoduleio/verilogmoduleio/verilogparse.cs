using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using verilogmoduleio.moduleParamData;

namespace verilogmoduleio
{
    class verilogparse
    {
        public readonly string linecommentword = "//";
        public readonly string startareacommentword = "/*";
        public readonly string endareacommentword = "*/";

        public readonly string inputPortword = "input";
        public readonly string outputPortword = "output";
        public readonly string inoutPortword = "inout";
        public readonly string wireword = "wire";
        public readonly string regword = "reg";
        public readonly string moduleword = "module";
        public readonly string endmoduleword = "endmodule";

        /// <summary>
        /// remove comment from verilog code.
        /// </summary>
        /// <param name="verilogcode">verilog code string</param>
        /// <returns>comment removed comment</returns>
        public string commentRemove(string verilogcode)
        {
            bool linecomment = false;
            bool areacomment = false;
            string removedcode = string.Empty;

            var reg = new Regex(@"\/*[\s\S]*?\*/|//.*");

            removedcode = reg.Replace(verilogcode, " ");

            return removedcode;


        }

        /// <summary>
        /// split each verilog module into string[].
        /// note: comment should be removed before running this function.
        /// </summary>
        /// <param name="verilogcode">verilog code without comment</param>
        /// <returns>splitted code</returns>
        public string[] moduleSplit(string verilogcode)
        {

            var reg = new Regex(endmoduleword);
            var modules = reg.Split(verilogcode);

            return modules;

        }


        public moduleParam extractmodulePorts(string verilogcode)
        {
            moduleParam module = new moduleParam();
            var modulename = extractmoduleDefinition(verilogcode);
            Dictionary<string, signalParam> ports = new Dictionary<string, signalParam>();

            if ( modulename != string.Empty || modulename != null )
            {
                module.moduleName = modulename;

                // input port extract
                var input =  this.extractSignalDefinition(verilogcode, inputPortword);
                var output = this.extractSignalDefinition(verilogcode, outputPortword);
                var inout = this.extractSignalDefinition(verilogcode, inoutPortword);

                try
                {
                    module.signalDic = input.Concat(output).ToDictionary(v => v.Key, v => v.Value).Concat(inout).ToDictionary(v=>v.Key,v=>v.Value);
                    


                }
                catch (Exception ex)
                {
                    Console.WriteLine("verilog code parsing error.");
                }



            }

            return module;
        }



        // this function should be private and be called by public investigate all module description.
        public string  extractmoduleDefinition(string verilogcode)
        {
            var moduleStartPoint = verilogcode.IndexOf(moduleword);
            var endofmoduleDefinitionPoint = verilogcode.IndexOf('(');
            string modulename = string.Empty;

            if ( endofmoduleDefinitionPoint >= 0)
            {
                var startofmoduleDefinitionPoint = moduleStartPoint + moduleword.Length;
                modulename = verilogcode.Substring(startofmoduleDefinitionPoint, endofmoduleDefinitionPoint - startofmoduleDefinitionPoint);
                modulename = modulename.Trim();
            }

            return modulename;
        }


        // this function should be private and be called by public investigate all module description.
        public Dictionary<string, signalParam> extractSignalDefinition(string  verilogcode, string extractKeyword)
        {
            Dictionary<string, signalParam> foundsignals = new Dictionary<string, signalParam>();

            // split line. verilog uses ';' to separate each line.
            var reg = new Regex(@";");
            var lines = reg.Split(verilogcode);

            foreach(var line in lines)
            {
                var startPointofword = line.IndexOf(extractKeyword);

                if (startPointofword >= 0 )
                {

                    // extract width
                    var widthMatch = Regex.Match(line, @"\[[\s\S]*?\]");
                    var width = widthMatch.Value;

                    // extract reg/wire definition and remove word from line
                    var wirePoint = line.IndexOf(wireword);
                    var regPoint = line.IndexOf(regword);
                    signalTypeProperty type = signalTypeProperty.wire;

                    if ( regPoint >= 0)
                    {
                        type = signalTypeProperty.reg;
                    }
                    var regwireRemovedline = line.Replace(wireword, "");
                    regwireRemovedline = regwireRemovedline.Replace(regword, "");



                    // extract signalname
                    var shiftedline = regwireRemovedline.Substring(startPointofword);
                    var temp = shiftedline.Trim().Replace(extractKeyword, "");

                    temp = temp.Replace(";", "");
                    if ( width != string.Empty && width != null)
                        temp = temp.Replace(width, "");

                    var namesinline = temp.Split(",");

                    foreach( var name in namesinline)
                    {
                        signalParam currentParam = new signalParam();

                        currentParam.signalName = name.Trim();
                        currentParam.signalWidth = width;
                        currentParam.signalProperty.Add(extractKeyword);
                        currentParam.signalType = type;
                        currentParam.signalIO = judgeIOfromword(extractKeyword);

                        foundsignals.Add(name,currentParam);
                    }


                }
            }


            return foundsignals;
        }


        private signalIOProperty judgeIOfromword(string keyword)
        {
            signalIOProperty iotype = new signalIOProperty();

            if ( keyword == inputPortword)
            {
                iotype = signalIOProperty.inputPort;
            }
            else if ( keyword == outputPortword)
            {
                iotype = signalIOProperty.outputPort;
            }
            else if ( keyword == inoutPortword)
            {
                iotype = signalIOProperty.inoutPort;
            }
            else
            {
                iotype = signalIOProperty.unKnown;
            }

            return iotype;

        }


    }
}
