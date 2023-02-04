namespace ggj.rootbeer
{
    public enum ToppingName { Orange_Peel, Coconut, Candy_Cane }
    public class Topping
    {
        public ToppingName Name { get; set; }
        public string Text { get; set; }

        public Topping(ToppingName name, string text = "")
        {
            Name = name;
            Text = text;
        }

        public string GetPrettyName()
        {
            return this.Name.ToString().Replace('_', ' ');
        }
    }
}
