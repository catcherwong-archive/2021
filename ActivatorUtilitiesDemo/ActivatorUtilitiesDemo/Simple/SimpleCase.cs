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


        internal static void GetServiceOrCreateInstance_Should_CallGetService()
        {
            Console.WriteLine($"Begin {nameof(GetServiceOrCreateInstance_Should_CallGetService)}");

            IServiceCollection services = new ServiceCollection();
            services.AddTransient<ISimpleInterface, SimpleInterface>();
            var provider = services.BuildServiceProvider();

            var obj = ActivatorUtilities.GetServiceOrCreateInstance<ISimpleInterface>(provider);
            obj.Do();

            Console.WriteLine($"End {nameof(GetServiceOrCreateInstance_Should_CallGetService)}\n");
        }

        internal static void GetServiceOrCreateInstance_Will_CreateInstance()
        {
            Console.WriteLine($"Begin {nameof(GetServiceOrCreateInstance_Will_CreateInstance)}");

            IServiceCollection services = new ServiceCollection();
            var provider = services.BuildServiceProvider();

            var obj = ActivatorUtilities.GetServiceOrCreateInstance(provider, typeof(SimpleInterface));
            ((ISimpleInterface)obj).Do();

            Console.WriteLine($"End {nameof(GetServiceOrCreateInstance_Will_CreateInstance)}\n");
        }

        internal static void CreateFactory_Case()
        {
            Console.WriteLine($"Begin {nameof(CreateFactory_Case)}");

            IServiceCollection services = new ServiceCollection();
            var provider = services.BuildServiceProvider();

            try
            {
                var factory = ActivatorUtilities.CreateFactory(typeof(SimpleClass), Type.EmptyTypes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var factory2 = ActivatorUtilities.CreateFactory(typeof(SimpleClass), new Type[] { typeof(int) });
            var obj2 = factory2.Invoke(provider, new object[] { 99 });
            ((SimpleClass)obj2).Do();

            var factory3 = ActivatorUtilities.CreateFactory(typeof(SimpleClass2), new Type[] { typeof(int), typeof(bool) });
            var obj3 = factory3.Invoke(provider, new object[] { 100, false });
            ((SimpleClass2)obj3).Do();

            try
            {
                var factory4 = ActivatorUtilities.CreateFactory(typeof(SimpleClass2), Type.EmptyTypes);
                factory4.Invoke(provider, Array.Empty<Type>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                var factory5 = ActivatorUtilities.CreateFactory(typeof(SimpleClass2), new Type[] { typeof(int) });
                var obj5 = factory5.Invoke(provider, new object[] { 120 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine($"End {nameof(CreateFactory_Case)}\n");
        }
    }
}
