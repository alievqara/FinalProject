using FinalProject.Entities.Base;

namespace FinalProject.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public bool isMain { get; set; }
        public List<Phone>? phoneList {get; set;}
        public List<Computer>? Computers { get; set; }
        public List<OtherProduct>? OtherProducts { get; set; }
        public List<Category>? Categories { get; set; }
        public int? CategoryId { get; set; }
    }

}
