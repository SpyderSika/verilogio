using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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

            var reg = new Regex(@"endmodule");
            var modules = reg.Split(verilogcode);

            return modules;

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

                    // extract signalname
                    var shiftedline = line.Substring(startPointofword);
                    
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

                        foundsignals.Add(name,currentParam);
                    }


                }
            }


            return foundsignals;
        }





    }
}
