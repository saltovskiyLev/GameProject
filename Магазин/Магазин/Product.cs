using System;

namespace Магазин
{
    public class Product
    {
        public int Price { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool IsAvailible
        {
            get
            {
                if (Number > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string GetInfo()
        {
            string Info = "";
            Info = "название: " + Name + "\n";
            Info += "цена: " + Price.ToString() + "\n";
            Info += "количество на складе: " + Number.ToString();
            return Info;
        }
        public void SetFields(string item)
        {
            string[] Fields = item.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            Name = Fields[0];
            Price = int.Parse(Fields[1]);
            Number = int.Parse(Fields[2]);
            Picture = Fields[3];
        }
    }
}
