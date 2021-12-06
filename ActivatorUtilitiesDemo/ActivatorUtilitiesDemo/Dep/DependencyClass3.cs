namespace ActivatorUtilitiesDemo
{
    using System;

    internal class DependencyClass3
    {
        private readonly int _num = 0;
        private readonly IAClass _aClass;

        public DependencyClass3(IAClass aClass, int n, bool flag)
        {
            _num = n;
            _aClass = aClass;
            Console.WriteLine("====" + flag);
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(DependencyClass3)}-{_num}");
            _aClass.Do();
        }
    }
}
