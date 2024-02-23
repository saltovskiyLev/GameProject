// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");






Product p1 = new Product();
Console.WriteLine(Product.counter);
Product p2 = new Product();
Console.WriteLine(Product.counter);
Product p3 = new Product();
Console.WriteLine(Product.counter);
Product p4 = p4 = Product.GetFromFile("...");

Console.ReadKey();

class Product
{
    public string Name { get; set; }

    public static int counter = 0;

    public int Id { get; private set; }

    public Product() 
    { 
       Id = counter++;
    }

    static public Product GetFromFile(string FileName)
    {
        Product p0 = new Product();
        return p0;
    }
}
