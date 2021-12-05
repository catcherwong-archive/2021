namespace ActivatorUtilitiesDemo
{
    using System;

    internal class DependencyClass
    {
        private readonly int _num = 0;
        private readonly IAClass _aClass;

        public DependencyClass(IAClass aClass)
        {
            _num = 1;
            _aClass = aClass;
        }

        public DependencyClass(int n, IAClass aClass)
        {
            _num = n;
            _aClass = aClass;
        }

        public DependencyClass(IAClass aClass, int n, bool flag)
        {
            _num = n;
            _aClass = aClass;
            Console.WriteLine("====" + flag);
        }

        public DependencyClass(IAClass aClass, int n, int n2)
        {
            _num = n2;
            _aClass = aClass;
            Console.WriteLine("====" + n);
        }

        public void Do()
        {
            Console.WriteLine(_num);
            _aClass.Do();
        }
    }
}
