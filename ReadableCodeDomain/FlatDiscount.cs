using System;

namespace ReadableCodeDomain
{
    public class FlatDiscount : Discount
    {
        private decimal _flatDiscountAmount;

        public FlatDiscount(decimal flatDiscountAmount)
        {
            _flatDiscountAmount = flatDiscountAmount;
        }

        internal override decimal ApplyTo(decimal cost)
        {
            var discountedCost = cost - _flatDiscountAmount;
            return discountedCost < 0 ? 0 : discountedCost;
        }
    }
}