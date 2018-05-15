using System;
using System.Collections.Generic;

namespace DeveLineStateSaver.ConsoleApp
{
    public class ComplexObject
    {
        public string Name { get; set; }
        public DateTime Age { get; set; }
        public int Timer { get; set; }

        public override bool Equals(object obj)
        {
            var @object = obj as ComplexObject;
            return @object != null &&
                   Name == @object.Name &&
                   Age == @object.Age &&
                   Timer == @object.Timer;
        }

        public override int GetHashCode()
        {
            var hashCode = -354006332;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Age.GetHashCode();
            hashCode = hashCode * -1521134295 + Timer.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"ComplexObject, Name: {Name}, Age: {Age}, Timer: {Timer}";
        }
    }
}