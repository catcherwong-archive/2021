namespace ActivatorUtilitiesDemo
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    internal static class DependencyCase
    {
        internal static void Nomal()
        {
            Console.WriteLine($"Begin {nameof(Nomal)}");

            IServiceCollection services = new ServiceCollection();
            var provider = services.BuildServiceProvider();

            try
            {
                var obj = ActivatorUtilities.CreateInstance(provider, typeof(DependencyClass));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
           
            Console.WriteLine($"End {nameof(Nomal)}\n");
        }

        internal static void With_DI()
        {
            Console.WriteLine($"Begin {nameof(With_DI)}");

            IServiceCollection services = new ServiceCollection();
            services.AddTransient<IAClass, AClass>();
            var provider = services.BuildServiceProvider();

            var obj = ActivatorUtilities.CreateInstance(provider, typeof(DependencyClass));
            ((DependencyClass)obj).Do();
            /*
            1
            AClass-Do
           */

            Console.WriteLine($"End {nameof(With_DI)}\n");
        }

        internal static void With_Multi_Constructor()
        {
            Console.WriteLine($"Begin {nameof(With_Multi_Constructor)}");

            IServiceCollection services = new ServiceCollection();
            services.AddTransient<IAClass, AClass>();
            var provider = services.BuildServiceProvider();

            var obj = ActivatorUtilities.CreateInstance(provider, typeof(DependencyClass), 90);
            ((DependencyClass)obj).Do();
            /*
             90
             AClass-Do
             */

            var obj2 = ActivatorUtilities.CreateInstance(provider, typeof(DependencyClass), 66, false);
            ((DependencyClass)obj2).Do();
            /*
             ====False
             66
             AClass-Do
             */

            var obj3 = ActivatorUtilities.CreateInstance(provider, typeof(DependencyClass), false, 50);
            ((DependencyClass)obj3).Do();
            /*
             ====False
             50
             AClass-Do
             */


            var obj4 = ActivatorUtilities.CreateInstance(provider, typeof(DependencyClass), 100, 50);
            ((DependencyClass)obj4).Do();
            /*
             ====100
             50
             AClass-Do
             */

            Console.WriteLine($"End {nameof(With_Multi_Constructor)}\n");
        }

        internal static void CreateFactory_Case()
        {
            Console.WriteLine($"Begin {nameof(CreateFactory_Case)}");

            IServiceCollection services = new ServiceCollection();
            services.AddTransient<IAClass, AClass>();
            var provider = services.BuildServiceProvider();

            var factory = ActivatorUtilities.CreateFactory(typeof(DependencyClass2), Type.EmptyTypes);
            var obj = factory.Invoke(provider, Array.Empty<Type>());
            ((DependencyClass2)obj).Do();

            try
            {
                var factory2 = ActivatorUtilities.CreateFactory(typeof(DependencyClass3), Type.EmptyTypes);
                var obj2 = factory2.Invoke(provider, Array.Empty<Type>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var factory3 = ActivatorUtilities.CreateFactory(typeof(DependencyClass3), new Type[] { typeof(int), typeof(bool) });
            var obj3 = factory3.Invoke(provider, new object[] { 100, false });
            ((DependencyClass3)obj3).Do();

            Console.WriteLine($"End {nameof(CreateFactory_Case)}\n");
        }
    }
}
