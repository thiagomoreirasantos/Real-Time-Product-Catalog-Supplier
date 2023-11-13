using RealTimeProductCatalog.Model.Enums;

namespace RealTimeProductCatalog.Model.Entities
{
    public class Color
    {
        public Guid Id { get; set; }        
        public ColorTypes ColorType { get; set; }        
        public int Number { get; set; }        
        public string? Name { get; set; }        
        public bool Visible { get; set; }
    }
}