using System;
using System.Reflection;
using Creatures;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load("Creatures");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Type[] type = assembly.GetTypes();
            foreach (var item in type)
            {
                try
                {
                    if (item.BaseType.ToString().Contains("Creatures.Entities")) 
                    {

                        Console.WriteLine(item.Name);
                        Console.WriteLine(item.BaseType);
                        Console.WriteLine(item.FullName);
                        Console.WriteLine("---");


                        object myObject = Activator.CreateInstance(item);
                        MethodInfo methodInfo = item.GetMethod("GetInfo");
                        Console.WriteLine(methodInfo.Invoke(myObject, null));
                        Console.WriteLine("--------------------------");
                    }

                }
                
                catch (MemberAccessException) 
                {

                }
                


            }
        }
    }
}
