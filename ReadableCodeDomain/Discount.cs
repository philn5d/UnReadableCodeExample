using System;

namespace ReadableCodeDomain
{
    public abstract class Discount
    {
        internal abstract decimal ApplyTo(decimal cost);
    }
}