using System.Collections.Generic;

namespace ZeroLib.DebugLog
{
    public interface ILog
    {
        string File { get; }
        List<string> Items { get; }
        int MaxCount { get; set; }

        void Write(string log);
    }
}