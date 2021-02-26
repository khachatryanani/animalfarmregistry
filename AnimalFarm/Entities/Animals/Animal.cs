using System;
using Creatures.Types;
using Creatures.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Creatures.Entities
{
    public class Animal: Creature
    {
        // Animal Type property that hold enum value of AnimalType
        [JsonConverter(typeof(StringEnumConverter))]
        public AnimalType Type { get;  set; }

        // Animal Group property that hold enum value of AnimalGroup
        [JsonConverter(typeof(StringEnumConverter))]
        public AnimalGroup Group { get;  set;}

        // Animal Name property of stirng type
        public override string Name { get; set ; }

        // Animal Class constructor with parameters
        public Animal(AnimalType animalType, AnimalGroup animalGroup, string animalName) 
        {
            Type = animalType;
            Group = animalGroup;
            Name = animalName;
        }

        // Parameterless Animal class constructor
        public Animal() 
        {

        }

        // Gets the basic info
        public override string GetInfo()
        {
            string  info = "I am a creature of type ANIMAL.";
            return info;
        }
    }    
}
