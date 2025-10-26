#pragma warning disable CS8618

using System.Xml.Serialization;

namespace lab3
{
    public class Punctuation
    {
        [XmlText]
        public string Value { get; set; } = string.Empty;

        [XmlIgnore]
        public int PositionIndex { get; set; }

        public Punctuation() { }

        public Punctuation(string value, int positionIndex)
        {
            Value = value;
            PositionIndex = positionIndex;
        }

        public void Print()
        {
            Console.Write(Value);
        }
    }
}
