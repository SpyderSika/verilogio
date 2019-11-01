using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace verilogmoduleio
{
    class verilogparse
    {
        public readonly string linecommentword = "//";
        public readonly string startareacommentword = "/*";
        public readonly string endareacommentword = "*/";
        public string commentRemove(string verilogcode)
        {
            bool linecomment = false;
            bool areacomment = false;
            string removedcode = string.Empty;

            var reg = new Regex(@"\/*[\s\S]*?\*/|//.*");

            removedcode = reg.Replace(verilogcode, " ");

            return removedcode;


        }

        public string[] moduleSplit(string verilogcode)
        {

            var reg = new Regex(@"endmodule");
            var modules = reg.Split(verilogcode);

            return modules;

        }



    }
}
