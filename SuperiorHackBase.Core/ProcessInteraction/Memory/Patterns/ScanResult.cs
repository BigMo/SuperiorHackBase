using SuperiorHackBase.Core.ProcessInteraction.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns
{
    public class ScanResult
    {
        public const string DEFAULT_VALUE_NAME = "Value";

        public Dictionary<string,Pointer> Values { get; private set; }
        public Pointer Value
        {
            get { return Values.ContainsKey(DEFAULT_VALUE_NAME) ? Values[DEFAULT_VALUE_NAME] : Pointer.Zero; }
            set { Values[DEFAULT_VALUE_NAME] = value; }
        }
        
        public ScanResult()
        {
            Values = new Dictionary<string, Pointer>();
        }

        public override string ToString()
        {
            return string.Join(", ", Values.Select(v => $"{v.Key}={v.Value}").ToArray());
        }
    }
}
