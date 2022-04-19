using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MenuPanel_For_TowerDefenc_
{
    public class MenuPanel : TabControl
    {
        Dictionary<string, MenuTab> Tabs = new Dictionary<string, MenuTab>();
        public void AddTab(string TabName)
        {
            MenuTab Tab = new MenuTab(TabName);
            Items.Add(Tab);
            Tabs.Add(TabName, Tab);
        }
        public void CreateNewItem(string text, string ImagePath, string ItemName, string TabName, Action <string> leftClickhHandler)
        {// картинка + текст
            MenuItem Item = new MenuItem(text, ImagePath, Tabs[TabName].ImageSize, leftClickhHandler);
            Tabs[TabName].AddItem(ItemName, Item);
            // принимает: Элемент(текст, путь к файлу с картинкой);
            
        }// картинка + текст

        public void SetImageSize(string TabName, int Size)
        {
            Tabs[TabName].ImageSize = Size;
        }
    }
}