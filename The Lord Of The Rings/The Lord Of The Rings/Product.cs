using System.Windows.Controls;

namespace The_Lord_Of_The_Rings
{
    class Product : StackPanel
    {
        public TextBlock ItemName = new TextBlock();
        public TextBlock CostItems = new TextBlock();
        Product(string itemName, int costImems)
        {
            ItemName.Text = itemName;
            CostItems.Text = costImems.ToString();

            this.Orientation = Orientation.Vertical;

            this.Children.Add(ItemName);
            this.Children.Add(CostItems);
        }
    }
}