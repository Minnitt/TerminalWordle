namespace Wordle.Logic;

using Enums;

public struct LetterContainer(string letter, LetterColour colour)
{
    public string Letter = letter;
    public LetterColour Colour = colour;

    public static void SetWord(string word, LetterContainer[] container)
    {
        for (int i = 0; i < Math.Min(container.Length, word.Length); i++)
        {
            container[i].Letter = word[i].ToString();
        }
    }

    public static void SetColours(LetterContainer[] container, LetterColour colour)
    {
        for (int i = 0; i < container.Length; i++)
        {
            container[i].Colour = colour;
        }
    }
}