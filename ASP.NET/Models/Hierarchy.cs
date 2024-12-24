namespace ASP.NET.Models
{
    public class Hierarchy
    {
        public int Id { get; set; }
        public int ParentObjId { get; set; }
        public string Path { get; set; }
    }
}
