using System;
using System.Collections.Generic;
using System.Text;

namespace DeveLineStateSaver.ConsoleApp
{
    public class ComplexObject
    {
        public string Name { get; set; }
        public DateTime Age { get; set; }
        public int Timer { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as ComplexObject;
            if (other == null)
            {
                return false;
            }

            if (Name != other.Name || Age != other.Age || Timer != other.Timer)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"ComplexObject, Name: {Name}, Age: {Age}, Timer: {Timer}";
        }
    }
}
