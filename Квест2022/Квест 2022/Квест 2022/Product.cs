using System.Windows.Controls;
using System.IO;
using System;
using System.Windows.Media.Imaging;

namespace Квест_2022
{
    class Product : StackPanel
    {
        Image image;
        TextBlock price;
        TextBlock description;

        public Product(string path, int productPrice, string productDescription)
        {
            image = new Image();
            var uri = new Uri(path);
            var img = new BitmapImage(uri);
            image.Source = img;
            image.Width = 100;
            image.Height = 100;
            price = new TextBlock();
            price.Text = productPrice.ToString();
            description = new TextBlock();
            description.Text = productDescription;

            Orientation = Orientation.Vertical;
            Children.Add(image);
            Children.Add(price);
            Children.Add(description);
            
        }
    }
}
