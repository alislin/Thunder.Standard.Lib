using System;
using System.Collections.Generic;

namespace ZeroLib.ConsoleView
{
    public interface IViewItem
    {
        Guid Id { get; set; }
        int Index { get; set; }
        string Info { get; set; }
        List<ProcessBar> ProcessItems { get; set; }
        string Title { get; set; }
        ProcessBar TotalProcess { get; set; }

        Guid Add(string title);
        void Remove(Guid id);
        void Update(Guid id, string title, int value, string info, int min = 0, int max = 100);
        List<string> ToList(int width = 80);
    }
}