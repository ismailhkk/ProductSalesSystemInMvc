using Microsoft.AspNetCore.Mvc;
using ProductSalesSystemInMvc.Models;
using System.Diagnostics;
using System.Reflection.Emit;


namespace ProductSalesSystemInMvc.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var urunler = GetUrunler();
            return View(urunler);
        }

        [HttpPost]
        public IActionResult Index(Satis model)
        {
            var urunler = GetUrunler();
            Urun alinacakUrun;
            
            foreach (var urun in urunler)
            {
                if (urun.UrunAd� == model.UrunAd�)
                {
                    alinacakUrun = urun;
                    break;
                }
            }

            if (alinacakUrun.Stok <= 0)
            {
                ViewData["Hata"] = "<div class=\"alert alert-warning\">Bu �r�n�n sto�u kalmad�.</div>";
                return View(urunler);
            }


            var paraUstu = model.OdenenTutar - alinacakUrun.Fiyat;

            if (paraUstu >= 0)
            {
                alinacakUrun.Stok--;
                UrunlerKaydetTxt(urunler);

                ViewData["Hata"] = $"<div class=\"alert alert-success\">Te�ekk�r ederiz. {(paraUstu > 0 ? $"Para �st�n�z {paraUstu} TL" : "")}</div>";
            }
            else
            {
                ViewData["Hata"] = $"<div class=\"alert alert-danger\">Paran�z yetersiz. {alinacakUrun.Fiyat - model.OdenenTutar} TL eksik.</div>";
            }
            var satis = new Satis
            {
                UrunAd� = model.UrunAd�,
                Adet = model.Adet,
                OdenenTutar = model.OdenenTutar,
            };

            SatisKaydetTxt(satis);


            return View("Index", urunler);
        }

        public List<Urun> GetUrunler()
        {
            var urunler = new List<Urun>();

            using StreamReader reader = new StreamReader("App_Data/UrunListesi.txt");
            {
                var urunlerTxt = reader.ReadToEnd();

                if (!string.IsNullOrEmpty(urunlerTxt))
                {
                    var urunListesi = urunlerTxt.Split('\n');

                    foreach (var urunlerSatir in urunListesi)
                    {
                        var urun = urunlerSatir.Split('|');


                        urunler.Add(new Urun
                        {
                            UrunAd� = urun[0],
                            Fiyat = int.Parse(urun[1]),
                            Stok = int.Parse(urun[2]),
                        });

                    }
                }
            }

            return urunler;
        }

        public void UrunlerKaydetTxt(List<Urun> urunler)
        {
            using StreamWriter writer = new StreamWriter("App_Data/UrunListesi.txt");
            {
                foreach (var urun in urunler)
                {
                    writer.WriteLine($"{urun.UrunAd�}|{urun.Fiyat}|{urun.Stok}");
                }
            }
        }

        public void SatisKaydetTxt(Satis satis)
        {
            using StreamWriter writer = new StreamWriter("App_Data/SatisListesi.txt");
            {
                writer.WriteLine($"�r�n Ad�: {satis.UrunAd�}  |Sat�lan �r�n: {satis.Adet}  |�denen Tutar: {satis.OdenenTutar}");
            }
        }
    }

}
