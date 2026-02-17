namespace Wordle.Logic;

public enum LetterColour
{
    GRAY,
    YELLOW,
    GREEN,
    UNKNOWN
}

public struct LetterContainer(char letter, LetterColour colour)
{
    public char letter = letter;
    public LetterColour colour = colour;
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

    private LetterContainer[] attempt(string guess)
    {

        guess = guess.ToLower();
        guess = guess.Replace(" ", "");
        if (guess.Length != 5)
        {
            Console.WriteLine("Guess needs to be 5 letters long.");
            return [];
        }
        Dictionary<char, int> freqDist =  new Dictionary<char, int>();
        LetterContainer[] returnWord = new LetterContainer[guess.Length];
        foreach (char letter in targetWord)
        {
            freqDist[letter]++;
        }

        for (int i = 0; i < returnWord.Length; i++)
        {
            char letter = guess[i];
            returnWord[i].letter = letter;
            
            if (letter == targetWord[i]) {
                returnWord[i].colour = LetterColour.GREEN;
                freqDist[letter]--;
            } else if (freqDist[letter] > 0) {
                returnWord[i].colour = LetterColour.YELLOW;
                freqDist[letter]--;
            } else {
                returnWord[i].colour = LetterColour.GRAY;
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