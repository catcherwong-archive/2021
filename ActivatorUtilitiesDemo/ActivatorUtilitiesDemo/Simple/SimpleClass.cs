namespace ActivatorUtilitiesDemo
{
    internal class SimpleClass
    {
        private readonly int num = 0;

        public SimpleClass()
        { 
            num = 1;
        }

        public SimpleClass(int n)
        {
            num = n;
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(SimpleClass)}-Do-{num}");
        }
    }
}
