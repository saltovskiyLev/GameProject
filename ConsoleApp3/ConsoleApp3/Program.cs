// See https://aka.ms/new-console-template for more information
using System.Data;

Console.WriteLine("Hello, World!");

int r;
CreateRandom(out r);
void CreateRandom(out int random)
{
    Random r = new Random((int)DateTime.Now.Ticks);
    random = r.Next();
}

Console.WriteLine(r.ToString());