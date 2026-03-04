using System.Text;

namespace Wordle.Interface;

using Wordle.Logic;
using System.IO;
using System;

public class Cell
{
    public Cell(string letter, int row, int col)
    {
        _letterSprite = letter;
        Row = row;
        Col = col;
    }
    
    private string _letterSprite;
    public int Row { get; set; }
    public int Col { get; set; }
    
    public event EventHandler? CellChanged;

    public string LetterSprite
    {
        get => _letterSprite;
        set
        {
            _letterSprite = value;
            CellChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
public class WordleInterface
{  
    private Cell[,] _cells;
    private int height;
    private int width;
    private int horizontalPadding;
    private int verticalPadding;
    private readonly string _emptyCell;
    private int _currentCell;
    
    public void InitialiseDisplay()
    {
        Console.Clear();

        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                _cells[i, j] = new Cell(_emptyCell, i, j);
            }
        }
        
        foreach (Cell cell in _cells)
        {
            SubscribeToCell(cell);
        }
        
        for (int row = 0; row < 6; row += 1)
        {
            for (int col = 0; col < 5; col += 1)
            {
                PrintLetterCell(_cells[row,col]);
            }
        }
    }

    private void SubscribeToCell(Cell cell)
    {
        cell.CellChanged += HandleLetterUpdate;
    }
    
    private void HandleLetterUpdate(object? sender, EventArgs e)
    {
        if (sender is Cell cell)
        {
            PrintLetterCell(cell);
        }
        else
        {
            Console.WriteLine("Failed to print cell");
        }
            
    }

    private void PrintLetterCell(string letter, int row, int col)
    {
        Console.SetCursorPosition(col, row);
        string[] lines = letter.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            Console.SetCursorPosition(col, row+i);
            Console.Write(lines[i]);
        }
    }

    private void PrintLetterCell(Cell cell)
    {
        Console.SetCursorPosition(cell.Row, cell.Col);
        string[] lines = cell.LetterSprite.Split();
        for (int i = 0; i < lines.Length; i++)
        {
            Console.SetCursorPosition(horizontalPadding + 8*cell.Col,verticalPadding + 4*cell.Row + i);
            Console.Write(lines[i]);
        }
    }
    
    public void WordleGameLoop()
    {
        WordleGame wordle = new WordleGame();
        

    }

    private void AttemptInputHandle()
    {
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            int currentRow = _currentCell/6;
            int currentCol = _currentCell%5;
            if (key.Key == ConsoleKey.Backspace)
            {
                _cells[currentRow, currentCol].LetterSprite = _emptyCell;
                if (_currentCell < 0)
                {
                    _currentCell *= -1;
                }
                _currentCell--;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (_currentCell < 0)
                {
                    _currentCell *= -1;
                    return;
                }
            }
            else if (!IsInAlphabet(key.Key.ToString()))
            {
                continue;
            }
            else if (_currentCell < 0)
            {
                continue;
            }
            string letter =  key.KeyChar.ToString().ToLower();
            _cells[currentRow, currentCol].LetterSprite = GetLetterSprite(letter, LetterColour.Black);
            
        }
    }

    public void PrintMessage(string message)
    {
        List<string> messageAsList = [message];
        int yOffset = Console.BufferHeight - 2*verticalPadding / 3;
        int lineLengthCap = Console.BufferWidth - horizontalPadding;
        while (messageAsList[^1].Length > lineLengthCap)
        {
            string uncutMessage = messageAsList[^1];
            int splitIndex = lineLengthCap;
            for (int i = 0; i < splitIndex; i++)
            {
                if (uncutMessage[splitIndex - i] == ' ')
                {
                    splitIndex = lineLengthCap - i;
                    break;
                }
            }
            string subString1 = uncutMessage[..splitIndex];
            string subString2 = uncutMessage[splitIndex..];
            messageAsList[^1] = subString1;
            messageAsList.Add(subString2);
        }
        //redo ts and also add check to see if the message will overflow off the screen

        for (int i = 0; i < messageAsList.Count; i++)
        {
            if (yOffset + i < Console.BufferHeight)
            {
                string currentMessage = messageAsList[i];
                int currentMessageLength = currentMessage.Length;
                int padding = (width - currentMessageLength) / 2;
                Console.SetCursorPosition(padding, yOffset + i);
                Console.Write(messageAsList[i]);
            }
            
        }
        
        //splice the string at the line length cap and then walk backwards until you find a space. then turn the string into 2 substrings. check if the next string is shorter than the line length cap. if yes repeat until you have a list of strings, all to be printed on their own separate lines
        

    }
    private bool IsInAlphabet(string input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        foreach (char c in input)
        {
            char upper = char.ToUpper(c);

            if (upper < 'A' || upper > 'Z')
                return false;
        }

        return true;
    }

    private string GetLetterSprite(string letter, LetterColour colour)
    {
        return "";
    }

    private string ReadFile(string filename)
    {
        return "";
    }
    
    public WordleInterface()
    {
        Console.Clear();
        _cells = new Cell[6, 5];
        _currentCell = 0;
        height = Console.WindowHeight;
        width = Console.WindowWidth;
        horizontalPadding = (width - 40) / 2;
        verticalPadding = (height - 24) / 2;
        _emptyCell = "\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;8;8;8m▀\e[0m\e[38;2;8;8;8;48;2;0;0;0m▀\e[0m\e[38;2;8;8;8;48;2;8;8;8m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\n";
        
    }   
}