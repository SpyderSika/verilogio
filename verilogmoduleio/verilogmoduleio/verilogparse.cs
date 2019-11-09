﻿using System;
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
        public readonly string logicword = "logic";
        public readonly string moduleword = "module";
        public readonly string endmoduleword = "endmodule";

        public readonly string left2rightarrow = "->";
        public readonly string busleft2rightarrow = "=>";

        public readonly string rigt2leftarrow = "<-";
        public readonly string busrigt2leftarrow = "<=";

        public readonly string bothdirectionarrow = "<->";
        public readonly string bothbusdirectionarrow = "<=>";

        /// <summary>
        /// remove comment from verilog code.
        /// </summary>
        /// <param name="verilogcode">verilog code string</param>
        /// <returns>comment removed code</returns>
        public string commentRemove(string verilogcode)
        {
            var removedcode = string.Empty;
            var reg = new Regex(@"\/*[\s\S]*?\*/|//.*",RegexOptions.Compiled);

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

                try
                {
                    module.signalDic = this.extractSignalDefinition(verilogcode, "");
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
            var startofparameterDefinitionPoint = verilogcode.IndexOf("#(");
            string modulename = string.Empty;

            endofmoduleDefinitionPoint = ( endofmoduleDefinitionPoint - startofparameterDefinitionPoint == 1 ) ? startofparameterDefinitionPoint : endofmoduleDefinitionPoint;

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
                var crRemovedline = line.Replace("\r", " ").Replace("\n", " ");

                var temp = extractSignal(crRemovedline);

                foundsignals = foundsignals.Concat(temp).ToDictionary(v => v.Key, v => v.Value);

            }
            return foundsignals;
        }


        public enum signalDefinitionState
        {
            init,io,widthstart,widthend,name
        }


        private Dictionary<string, signalParam>extractSignal(string line)
        {
            Dictionary<string, signalParam> foundsignals = new Dictionary<string, signalParam>();

            try
            {

            string[] splittedLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            signalParam currentParam = new signalParam();
            signalDefinitionState currentstate = signalDefinitionState.init;

            foreach ( var st in splittedLine)
            {
                if ( ( st == inputPortword || st == inoutPortword || st == outputPortword) )
                {
                    currentParam = new signalParam();
                    currentParam.signalIO = judgeIOfromword(st);
                    currentstate = signalDefinitionState.io;

                }
                else if ( (st.StartsWith("[") || st.EndsWith("]")) && currentstate == signalDefinitionState.io  )
                {
                    currentParam.signalWidth += st;
                    bool widthWrittenin1 = st.StartsWith("[") && st.EndsWith("]");
                    bool widthEndDetect  = (currentstate == signalDefinitionState.widthstart ) && st.EndsWith("]");

                    if ( widthWrittenin1 || widthEndDetect)
                        currentstate = signalDefinitionState.widthend;
                    else
                        currentstate = signalDefinitionState.widthstart;
                }
                else if (( currentstate == signalDefinitionState.widthend || currentstate == signalDefinitionState.io) && !signalTypeDefinitionWord(st) )
                {
                    currentstate = signalDefinitionState.name;

                    var signalNamearray = st.Split(new string[] { ",", ";" ,")"}, StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach(var name in signalNamearray)
                    {
                        var signal = new signalParam();
                        signal.signalIO = currentParam.signalIO;
                        signal.signalWidth = currentParam.signalWidth;
                        signal.signalName = name;

                        if ( foundsignals.ContainsKey(name))
                        {
                            Console.WriteLine("Signal {0} is already defined",name);
                        }
                        else
                        {
                            foundsignals.Add(name,signal);
                        }
                    }

                    currentstate = signalDefinitionState.init;
                }


            }
            }
            catch ( Exception ex)
            {
                var msg = ex.ToString();
                Console.WriteLine(msg);
                throw ex;
            }

            return foundsignals;
        }


        private bool signalTypeDefinitionWord(string word)
        {
            bool result = false;

            if (word == wireword || word == regword || word == logicword)
                result = true;

            return result;

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

        public string drawmoduleIO(moduleParam modP)
        {
            string result = string.Empty;

            string moduletop = "################";
            string modulebody = "#              #";

            // module name
            result += linecommentword + modP.moduleName + Environment.NewLine;

            // max input port length
            var maxlength = 0;
            foreach( var sig in modP.signalDic)
            {
                var leftlength = sig.Value.signalName.Length + sig.Value.signalWidth.Length;
                maxlength = (maxlength < leftlength) ? leftlength : maxlength;
            }

            // padding
            var leftpadding = string.Empty;
            for (int i = 0; i < maxlength + left2rightarrow.Length; i++)
                leftpadding += " ";

            result += linecommentword + leftpadding + moduletop + Environment.NewLine;

            // loop twice is not smart.... 
            foreach (var sig in modP.signalDic)
            {
                var temp = string.Empty;

                if (sig.Value.signalIO == signalIOProperty.inputPort)
                {
                    var leftport = sig.Value.signalName + sig.Value.signalWidth;
                    int loop = maxlength - leftport.Length;

                    // padding
                    if (leftport.Length < maxlength)
                        for (int i = 0; i < loop; i++)
                            leftport += " ";

                    var arrow = (sig.Value.signalWidth == string.Empty) ? left2rightarrow : busleft2rightarrow;

                    result += linecommentword + leftport + arrow + modulebody + Environment.NewLine;

                }
                else
                {
                    var rightport = sig.Value.signalName + sig.Value.signalWidth;



                    var arrow = (sig.Value.signalWidth == string.Empty) ? left2rightarrow : busleft2rightarrow;
                    if (sig.Value.signalIO == signalIOProperty.inoutPort)
                        arrow = (sig.Value.signalWidth == string.Empty) ? bothdirectionarrow : bothbusdirectionarrow;


                    result += linecommentword + leftpadding + modulebody + arrow + rightport + Environment.NewLine;

                }

            }

            result += linecommentword + leftpadding + moduletop + Environment.NewLine;




            return result;
        }


    }
}
