namespace ActivatorUtilitiesDemo
{
    using Microsoft.Extensions.DependencyInjection;

    internal static class SimpleCase
    {
        internal static void Nomal_Class()
        {
            Console.WriteLine($"Begin {nameof(Nomal_Class)}");

            IServiceCollection services = new ServiceCollection();
            var provider = services.BuildServiceProvider();

            var a1 = ActivatorUtilities.CreateInstance(provider, typeof(SimpleInterface));

            ((ISimpleInterface)a1).Do();

            Console.WriteLine($"End {nameof(Nomal_Class)}\n");
        }

        internal static void Nomal_Class_With_Interface()
        {
            Console.WriteLine($"Begin {nameof(Nomal_Class_With_Interface)}");

            IServiceCollection services = new ServiceCollection();

            services.Add(new ServiceDescriptor(typeof(ISimpleInterface), typeof(SimpleInterface), ServiceLifetime.Transient));

            var provider = services.BuildServiceProvider();

            var a1 = ActivatorUtilities.CreateInstance(provider, typeof(SimpleInterface));

            ((ISimpleInterface)a1).Do();

            Console.WriteLine($"End {nameof(Nomal_Class_With_Interface)}\n");
        }

        internal static void Nomal_Class_With_Multi_Constructor()
        {
            Console.WriteLine($"Begin {nameof(Nomal_Class_With_Multi_Constructor)}");

            IServiceCollection services = new ServiceCollection();
            var provider = services.BuildServiceProvider();

            var obj = ActivatorUtilities.CreateInstance(provider, typeof(SimpleClass));
            ((SimpleClass)obj).Do();

            var obj2 = ActivatorUtilities.CreateInstance(provider, typeof(SimpleClass), 99);
            ((SimpleClass)obj2).Do();

            Console.WriteLine($"End {nameof(Nomal_Class_With_Multi_Constructor)}\n");
        }

        internal static void Class_With_ActivatorUtilitiesConstructor()
        {
            Console.WriteLine($"Begin {nameof(Class_With_ActivatorUtilitiesConstructor)}");

            IServiceCollection services = new ServiceCollection();
            var provider = services.BuildServiceProvider();

            try
            {
                var obj = ActivatorUtilities.CreateInstance(provider, typeof(SimpleClass2));
                ((SimpleClass2)obj).Do();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
           
            var obj2 = ActivatorUtilities.CreateInstance(provider, typeof(SimpleClass2), 99, false);
            ((SimpleClass2)obj2).Do();

            Console.WriteLine($"End {nameof(Class_With_ActivatorUtilitiesConstructor)}\n");
        }

        internal static void Abst_Case()
        {
            Console.WriteLine($"Begin {nameof(Abst_Case)}");

            IServiceCollection services = new ServiceCollection();
            var provider = services.BuildServiceProvider();

            try
            {
                var obj = ActivatorUtilities.CreateInstance(provider, typeof(AbstClass));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine($"End {nameof(Abst_Case)}\n");
        }
    }
}
