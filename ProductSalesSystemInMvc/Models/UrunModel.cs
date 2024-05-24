using System.ComponentModel.DataAnnotations;

namespace ProductSalesSystemInMvc.Models
{
	public class Urun
	{
		public string UrunAdı { get; set; }
		public int Fiyat { get; set; }
		public int Stok { get; set; }
	}
}
