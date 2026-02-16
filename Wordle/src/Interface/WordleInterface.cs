namespace Wordle.Interface;

using System.IO;
using System;

public struct Cell(string letter)
{
    public string letter = letter;
}
public class WordleInterface
{  
    private Cell[,] _cells;
    
    public void Display()
    {
        Console.Clear();
        var height = Console.WindowHeight;
        var width = Console.WindowWidth;
        var horizontalPadding = (width - 40) / 2;
        var verticalPadding = (height - 24) / 2;
        
        string baseDir = @"C:\Users\atnm\RiderProjects\ConsoleApp1\Wordle";
        string resourcesDir = Path.Combine(baseDir, "Resources");
        string emptyCell;
        try
        {
            emptyCell = File.ReadAllText(Path.Combine(resourcesDir, "empty_cell.txt"));
        }
        catch (Exception e)
        {
            Console.WriteLine("File Not Found");
            return;
        }

        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                _cells[i, j] = new Cell(emptyCell);
            }
        }
        
        for (int row = 0; row < 24; row += 4)
        {
            for (int col = 0; col < 40; col += 8)
            {
                PrintLetterCell(emptyCell, row+verticalPadding, col+horizontalPadding);
            }
        }
        
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
        this._cells = new Cell[5, 6];
    }
    
    
}