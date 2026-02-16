namespace Wordle.Interface;


using System.IO;
using System;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


public class WordleInterface
{  
    
    //public void SettingTheScreenToAnImage(string imageFilepath)
    //{
    //    var width = Console.WindowWidth;
    //    var height = Console.WindowHeight;
    //    Image<Rgba32> img;
//
    //    try
    //    {
    //        img = Image.Load<Rgba32>(imageFilepath);
    //    }
    //    catch
    //    {
    //        Console.WriteLine("File is not a valid image file");
    //        return;
    //    }
    //    
    //    img.Mutate(x => x.Resize(width, height*2));
//
    //    for (int y = 0; y < height; y++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            Rgba32 topPixel = img[x, y*2];
    //            Rgba32 bottomPixel = img[x, y*2+1];
    //            
    //            Console.Write($"\x1B[38;2;{topPixel.R};{topPixel.G};{topPixel.B};48;2;{bottomPixel.R};{bottomPixel.G};{bottomPixel.B}m▀\x1B[0m");
    //        }
    //    }
    //}

    public void Display()
    {
        Console.Clear();
        var height = Console.WindowHeight;
        var width = Console.WindowWidth;
        var horizontalPadding = (width - 40) / 2;
        var verticalPadding = (height - 49) / 2;

        if (horizontalPadding < 0) horizontalPadding = 0;
        if (verticalPadding < 0) verticalPadding = 0;
        
        
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
    }
    
    
}