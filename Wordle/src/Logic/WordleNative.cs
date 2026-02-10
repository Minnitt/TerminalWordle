using System.Runtime.InteropServices;

namespace Wordle.Logic;

internal static class WordleNative
{
    [DllImport("Wordle.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr Wordle_Create();

    [DllImport("Wordle.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void Wordle_Destroy(IntPtr instance);

    [DllImport("Wordle.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void Wordle_Attempt(
        IntPtr instance,
        string guess,
        char[] letters,
        int[] colours
    );

    [DllImport("Wordle.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern string Wordle_GetTargetWord(IntPtr instance);
}

public enum LetterColour
{
    Gray,
    Yellow,
    Green
}

public struct Tile
{
    public char Letter;
    public LetterColour Colour;
}

public sealed class Wordle : IDisposable
{
    private IntPtr _instance;

    public string GetTargetWord()
    {
        if (_instance != IntPtr.Zero)
            return WordleNative.Wordle_GetTargetWord(_instance);
        return "";
    }
    
    public Wordle()
    {
        _instance = WordleNative.Wordle_Create();
    }

    public Tile[] Attempt(string guess)
    {
        if (guess.Length != 5)
            throw new ArgumentException("Guess must be 5 characters long");
        
        char[] letters = new char[5];
        int[] colours = new int[5];

        WordleNative.Wordle_Attempt(_instance, guess, letters, colours);
        
        Tile[] result = new Tile[5];
        for (int i = 0; i < 5; i++)
        {
            result[i] = new Tile
            {
                Letter = letters[i],
                Colour = (LetterColour)colours[i]
            };
        }
        
        return result;
    }
    
    public void Dispose()
    {
        if (_instance != IntPtr.Zero)
        {
            WordleNative.Wordle_Destroy(_instance);
            _instance = IntPtr.Zero;
        }
        GC.SuppressFinalize(this);
    }
    
    ~Wordle()
    {
        Dispose();
    }
}