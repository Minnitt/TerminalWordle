// See https://aka.ms/new-console-template for more information

using Wordle.Interface;

WordleInterface game = new WordleInterface(); 
game.InitialiseDisplay();

//game.AttemptInputHandle();
//string a = "012345689";
//Console.WriteLine(a[..5]);
//Console.WriteLine(a[5..]);
Console.ReadKey();
Console.Clear();

