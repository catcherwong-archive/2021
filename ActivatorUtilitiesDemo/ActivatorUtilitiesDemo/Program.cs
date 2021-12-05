namespace ActivatorUtilitiesDemo
{
    using System;

    public class Program
    {
        public static void Main()
        {
            SimpleCase.Abst_Case();
            SimpleCase.Nomal_Class();
            SimpleCase.Nomal_Class_With_Interface();
            SimpleCase.Nomal_Class_With_Multi_Constructor();            
            SimpleCase.Class_With_ActivatorUtilitiesConstructor();

            DependencyCase.Nomal();
            DependencyCase.With_DI();
            DependencyCase.With_Multi_Constructor();

            Console.ReadKey();
        }        
    }
}
