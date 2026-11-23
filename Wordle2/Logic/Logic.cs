namespace Wordle.Logic;

using Enums;

public class WordleGame
{
    private string[] totalWordList;
    private string[] validWordList;
    private string targetWord;
    private GameState gameState;
    private int attemptCount;
    
    private static string RandomWord(string[] wordlist)
    {
        var random = new Random();
        return wordlist[random.Next(0, wordlist.Length)];
    }

    private static string ReplaceFirstOfLetterWithSpace(string word, char letter)
    {
        int index = word.IndexOf(letter);
        return word[..index] + " " + word[(index + 1)..];
    }

    private LetterContainer[] GuessAttempt(string guess)
    {
        string targetWordCopy =  targetWord;
        LetterContainer[] result = new LetterContainer[targetWord.Length];
        guess = guess.Replace(" ", string.Empty);
        guess = guess.ToLower();
        
        if (guess.Length != 5)
        {
            throw new ArgumentException("Invalid word length");
        }
        
        if (guess.Any(c => !char.IsLetter(c)))
        {
            throw new ArgumentException("Word contains invalid characters");
        }

        if (!totalWordList.Contains(guess))
        {
            throw new Exception("Invalid word");
        }
        
        LetterContainer.SetWord(guess, result);
        LetterContainer.SetColours(result, LetterColour.Gray);
        
        //check logic
        //correct letters pass
        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] != targetWordCopy[i]) continue;
            result[i].Colour = LetterColour.Green;
            targetWordCopy = targetWordCopy[..i] + " " + targetWordCopy[(i + 1)..];
        }
        
        //present letters 
        for (int i = 0; i < guess.Length; i++)
        {
            if (result[i].Colour == LetterColour.Green) continue;
            
            if (targetWordCopy.Contains(guess[i]))
            {
                result[i].Colour = LetterColour.Yellow;
                targetWordCopy = ReplaceFirstOfLetterWithSpace(targetWordCopy, guess[i]);
            }
        }
        
        return result;
    }
    
    private static string EncodeLetterColour(LetterContainer letterContainer)
    {
        string colourEncodedLetter = "";
        switch (letterContainer.Colour)
        {
            case LetterColour.Green:
                colourEncodedLetter += "\e[48;2;83;141;78m";
                break;
            case LetterColour.Yellow:
                colourEncodedLetter += "\e[48;2;197;165;17m";
                break;
            case LetterColour.Gray:
                colourEncodedLetter += "\e[48;2;62;62;64m";
                break;
        }

        colourEncodedLetter += letterContainer.Letter;
        colourEncodedLetter += "\e[0m";
        
        return colourEncodedLetter;
    }

    private string EncodeWordColour(LetterContainer[] letterContainer)
    {
        return letterContainer.Aggregate("", (current, letter) => current + EncodeLetterColour(letter));
    }

    private void AttemptLoop()
    {
        try
        {
            //get user input
            attemptCount++;
            Console.Write("Enter word: ");
            string guess = Console.ReadLine();
            string colourEncodedWord = "";
            LetterContainer[] result = new LetterContainer[totalWordList.Length];
            result = GuessAttempt(guess);
            colourEncodedWord = EncodeWordColour(result);
            
            //display guess
            Console.WriteLine(colourEncodedWord);
            
            //check for win condition
            if(!result.Any(letter => letter.Colour == LetterColour.Gray ||  letter.Colour == LetterColour.Yellow))
            {
                gameState = GameState.Win;
            }
            else if(attemptCount < 6)
            {
                //not win and not lose, gamestate stays in attempt loop
            }
            else
            {
                gameState = GameState.Lose;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }

    public void Game()
    {
        gameState = GameState.Menu;
        Console.Clear();
        Console.WriteLine("Welcome to Wordle!");
        gameState = GameState.AttemptLoop;
        while (gameState == GameState.AttemptLoop)
        {
            AttemptLoop();
        }

        if (gameState == GameState.Win)
        {
            Console.WriteLine("You won!");
        }
        
        else if (gameState == GameState.Lose)
        {
            Console.WriteLine("You lost!");
        }
        
    }
    
    public WordleGame()
    {
        totalWordList = WordleFileManager.ReadWordList("./resources/total.txt");
        validWordList = WordleFileManager.ReadWordList("./resources/valid.txt");
        targetWord = RandomWord(validWordList);
        gameState = GameState.Unknown;
        attemptCount = 0;
    }
}