using Creatures.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Creatures.Entities
{
    public class Bird: Animal
    {
        // New Birds Group property that hold enum value of BirdGroup
        new public BirdGroup Group { get; private set; }

        // Parameterized Mammual Class constructor
        public Bird(BirdGroup birdGroup, string birdName)
        {
            Type = AnimalType.Bird;
            Group = birdGroup;
            Name = birdName;
        }

        // Parameterless Mammual Class constructor
        public Bird()
        {
            Type = AnimalType.Bird;
        }

        // Gets the basic info
        public override string GetInfo()
        {
            string info = "I am an animal of type BIRD.";
            return info;
        }
    }
}

