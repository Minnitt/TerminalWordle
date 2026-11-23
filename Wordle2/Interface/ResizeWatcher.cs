namespace Wordle.Interface;

public class ResizeEventArgs(int? width, int? height, bool isSmall) : EventArgs
{
    public int? Width { get; } = width;
    public int? Height { get; } = height;
    public bool TooSmall { get; } =  isSmall;
}

public class ResizeWatcher(int pollingInterval)
{
    public const int MIN_WIDTH = 40;
    public const int MIN_HEIGHT = 24;
    
    public event EventHandler<ResizeEventArgs>? OnResize;
    public event EventHandler<ResizeEventArgs>? OnTooSmall;
    public event EventHandler<ResizeEventArgs>? OnSizeRestored;
    
    private int _lastWidth = Console.WindowWidth;
    private int _lastHeight = Console.WindowHeight;
    private bool _wasTooSmall;
    private Thread? _watcherThread;
    private CancellationTokenSource? _cts;

    public void Start()
    {
        _cts = new CancellationTokenSource();

        _watcherThread = new Thread(() => WatchLoop(_cts.Token))
        {
            IsBackground = true,
            Name = "ResizeWatcherThread"
        };
        
        _watcherThread.Start();
    }

    public void Stop()
    {
        _cts.Cancel();
    }

    private void WatchLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            int currentWidth = Console.WindowWidth;
            int currentHeight = Console.WindowHeight;
            
            bool tooSmall = currentWidth < MIN_WIDTH || currentHeight < MIN_HEIGHT;
            bool changed = currentWidth != _lastWidth || currentHeight != _lastHeight;

            if (tooSmall)
            {
                // Only fire OnTooSmall once when we first enter the too-small state,
                // not repeatedly every poll tick while it stays small
                if (!_wasTooSmall)
                {
                    _wasTooSmall = true;
                    OnTooSmall?.Invoke(this, new ResizeEventArgs(null, null, true));
                }
            }
            else
            {
                // Window is a valid size — check if we just recovered from too-small
                if (_wasTooSmall)
                {
                    _wasTooSmall = false;
                    OnSizeRestored?.Invoke(this, new ResizeEventArgs(currentWidth, currentHeight, false));   // Let the app know it can redraw properly
                }
                else if (changed)
                {
                    OnResize?.Invoke(this, new ResizeEventArgs(currentWidth, currentHeight, false)); // Normal resize within valid bounds
                }
            }
            
            _lastWidth = currentWidth;
            _lastHeight = currentHeight;
            
            Thread.Sleep(pollingInterval);
        }
    }
}