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
/*
        private (string, bool, bool) linecommentRemove(string linecode, bool currentlinecomment, bool currentareacomment)
        {
            bool linecomment = false;
            bool areacomment = false;
            string removedcode = linecode;

            // find '//'
            var linecommentposition = removedcode.IndexOf(linecommentword);
            var startareacommentposition = removedcode.IndexOf(startareacommentword);
            var endareacommentposition = removedcode.IndexOf(endareacommentword);

            if ( currentareacomment == true)
            {
                if (endareacommentposition >= 0)
                {

                }
                else
                {

                }
            }
            else
            {



            }





        }
        */

        // /*da
        /* //afdsa
         */

        


    }
}
