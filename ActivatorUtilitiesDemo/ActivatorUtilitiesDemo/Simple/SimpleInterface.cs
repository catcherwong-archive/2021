namespace ActivatorUtilitiesDemo
{
    internal class SimpleInterface : ISimpleInterface
    {
        public void Do()
        {
            Console.WriteLine($"{nameof(SimpleInterface)}-Do");
        }
    }
}
