using System;
using System.Collections.Generic;
using System.Linq;
using AnimalFarm;
using AnimalFarm.Entities;

namespace Farming
{
    class Program
    {
        static void Main(string[] args)
        {
            //{
            //    Farm myFarm = new Farm();
            //    Animal dog = new Dog("Sunny");
            //    Animal cat = new Cat("Vunny");
            //    Animal duck = new Duck("Bunny");
            //    List<Animal> mylist = new List<Animal>();
            //    Dog d = new Dog("A");

            //    myFarm.Add(dog);
            //    myFarm.Add(cat);
            //    myFarm.Add(duck);
            //    foreach (Animal item in myFarm.animals)
            //    {
            //        Console.WriteLine(item.MakeASound() + " "+item.Name);
            //    }
            MammualGroup g = MammualGroup.Cow;
            Console.WriteLine(g.ToString());
        }
    }
}
