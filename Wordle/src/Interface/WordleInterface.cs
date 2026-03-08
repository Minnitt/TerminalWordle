namespace Wordle.Interface;

using Logic;
using System.IO;
using System;

public class Cell
{
    public Cell(string letterSprite, string letter, LetterColour colour, int row, int col)
    {
        _letterSprite = letterSprite;
        letterContainer = new LetterContainer(letter, colour);
        Row = row;
        Col = col;
    }
    
    private string _letterSprite;
    public LetterContainer letterContainer;
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
    private int _currentCellRow;
    private int _currentCellCol;
    public event EventHandler? _currentlyPrintedMessageChanged;

    public string CurrentlyPrintedMessage
    {
        get
        {
            if (string.IsNullOrEmpty(field))
            {
                return "";
            }
            return field;
        }
        set
        {
            field = value;
            _currentlyPrintedMessageChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void InitialiseDisplay()
    {
        Console.Clear();
        if (height < 1 || width < 1)
        {
            PrintMessage("Window is too small. Please Resize");
        }

        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                _cells[i, j] = new Cell(_emptyCell, "", LetterColour.Unknown, i, j);
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
        string[] lines = cell.LetterSprite.Split("\n");
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

    private void CellInputHandle(WordleGame game)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        string key = keyInfo.KeyChar.ToString();
        bool cellEmpty = _cells[_currentCellRow, _currentCellCol].LetterSprite == _emptyCell;
        if (cellEmpty)
        {
            if (IsInAlphabet(key))
            {
                _cells[_currentCellRow, _currentCellCol].LetterSprite = GetLetterSprite(key, LetterColour.Black);
                _cells[_currentCellRow, _currentCellCol].letterContainer.Letter = key;
                _cells[_currentCellRow, _currentCellCol].letterContainer.Colour = LetterColour.Black;
                if (_currentCellCol < 4) _currentCellCol++;
            }
            else if (keyInfo.Key == ConsoleKey.Backspace)
            {
                _cells[_currentCellRow, _currentCellCol].LetterSprite = _emptyCell;
                _cells[_currentCellRow, _currentCellCol].letterContainer.Letter = "";
                _cells[_currentCellRow, _currentCellCol].letterContainer.Colour = LetterColour.Unknown;
                if (_currentCellCol != 0) _currentCellCol--;
            }
            else if (keyInfo.Key == ConsoleKey.Enter) PrintMessage("Guess is too short.", 1000);
        }
        else
        {
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                string guess = "";
                for (int i = 0; i < 5; i++)
                {
                    guess += _cells[_currentCellRow, i].letterContainer.Letter;
                }

                LetterContainer[] attemptOutput = game.Attempt(guess);
                CellsUpdate(attemptOutput);
            }
        }
        
        
    }

    private void CellsUpdate(LetterContainer[] attemptInput)
    {
        for (int i = 0; i < attemptInput.Length; i++)
        {
            _cells[_currentCellRow, i].LetterSprite = GetLetterSprite(attemptInput[i]);
            _cells[_currentCellRow, i].letterContainer.Letter = attemptInput[i].Letter;
            _cells[_currentCellRow, i].letterContainer.Colour = attemptInput[i].Colour;
        }
    }

    // public void AttemptInputHandle() //ts genuinely ass
    // {
    //     while (true)
    //     {
    //         ConsoleKeyInfo key = Console.ReadKey(true);
    //         int currentRow = _currentCell/6;
    //         int currentCol = _currentCell%5;
    //         if (key.Key == ConsoleKey.Backspace)
    //         {
    //             _cells[currentRow, currentCol].LetterSprite = _emptyCell;
    //             if (_currentCell < 0) // if the cellCursor is not on any of the cells
    //             {
    //                 _currentCell *= -1; 
    //             }
    //             else if (_currentCell == 0)
    //             {
    //                 continue;
    //             }
    //             _currentCell--;
    //         }
    //         else if (key.Key == ConsoleKey.Enter)
    //         {
    //             if (_currentCell < 0)
    //             {
    //                 _currentCell *= -1;
    //                 
    //                 return;
    //             }
    //         }
    //         else if (!IsInAlphabet(key.Key.ToString()))
    //         {
    //             continue;
    //         }
    //         else if (_currentCell < 0)// ignores input if cellCursor is not on any of the cells. I also realise now that this idea was not the best.
    //         {
    //             continue;
    //         }
    //         string letter =  key.KeyChar.ToString().ToLower();
    //         _cells[currentRow, currentCol].LetterSprite = GetLetterSprite(letter, LetterColour.Black);
    //         
    //     }
    // }

    private void HandlePrintMessageEvent(object? sender, EventArgs e)
    {
        PrintMessage(CurrentlyPrintedMessage);
    }

    public void PrintMessage(string message, int timeAsMilliseconds)
    {
        PrintMessage(message);
        Thread.Sleep(timeAsMilliseconds);
        PrintMessage("");
    }
    public void PrintMessage(string message)
    {
        // CurrentlyPrintedMessage = message;
        List<string> messageAsList = [message];
        int yOffset = height - 2*verticalPadding / 3;
        int lineLengthCap = width - horizontalPadding;
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
        
        if (string.IsNullOrEmpty(CurrentlyPrintedMessage))
        {
            for (int i = 0; i < height - yOffset; i++)
            {
                int padding = (width - lineLengthCap) / 2;
                string whiteSpace = new string(' ', lineLengthCap);
                Console.SetCursorPosition(padding, yOffset + i);
                Console.Write(whiteSpace);
            }
        }

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

    private string GetLetterSprite(LetterContainer input)
    {
        return GetLetterSprite(input.Letter, input.Colour);
    }

    private string GetLetterSprite(string letter, LetterColour colour)
    {
        string filename = Path.Combine("resources", "letters");
        
        switch (colour)
        {
            case LetterColour.Black:
                filename = Path.Combine(filename, "black_");
                break;
            case LetterColour.Gray:
                filename = Path.Combine(filename, "gray_");
                break;
            case LetterColour.Green:
                filename = Path.Combine(filename, "green_");
                break;
            case LetterColour.Yellow:
                filename = Path.Combine(filename, "yellow_");
                break;
        }
        
        if (letter.Length != 1)
        {
            CurrentlyPrintedMessage = "Letter in GetLetterSprite() is not 1 char.";
            return _emptyCell;
        }
        
        letter = letter.ToLower();
        letter = letter.Trim();
        filename += letter;
        filename += ".txt";
        return ReadWholeFile(filename);
    }

    private string ReadWholeFile(string filename)
    {
        return File.ReadAllText(filename);
    }
    
    public WordleInterface()
    {
        Console.CursorVisible = false;
        Console.Clear();
        _cells = new Cell[6, 5];
        _currentCellRow = 0;
        _currentCellCol = 0;
        height = Console.WindowHeight;
        width = Console.WindowWidth;
        horizontalPadding = (width - 40) / 2;
        verticalPadding = (height - 24) / 2;
        _emptyCell = "\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;8;8;8m▀\e[0m\e[38;2;8;8;8;48;2;0;0;0m▀\e[0m\e[38;2;8;8;8;48;2;8;8;8m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\n";
        _currentlyPrintedMessageChanged += HandlePrintMessageEvent;

    }   
}