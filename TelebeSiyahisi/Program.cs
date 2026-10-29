using System;
using System.IO;
using System.Text;

namespace TelebeSiyahisi
{
    class Program
    {
        const string FayilAdi = "telebeler.txt";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== TƏLƏBƏ SİYAHISI =====");
                Console.WriteLine("1) Tələbə əlavə et");
                Console.WriteLine("2) Hamısını göstər");
                Console.WriteLine("3) Ada görə axtar");
                Console.WriteLine("4) Çıxış");
                Console.Write("Seçim: ");
                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1": TelebeElave(); break;
                    case "2": HamisiniGoster(); break;
                    case "3": AdaGoreAxtar(); break;
                    case "4":
                        Console.WriteLine("Proqram bağlandı.");
                        return;
                    default:
                        Console.WriteLine("Yanlış seçim! 1-4 arası daxil edin.");
                        break;
                }
            }
        }

        static void TelebeElave()
        {
            Console.Write("ID: ");
            string id = Console.ReadLine();

            Console.Write("Ad: ");
            string ad = Console.ReadLine();

            Console.Write("Yaş: ");
            string yasMetn = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(ad))
            {
                Console.WriteLine("ID və Ad boş ola bilməz.");
                return;
            }

            int yas;
            if (!int.TryParse(yasMetn, out yas) || yas <= 0 || yas > 120)
            {
                Console.WriteLine("Yaş düzgün daxil edilməyib.");
                return;
            }

            id = id.Trim();
            ad = ad.Trim();

            if (id.Contains(";") || ad.Contains(";"))
            {
                Console.WriteLine("ID və Ad daxilində ';' simvolu ola bilməz.");
                return;
            }

            try
            {

                if (File.Exists(FayilAdi))
                {
                    using (StreamReader sr = new StreamReader(FayilAdi, Encoding.UTF8))
                    {
                        string setir;
                        while ((setir = sr.ReadLine()) != null)
                        {
                            string[] hisseler = setir.Split(';');
                            if (hisseler.Length == 3 && hisseler[0] == id)
                            {
                                Console.WriteLine("Bu ID artıq mövcuddur: " + id);
                                return;
                            }
                        }
                    }
                }


                using (StreamWriter sw = new StreamWriter(FayilAdi, true, Encoding.UTF8))
                {
                    sw.WriteLine(id + ";" + ad + ";" + yas);
                }

                Console.WriteLine("Tələbə əlavə olundu.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Xəta baş verdi: " + ex.Message);
            }
        }

        static void HamisiniGoster()
        {
            if (!File.Exists(FayilAdi))
            {
                Console.WriteLine("Hələ heç bir tələbə yoxdur.");
                return;
            }

            try
            {
                int say = 0;
                Console.WriteLine();
                Console.WriteLine("{0,-8} {1,-20} {2,-5}", "ID", "Ad", "Yaş");
                Console.WriteLine(new string('-', 35));

                using (StreamReader sr = new StreamReader(FayilAdi, Encoding.UTF8))
                {
                    string setir;
                    while ((setir = sr.ReadLine()) != null)
                    {
                        string[] hisseler = setir.Split(';');
                        if (hisseler.Length != 3) continue; 
                        Console.WriteLine("{0,-8} {1,-20} {2,-5}", hisseler[0], hisseler[1], hisseler[2]);
                        say++;
                    }
                }

                if (say == 0)
                    Console.WriteLine("Hələ heç bir tələbə yoxdur.");
                else
                    Console.WriteLine("Cəmi: " + say + " tələbə");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fayl oxunarkən xəta: " + ex.Message);
            }
        }

        static void AdaGoreAxtar()
        {
            if (!File.Exists(FayilAdi))
            {
                Console.WriteLine("Hələ heç bir tələbə yoxdur.");
                return;
            }

            Console.Write("Axtarılan ad: ");
            string axtar = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(axtar))
            {
                Console.WriteLine("Ad boş ola bilməz.");
                return;
            }

            axtar = axtar.Trim();

            try
            {
                int tapildi = 0;
                using (StreamReader sr = new StreamReader(FayilAdi, Encoding.UTF8))
                {
                    string setir;
                    while ((setir = sr.ReadLine()) != null)
                    {
                        string[] hisseler = setir.Split(';');
                        if (hisseler.Length != 3) continue;

                        if (hisseler[1].IndexOf(axtar, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Console.WriteLine("ID: " + hisseler[0] + " | Ad: " + hisseler[1] + " | Yaş: " + hisseler[2]);
                            tapildi++;
                        }
                    }
                }

                if (tapildi == 0)
                    Console.WriteLine("Belə tələbə tapılmadı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Axtarış zamanı xəta: " + ex.Message);
            }
        }
    }
}