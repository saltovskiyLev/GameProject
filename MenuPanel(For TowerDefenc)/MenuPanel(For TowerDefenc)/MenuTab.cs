using System.Windows.Controls;
using System.Collections.Generic;

namespace MenuPanel_For_TowerDefenc_
{
    public class MenuTab : TabItem
    {
        private Dictionary<string, MenuItem> Items = new Dictionary<string, MenuItem>();
        private WrapPanel Panel = new WrapPanel();
        public int ImageSize = 40;
        public MenuTab(string TabName)
        {
            Content = Panel;
            Header = new TextBlock { Text = TabName };
        }
        public void AddItem(string name, MenuItem item)
        {
            Items.Add(name, item);
            Panel.Children.Add(item);
        }
    }
}