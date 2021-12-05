namespace ActivatorUtilitiesDemo
{
    internal class SimpleClass2
    {
        private readonly int num = 0;

        public SimpleClass2()
        {
            num = 1;
        }

        public SimpleClass2(int n)
        {
            num = n;
        }

        [Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]
        public SimpleClass2(int n, bool b)
        {
            num = n / 2;
            Console.WriteLine($"{nameof(SimpleClass2)}-----{b}");
        }


        public void Do()
        {
            Console.WriteLine($"{nameof(SimpleClass)}-Do-{num}");
        }
    }
}
