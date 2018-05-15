using System;
using System.Collections.Generic;

namespace DeveLineStateSaver.Tests
{
    public class ComplexObjectTest
    {
        public string Name { get; set; }
        public DateTime Age { get; set; }
        public int Timer { get; set; }

        public override bool Equals(object obj)
        {
            var test = obj as ComplexObjectTest;
            return test != null &&
                   Name == test.Name &&
                   Age == test.Age &&
                   Timer == test.Timer;
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