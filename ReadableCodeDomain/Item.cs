namespace ReadableCodeDomain
{
    public class Item
    {
        public Item(int id, decimal cost)
        {
            Id = id;
            Cost = cost;
        }
        public int Id { get; private set; }
        public decimal Cost { get; private set; }
    }
}