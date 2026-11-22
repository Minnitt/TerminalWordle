using Wordle.Logic;
using Wordle.Logic.Enums;

namespace Wordle.Interface;

public class WordleGrid
{
    private const int GridWidth = 40;
    private const int GridHeight = 24;
    private const string EmptyCell = "\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;8;8;8m▀\e[0m\e[38;2;8;8;8;48;2;0;0;0m▀\e[0m\e[38;2;8;8;8;48;2;8;8;8m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\n";
    private TerminalWindow _window;
    private float _leftPadding;
    private float _topPadding;
    private float _rightPadding;
    private float _bottomPadding;

    public WordleGrid()
    {
        _window = new TerminalWindow();
        _window.resizeWatcher.OnResize += HandleResizeWatcher;
        _window.resizeWatcher.OnSizeRestored += HandleResizeWatcher;
        _window.resizeWatcher.OnTooSmall += HandleResizeWatcher;
    }

    private void HandleResizeWatcher(object? sender, EventArgs e)
    {
        if (e is ResizeEventArgs args && args.TooSmall == false)
        {
            if (args.Width != null && args.Height != null)
            {
                CalculatePadding((int)args.Width, (int)args.Height);
            }
            else throw new ArgumentNullException(nameof(sender), "ResizeEventArgs width or height was null when TooSmall was false.");
        }
        else
        {
            throw new Exception("Console window too small");
        }
    }

    private void CalculatePadding(int width, int height)
    {
        int freeVerticalSpace = height - GridHeight;
        int freeHorizontalSpace = width - GridWidth;

        _topPadding = freeVerticalSpace / 3;
        _bottomPadding = freeVerticalSpace - _topPadding;
        _rightPadding = freeHorizontalSpace / 2;
        _leftPadding = freeHorizontalSpace - _rightPadding;
        
    }

    private void PrintLetterGrid(LetterContainer[,] grid)
    {
        try
        {
            if (grid.GetLength(0) > 4)
            {
                throw new  ArgumentOutOfRangeException(nameof(grid), grid.GetLength(0), "Too many columns");
            }
            else if (grid.GetLength(0) < 0)
            {
                throw new  ArgumentOutOfRangeException(nameof(grid), grid.GetLength(0), "Too many rows");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private (int, int) GetGridPosition(int x, int y)
    {
        var result = new ValueTuple<int, int>();
        try
        {
            if (x is > 4 or < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(x), x, "x is too large or small");
            }

            if (y is > 5 or < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(y),  y, "y is too large or small");
            }

            int gridXPosition = (int) _leftPadding + x * 8;
            int gridYPosition = (int) _topPadding + y * 4;
            result.Item1 = gridXPosition;
            result.Item2 = gridYPosition;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return result;
    }
    
}

/*
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     ####### ####### ####### ####### ####### 
     
     30 across, 24 tall
     */