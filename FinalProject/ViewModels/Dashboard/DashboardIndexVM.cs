using FinalProject.Entities;
using Microsoft.AspNetCore.Identity;
using ViewModels.Account;

namespace ViewModels.Dashboard
{
    public class DashboardIndexVM
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }

        public List<Musteri> CreditList { get; set; }
        public List<Musteri> CashList { get; set; }

    }
}
