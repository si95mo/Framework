using System;
using System.Text.RegularExpressions;

namespace Rest.TransferModel.System.Hardware
{
    public class CpuInformationMatch : Information
    {
        public Regex Regex { get; private set; }
        public Action<string> Update { get; private set; }

        public CpuInformationMatch(string pattern, Action<string> update)
        {
            Regex = new Regex(pattern, RegexOptions.Compiled);
            Update = update;
        }
    }
}
