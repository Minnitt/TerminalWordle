namespace Wordle.Interface;

using System;
using System.Threading;

public class TerminalWindow
{
    //check once every 50ms if they have changed
    public ResizeWatcher resizeWatcher { get; }
    public int? width { get; private set; }
    public int? height { get; private set; }
    
    public bool isTooSmall { get; private set; }
    
    public TerminalWindow()
    {
        resizeWatcher = new ResizeWatcher(50);
        width = Console.WindowWidth;
        height = Console.WindowHeight;
        isTooSmall = width > ResizeWatcher.MIN_WIDTH && height > ResizeWatcher.MIN_HEIGHT;
        
        resizeWatcher.Start();

        resizeWatcher.OnResize += (_, args) =>
        {
            width = args.Width;
            height = args.Height;
            isTooSmall = args.TooSmall;
        };

        resizeWatcher.OnSizeRestored += (_, args) =>
        {
            width = args.Width;
            height = args.Height;
            isTooSmall = args.TooSmall;
        };

        resizeWatcher.OnTooSmall += (_, args) =>
        {
            width = args.Width;
            height = args.Height;
            isTooSmall = args.TooSmall;
        };
    }

}