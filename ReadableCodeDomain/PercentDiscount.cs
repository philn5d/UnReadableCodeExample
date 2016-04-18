using System;

namespace ReadableCodeDomain
{
    public class PercentDiscount : Discount
    {
        private decimal _percentDiscountAmount;

        public PercentDiscount(decimal percentDiscountAmount)
        {
            _percentDiscountAmount = percentDiscountAmount;
        }

        internal override decimal ApplyTo(decimal cost)
        {
            return cost - (cost * _percentDiscountAmount);
        }
    }
}