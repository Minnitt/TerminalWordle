// See https://aka.ms/new-console-template for more information

using Wordle.Interface;

WordleInterface game = new WordleInterface(); 
game.InitialiseDisplay();
game.PrintMessage("hello this is a message");
//string a = "012345689";
//Console.WriteLine(a[..5]);
//Console.WriteLine(a[5..]);
Console.ReadKey();
Console.Clear();

