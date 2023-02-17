using FinalProject.Entities.Base;

namespace FinalProject.Entities
{
    public class Musteri : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? FatherName { get; set; }
        public string Tel { get; set; }
        public string? HomeTel { get; set; }
        public string? ZaminTel { get; set; }
        public bool? Is_Yeri { get; set; }
        public string? Is_Yerinin_Adi { get; set; }
        public double? Is_Staji { get; set; }
        public decimal UmumiDeyer_Borc { get; set; }
        public List<Product> Products { get; set; }
        public bool Kredit_Nagd { get; set; }
        public int? Kredit_Muddeti { get; set; }
        public int? Kredit_Faizi { get; set; }
        public DateTime? OdemeTarixi { get; set; }
        public decimal? IlkinOdenis { get; set; }
        public string Satici { get; set; }


    }
}
