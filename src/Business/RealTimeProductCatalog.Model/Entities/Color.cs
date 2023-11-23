namespace RealTimeProductCatalog.Model.Entities
{
    public class Color
    {
        public Color(Guid id, ColorTypes colorType, int number, string? name, bool visible)
        {
            Id = id;
            ColorType = colorType;
            Number = number;
            Name = name;
            Visible = visible;
        }

        public Guid Id { get; private set; }
        public ColorTypes ColorType { get; private set; }
        public int Number { get; private set; }
        public string? Name { get; private set; }
        public bool Visible { get; private set; }
        private static readonly HashSet<string> ValidColorNames = new HashSet<string>
        {
            "Red",
            "Green",
            "Blue",
            "Yellow",
            "Orange",
            "Purple",
            "Black",
            "White",
        };

        public static bool IsValidColorName(string name)
        {
            return ValidColorNames.Contains(name);
        }
    }
}