using Newtonsoft.Json;

namespace DeveLineStateSaver
{
    public class Argument
    {
        public string Value { get; set; }
        public string Type { get; set; }

        public Argument()
        {
        }

        public Argument(string type, object value)
        {
            Type = type;
            Value = JsonConvert.SerializeObject(value);
        }

        public bool IsEqualTo(Argument other)
        {
            if (Type != other.Type || !Value.Equals(other.Value))
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return $"{Type} {Value}";
        }
    }
}