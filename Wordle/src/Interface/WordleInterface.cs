namespace Wordle.Interface;

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
    private string emptyCell;
    
    public void InitialiseDisplay()
    {
        Console.Clear();
        
        string baseDir = @"C:\Users\atnm\RiderProjects\ConsoleApp1\Wordle";
        string resourcesDir = Path.Combine(baseDir, "Resources");
        

        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                _cells[i, j] = new Cell(emptyCell, i, j);
            }
        }
        
        for (int row = 0; row < 6; row += 1)
        {
            for (int col = 0; col < 5; col += 1)
            {
                PrintLetterCell(emptyCell, (row*4)+verticalPadding, (col*8)+horizontalPadding);
            }
        }
        
    }

    public void SubscribeToCell(Cell cell)
    {
        cell.CellChanged += HandleLetterUpdate;
    }

    private void HandleLetterUpdate(Object sender, EventArgs e)
    {
        Cell cell = (Cell)sender;
        PrintLetterCell(cell.LetterSprite, cell.Row, cell.Col);
    }

    public void PrintLetterCell(string letter, int row, int col)
    {
        Console.SetCursorPosition(col, row);
        string[] lines = letter.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            Console.SetCursorPosition(col, row+i);
            Console.Write(lines[i]);
        }
    }
    public WordleInterface()
    {
        Console.Clear();
        _cells = new Cell[5, 6];
        height = Console.WindowHeight;
        width = Console.WindowWidth;
        horizontalPadding = (width - 40) / 2;
        verticalPadding = (height - 24) / 2;
        emptyCell = "\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;8;8;8m▀\e[0m\e[38;2;8;8;8;48;2;0;0;0m▀\e[0m\e[38;2;8;8;8;48;2;8;8;8m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;8;8;8;48;2;21;21;21m▀\e[0m\e[38;2;21;21;21;48;2;21;21;21m▀\e[0m\e[38;2;37;37;37;48;2;37;37;37m▀\e[0m\n\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\e[38;2;37;37;37m▀\e[0m\n";
        foreach (Cell cell in _cells)
        {
            SubscribeToCell(cell);
        }
    }
}