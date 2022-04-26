using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MenuPanel_For_TowerDefenc_
{
    public class MenuItem : StackPanel
    {
        Image image = new Image();
        TextBlock text = new TextBlock();
        Uri uri;
        BitmapImage img;
        string ItemName;
        Action<string> LeftClickHandler;
        public MenuItem(string text, string ImagePath, int ImageSize, Action<string> leftClickhHandler, string itemName)
        {
            this.Orientation = Orientation.Vertical;
            this.text.Text = text;
            MouseDown += Item_MouseDown;
            var uri = new Uri(ImagePath); 
            var img = new BitmapImage(uri);
            LeftClickHandler = leftClickhHandler;
            ItemName = itemName;
            image.Source = img;
            image.Width = ImageSize;
            Margin = new System.Windows.Thickness(7);
            Children.Add(image);
            Children.Add(this.text);
        }
        private void Item_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                LeftClickHandler(ItemName);
                //MessageBox.Show("Текст");
            }
        }
    }
}
