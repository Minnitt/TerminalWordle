namespace Wordle.Logic;

public static class WordleFileManager
{
    public static string[] ReadWordList(string wordListFileName)
    {
        string[] wordList;
        try
        {
            wordList = File.ReadAllLines(wordListFileName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return [];
        }
        return wordList;
    }
}