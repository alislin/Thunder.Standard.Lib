using System;

namespace ZeroLib.ConsoleView
{
    public interface IConsoleView
    {
        int Width { get; }

        IViewItem Add(string title);
        void Remove(Guid id);
    }
}