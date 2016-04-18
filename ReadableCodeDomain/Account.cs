namespace ReadableCodeDomain
{
    public class Account
    {
        private Discount _discount = new FlatDiscount(0);

        public decimal ApplyDiscount(decimal cost)
        {
            return _discount.ApplyTo(cost);
        }
        public Discount Discount { set { _discount = value; } }
    }

    public class DefaultAccount : Account
    {

    }
}