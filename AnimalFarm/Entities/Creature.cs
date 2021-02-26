using System;
using System.Collections.Generic;
using System.Text;

namespace Creatures.Entities
{
    // base abstact class of all creatures
    public abstract class Creature
    {
        // Abstract Property of creature's name
        public abstract string Name { get; set; }

        // Abstract Method for main info on creature
        public abstract string GetInfo();
    }
}
