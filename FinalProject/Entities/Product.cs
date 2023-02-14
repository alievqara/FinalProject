using FinalProject.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Entities
{
    public class Product : BaseEntity
    {
        public string Brand_Name { get; set; }
        public string Model_Name { get; set; }
        public decimal Price { get; set; }
        public string Detail { get; set; }
        public bool Stock_Sale { get; set; } // true dursa Stockdadir falcedirse Satilibdir.
        public bool  Yararsiz { get; set; }
        public int CategoryID { get; set; }
        public Category? Category { get; set; }
        public bool AddSaleBasket { get; set; }
        public string? UserSale { get; set; }


    }
}
