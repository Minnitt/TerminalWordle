namespace Wordle;

using Logic;

internal static class Program
{
    static void Main(string[] args)
    {
        WordleGame game = new WordleGame();
        game.Game();
    }
}