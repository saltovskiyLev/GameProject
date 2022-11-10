using System.Windows.Controls;

namespace Квест_2022
{
    public class ShopItemUI : StackPanel
    {
        public TextBlock ItemName = new TextBlock();
        public TextBlock ItemPrice = new TextBlock();
        public ShopItemUI(string _itemName, int _itemPrice)
        {
            ItemName.Text = _itemName;
            ItemPrice.Text = _itemPrice.ToString();
            this.Orientation = Orientation.Vertical;
            this.Children.Add(ItemName);
            this.Children.Add(ItemPrice);
            this.Margin = new System.Windows.Thickness(10, 5, 10, 0);
            ItemName.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            ItemPrice.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
        }
    }
}