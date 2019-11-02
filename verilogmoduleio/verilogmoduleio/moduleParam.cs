using System;
using System.Collections.Generic;
using System.Text;

namespace verilogmoduleio.moduleParamData
{
    /// <summary>
    /// Data store class for module definition
    /// </summary>
    class moduleParam
    {
        public string moduleName;
        public string instanceName;

        public Dictionary<string, signalParam> wireDic;
        public Dictionary<string, instanceParam> instanceDic;

        public string definedFilePath;

        public moduleParam()
        {
            moduleName = string.Empty;
            instanceName = string.Empty;
            wireDic = new Dictionary<string, signalParam>();
            instanceDic = new Dictionary<string, instanceParam>();

            definedFilePath = string.Empty;
        }

    }

    /// <summary>
    /// Data store class for instance.
    /// it should be used under moduleParam class
    /// </summary>
    public class instanceParam
    {
        public string moduleName;
        public string instanceName;

        public Dictionary<string, signalParam> wireDic;

        public instanceParam()
        {
            moduleName = string.Empty;
            instanceName = string.Empty;

            wireDic = new Dictionary<string, signalParam>();
        }

    }



    /// <summary>
    /// wire property enum
    /// inputPort : verilog input
    /// outputPort: verilog output
    /// inoutPort: verilog inout
    /// internalSingal: verilog  internal wire
    /// </summary>
    public enum signalIOProperty
    {
        inputPort,outputPort,inoutPort, internalSignal,unKnown
    }

    /// <summary>
    /// signal type : verlog reg or verilog wire
    /// </summary>
    public  enum signalTypeProperty
    {
        reg,wire
    }

    /// <summary>
    /// wire property definition class
    /// 
    /// </summary>
    public class signalParam
    {
        public string signalName;
        public string signalWidth;
        public List<string> signalProperty; // input, output, inout, reg,wire
        
        public signalIOProperty signalIO;
        public signalTypeProperty signalType;

        public signalParam()
        {
            signalName = string.Empty;
            signalWidth = string.Empty;
            signalProperty = new List<string>();
            signalIO = signalIOProperty.unKnown;
            signalType = signalTypeProperty.wire;
        }

    }


}
