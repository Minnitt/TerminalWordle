namespace Wordle.Interface;


using System.IO;
using System;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


public class WindowController
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
        
        string baseDir = AppContext.BaseDirectory;
        string resourcesDir = Path.Combine(baseDir, "Resources");
        string emptyCell = File.ReadAllText(Path.Combine(resourcesDir, "empty_cell.txt"));
        
        
        
        


    }

    WindowController()
    {
        Console.Clear();
    }
    
    
}