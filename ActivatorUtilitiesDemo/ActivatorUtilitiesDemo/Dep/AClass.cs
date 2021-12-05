namespace ActivatorUtilitiesDemo
{
    using System;

    internal class AClass : IAClass
    {
        public void Do()
        {
            Console.WriteLine($"{nameof(AClass)}-Do");
        }
    }
}
