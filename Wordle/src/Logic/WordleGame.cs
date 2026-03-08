namespace Wordle.Logic;

public enum LetterColour
{
    Gray,
    Yellow,
    Green,
    Black,
    Unknown
}

public struct LetterContainer(string letter, LetterColour colour)
{
    public string Letter = letter;
    public LetterColour Colour = colour;
}

public class WordleGame
{
    private string[] totalWordList;
    private string[] validWordList;
    private string targetWord;
    private int currentAttemptCount;
    private LetterContainer[,] wordleGame;
    
    private string[] ReadFile(string fileName)
    {
        string[] wordList;
        try
        {
            wordList = File.ReadAllLines(fileName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return [];
        }
        return wordList;
    }

    private string RandomWord()
    {
        Random random = new Random();
        return validWordList[random.Next(0, validWordList.Length)];
    }

    public LetterContainer[] Attempt(string guess)
    {
        guess = guess.ToLower();
        guess = guess.Replace(" ", "");
        if (guess.Length != 5)
        {
            Console.WriteLine("Word needs to be 5 letters long.");
            return [];
        }

        if (!totalWordList.Contains(guess))
        {
            Console.WriteLine("Word is not a valid guess.");
        }
        
        var freqDist =  new Dictionary<string, int>();
        var returnWord = new LetterContainer[guess.Length];
        foreach (char letter in targetWord)
        {
            freqDist[letter.ToString()]++;
        }

        for (int i = 0; i < returnWord.Length; i++)
        {
            string letter = guess[i].ToString();
            returnWord[i].Letter = letter;
            
            if (letter == targetWord[i].ToString()) {
                returnWord[i].Colour = LetterColour.Green;
                freqDist[letter]--;
            } else if (freqDist[letter] > 0) {
                returnWord[i].Colour = LetterColour.Yellow;
                freqDist[letter]--;
            } else {
                returnWord[i].Colour = LetterColour.Gray;
            }
            
            wordleGame[currentAttemptCount, i] = returnWord[i];
        }
        
        currentAttemptCount++;
        return returnWord;
    }

    public WordleGame()
    {
        totalWordList = ReadFile("../resources/total.txt");
        validWordList = ReadFile("../resources/valid.txt");
        targetWord = RandomWord();
        wordleGame = new LetterContainer[5, 6];
        currentAttemptCount = 0;
    }
}