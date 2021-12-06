namespace ActivatorUtilitiesDemo
{
    using System;

    internal class DependencyClass2
    {
        private readonly int _num = 0;
        private readonly IAClass _aClass;

        public DependencyClass2(IAClass aClass)
        {
            _num = 1;
            _aClass = aClass;
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(DependencyClass2)}-{_num}");
            _aClass.Do();
        }
    }
}
