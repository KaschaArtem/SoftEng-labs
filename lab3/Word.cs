using System.Xml.Serialization;

namespace lab3
{
    public class Word
    {
        [XmlText]
        public string Value { get; set; } = string.Empty;

        [XmlIgnore]
        public int PositionIndex { get; set; }

        public Word() { }

        public Word(string value, int positionIndex)
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
