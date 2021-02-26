using System;
using System.Collections.Generic;
using System.Text;
using Creatures.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Creatures.Entities
{
    public class Mammual: Animal
    {
        // New Mammual Group property that hold enum value of Mammual Group
        new public MammualGroup Group { get; private set; }

        // Parameterized Mammual Class constructor
        public Mammual(MammualGroup mammualGroup, string mammualName) : base()
        {
            Type = AnimalType.Mammual;
            Group = mammualGroup;
            //Name = mammualName;
        }

        // Parameterless Mammual Class constructor
        public Mammual()
        {
            Type = AnimalType.Mammual;
        }

        // Gets the basic info
        public override string GetInfo()
        {
            string info = "I am an animal of type MAMMUAL.";
            return info;
        }
    }
    
}
