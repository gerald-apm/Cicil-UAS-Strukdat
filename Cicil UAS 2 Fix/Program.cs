using System;
using System.Globalization;

namespace Cicilan_UAS_1
{
    public class Paket
    {
        public int[] dec;
        public string biner;
        public bool centang;
        public Paket next;
        public Paket(int[] angka, string bin)
        {

            dec = new int[angka.Length];
            for (int hitung = 0; hitung < angka.Length; hitung++)
            {
                dec[hitung] = angka[hitung];
            }
            biner = bin;
            next = null;
        }
    }

    public class SingleLink
    {
        public Paket Head;
        public Paket Tail;
        public Paket Baru;
        public Paket Bantu;

        //Seleksi PI
        public Paket St;
        public Paket Di;

        public void MasukkanData(int[] angka, string bin)
        {
            if (Head == null && Tail == null)
            {
                Baru = new Paket(angka, bin);
                Head = Baru;
                Tail = Baru;
            }
            else
            {
                Baru = new Paket(angka, bin);
                Tail.next = Baru;
                Tail = Baru;
            }
        }

        public void CetakData(bool pi)
        {
            Bantu = Head;
            while (Bantu != null)
            {
                if (pi)
                {
                    if (Bantu.centang == false)
                    {
                        for (int hitung = 0; hitung < Bantu.dec.Length; hitung++)
                        {
                            Console.Write(Bantu.dec[hitung]);
                            if (!(hitung == Bantu.dec.Length - 1))
                                Console.Write(",");
                        }
                        Console.Write("\t");
                        Console.Write(Bantu.biner);
                        Console.Write("  ");
                        if (Bantu.centang)
                            Console.Write("v");
                        else
                            Console.Write("-");
                        Console.WriteLine();
                    }
                }
                else
                {
                    for (int hitung = 0; hitung < Bantu.dec.Length; hitung++)
                    {
                        Console.Write(Bantu.dec[hitung]);
                        if (!(hitung == Bantu.dec.Length - 1))
                            Console.Write(",");
                    }
                    Console.Write("\t");
                    Console.Write(Bantu.biner);
                    Console.Write("  ");
                    if (Bantu.centang)
                        Console.Write("v");
                    else
                        Console.Write("-");
                    Console.WriteLine();
                }
                Bantu = Bantu.next;
            }
        }

        public void SeleksiPI()
        {
            St = Head;
            while (St != null)
            {
                char[] satu = St.biner.ToCharArray();
                Di = St.next;
                int cekk = 0;
                while (Di != null)
                {
                    char[] dua = Di.biner.ToCharArray();
                    if (CekBandingChar(satu, dua))
                    {
                        char[] baru = new char[Di.biner.Length];
                        baru = BandingkanChar(satu, dua);
                        string biner = new string(baru);

                        if (biner == St.biner)
                        {
                            St.centang = true;
                            Di.centang = false;
                        }
                        else
                        {
                            int[] decbaru = new int[St.dec.Length + Di.dec.Length];
                            Array.Copy(St.dec, decbaru, St.dec.Length);
                            Array.Copy(Di.dec, 0, decbaru, St.dec.Length, Di.dec.Length);
                            MasukkanData(decbaru, biner);
                            St.centang = true;
                            Di.centang = true;

                        }

                    }
                    Di = Di.next;
                    cekk++;
                }
                St = St.next;
            }
        }

        private bool CekBandingChar(char[] satu, char[] dua)
        {
            int cekh = 0;
            int ceksatu = 0;
            int cekdua = 0;
            for (int i = 0; i < satu.Length; i++)
            {
                if (satu[i] == '1')
                    ceksatu++;
                else if (dua[i] == '1')
                    cekdua++;
                if ((((satu[i] == '0') && (dua[i] == 'X')) || ((satu[i] == '1') && (dua[i] == 'X'))) || (((satu[i] == 'X') && (dua[i] == '1')) || ((satu[i] == 'X') && (dua[i] == '0'))))
                    return false;
                else if (satu[i] != dua[i])
                    cekh++;
            }
            if (cekh > 1)
                return false;

            return true;
        }

        private char[] BandingkanChar(char[] satu, char[] dua)
        {
            char[] hasil = new char[satu.Length];
            for (int i = 0; i < satu.Length; i++)
            {

                if (satu[i] == dua[i])
                    hasil[i] = satu[i];
                else if (((satu[i] == '0') && (dua[i] == '1')) || ((satu[i] == '1') && (dua[i] == '0')))
                    hasil[i] = 'X';
            }
            return hasil;
        }

        private bool CekSama(int a1, int a2)
        {
            return (a1 == a2);
        }

        private void CetakHasil(string bin)
        {
            char[] pers = bin.ToCharArray();
            int hh = 0;
            for (int j = 0; j < pers.Length; j++)
            {
                if (pers[j] != ' ')
                {
                    if (pers[j] == '1')
                        Console.Write((char)(65 + hh));
                    else if (pers[j] == '0')
                        Console.Write((char)(65 + hh) + "^");
                    hh++;
                }
            }
        }

        public void CariEPI(int[] minterm)
        {
            int[] bindua = new int[minterm.Length];
            int[] bintiga = new int[minterm.Length];
            int hitung = 0;
            int jumlahpers = 0;

            //Cari banyaknya "1" dalam array minterm
            int jumlahmin = 0;
            foreach (int ab in minterm)
            {
                if (ab == 1)
                    jumlahmin++;
            }

            // init ke nol
            for (int ac = 0; ac < minterm.Length; ac++)
            {
                bindua[ac] = 0;
                bintiga[ac] = 0;
            }

            Bantu = Head;
            hitung = 0;
            while (Bantu != null)
            {
                if (Bantu.centang == false)
                    hitung++;
                Bantu = Bantu.next;
            }
            jumlahpers = hitung;
           // Console.WriteLine(jumlahpers);
            int[,] tabelepi = new int[jumlahpers, minterm.Length];
            string[] epibin = new string[jumlahpers];
            Bantu = Head;
            hitung = 0;
            while (Bantu != null)
            {
                if (Bantu.centang == false)
                {
                    foreach (uint dec in Bantu.dec)
                    {
                        tabelepi[hitung, dec] = 1;
                    }
                    epibin[hitung] = Bantu.biner;
                    hitung++;
                }
                Bantu = Bantu.next;
            }
            //CetakTabelEPI(tabelepi, minterm, jumlahpers, minterm.Length);

            //CetakPI(epibin);
            BacktrackEPI(tabelepi, epibin, bindua, bintiga, minterm, jumlahpers, jumlahmin, 0);

        }

        public void CetakPI(string[] epi)
        {
            foreach (string ab in epi)
            {
                CetakHasil(ab);
                Console.Write(" + ");
            }
        }

        public bool BacktrackEPI(int[,] epi, string[] binn, int[] hasil, int[] past, int[] minterm, int jumlahpers, int jumlahmin, int jumjwb)
        {
            //Base return
            if (jumlahpers == 0)
                return false;

            if (jumjwb == jumlahmin)
            {
                CetakHasil(binn[jumlahpers - 1]);
                return true;
            }

            for (int htg = 0; htg < jumlahpers; htg++)
            {
                for (int gbg = 0; gbg < minterm.Length; gbg++)
                {
                    past[gbg] = hasil[gbg];
                }

                if (jumjwb <= jumlahmin)
                {

                    for (int gbg = 0; gbg < minterm.Length; gbg++)
                    {
                        hasil[gbg] |= epi[htg, gbg];
                    }

                    jumjwb = 0;
                    for (int gbg = 0; gbg < minterm.Length; gbg++)
                    {
                        if ((hasil[gbg] & minterm[gbg]) == 1)
                        {
                            jumjwb += 1;
                        }
                    }
                   //Console.WriteLine("htg = " + htg + " jumpers = " + jumlahpers);
                    //Console.WriteLine();

                    if (BacktrackEPI(epi, binn, hasil, past, minterm, jumlahpers - 1, jumlahmin, jumjwb) == true)
                    {
                        if (jumlahpers >= 0)
                        {
                            Console.Write(" + ");
                            CetakHasil(binn[jumlahpers - 1]);
                        }
                    }

                    for (int gbg = 0; gbg < minterm.Length; gbg++)
                    {
                        hasil[gbg] = past[gbg];
                    }
                }
            }
            return false;
        }

        public void CetakTabelEPI(int[,] tabel, int[] mint, int baris, int kolom)
        {
            for (int kl = 0; kl < kolom; kl++)
            {
                Console.Write(mint[kl] + "  ");
            }
            Console.WriteLine();
            for (int br = 0; br < baris; br++)
            {
                for (int kl = 0; kl < kolom; kl++)
                {
                    Console.Write(tabel[br, kl] + "  ");
                }
                Console.WriteLine();
            }
        }
    }

    class Mahayadnya
    {
        // Konversi dari decimal ke binary
        public int[] Biner(int hex, int jumlah)
        {
            int[] digits = new int[jumlah];
            int i = 0;
            while (hex > 0)
            {
                digits[i] = hex % 2;
                i++;
                hex /= 2;
            }
            return digits;
        }

        //Membalik value binary mengingat pembacaan dimulai dari bawah
        public int[] BalikBiner(int[] a, int jumlah)
        {
            int[] digits = new int[jumlah];
            int i, j = jumlah - 1;
            for (i = 0; i < jumlah; i++)
            {
                digits[i] = a[j];
                j -= 1;
            }
            return digits;
        }

        //Perpangkatan bilangan
        public int Power(int angka, int pangkat)
        {
            if (pangkat == 0)
                return 1;
            else
                return angka * Power(angka, pangkat - 1);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            //Instance untuk balik biner
            Mahayadnya gey;
            gey = new Mahayadnya();

            //SingeLink untuk Penampung minterm & Don't care
            SingleLink ay;
            ay = new SingleLink();
            int[] desimal;

            //Declare var, not war
            int digits, digithex, banyakbit, dcare, hitung;
            int[] hasil, minterm1, dcare1, hexbil;
            char[] hasilhex, dcarehex;
            bool cekHex;
            string angka, hexcek;
            Console.WriteLine("Cicilan UAS 1");

            //input banyak variabel
            do
            {
                Console.Write("Banyak Variabel [2..20] : ");
                angka = Console.ReadLine();
                if (int.TryParse(angka, out banyakbit))
                    banyakbit = int.Parse(angka);
                else
                {
                    Console.Write("Input Salah, Ulangi ! [enter] ");
                    Console.ReadLine();
                }
            } while (!(int.TryParse(angka, out banyakbit)) || !(banyakbit <= 20) || !(banyakbit > 2));

            //menentukan banyak bit, dan banyak digit hex
            digits = gey.Power(2, banyakbit);
            digithex = digits / 4;
            Console.WriteLine("Digit : " + digits);
            Console.WriteLine("Max Hex : " + digithex);

            //deklarasi array penyimpan hasil konversi hex
            hasil = new int[digits];
            minterm1 = new int[digits];
            dcare1 = new int[digits];

            //deklarasi array penyimpan digit Hex
            hasilhex = new char[digithex];
            dcarehex = new char[digithex];

            //deklarasi array penyimpan sementara konversi hex - binary
            hexbil = new int[4];


            //Bagian Input bilangan hex
            bool cekCare = false;
            do
            {
                cekHex = false;
                do
                {
                    //Cek jumlah digit apakah sesuai dengan yang semestinya 
                    do
                    {
                        Console.WriteLine();
                        Console.Write("Minterm [HEX] : ");
                        for (int j = 0; j < hasilhex.Length; j++)
                            hasilhex[j] = '\0';
                        angka = Console.ReadLine();
                        hasilhex = angka.ToCharArray();
                        if (!(angka.Length == digithex))
                        {
                            Console.Write("Digit tidak sesuai ketentuan ! [enter]");
                            Console.ReadLine();
                        }
                    } while (!(angka.Length == digithex));

                    //Cek Hexadesimal apakah dapat dikonversi
                    hitung = 0;
                    for (int i = 0; i < hasilhex.Length; i++)
                    {
                        dcare = 0;

                        //ubah char -> string
                        hexcek = hasilhex[i].ToString(CultureInfo.InvariantCulture);
                        if (int.TryParse(hexcek, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out dcare))
                        {
                            //string -> Hex integer
                            dcare = int.Parse(hexcek, NumberStyles.HexNumber);

                            //integer -> binary
                            hexbil = gey.Biner(dcare, 4);
                            hexbil = gey.BalikBiner(hexbil, 4);

                            //Masukkan array of binary kedalam hasil array
                            for (int j = 0; j < 4; j++)
                            {
                                minterm1[hitung] = hexbil[j];
                                hitung++;
                            }
                            cekHex = false;
                        }
                        else
                        {
                            cekHex = true;
                            Console.Write("Digit Hex salah ! [enter]");
                            Console.ReadLine();
                            break;
                        }
                    }
                } while (cekHex);


                //Proses konversi sama seperti pada minterm
                cekHex = false;
                do
                {
                    do
                    {
                        Console.WriteLine();
                        Console.Write("Don't Care [HEX] : ");
                        for (int j = 0; j < hasilhex.Length; j++)
                            hasilhex[j] = '\0';
                        angka = Console.ReadLine();
                        hasilhex = angka.ToCharArray();
                        if (!(angka.Length == digithex))
                        {
                            Console.Write("Digit tidak sesuai ketentuan ! [enter]");
                            Console.ReadLine();
                        }
                    } while (!(angka.Length == digithex));

                    hitung = 0;
                    for (int i = 0; i < hasilhex.Length; i++)
                    {
                        dcare = 0;
                        hexcek = hasilhex[i].ToString(CultureInfo.InvariantCulture);
                        if (int.TryParse(hexcek, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out dcare))
                        {
                            dcare = int.Parse(hexcek, NumberStyles.HexNumber);
                            hexbil = gey.Biner(dcare, 4);
                            hexbil = gey.BalikBiner(hexbil, 4);
                            for (int j = 0; j < 4; j++)
                            {
                                dcare1[hitung] = hexbil[j];
                                hitung++;
                            }
                            cekHex = false;
                        }
                        else
                        {
                            cekHex = true;
                            Console.Write("Digit Hex salah ! [enter]");
                            Console.ReadLine();
                            break;
                        }
                    }
                } while (cekHex);

                //Checking apakah terdapat overlapping pada minterm & don't care
                for (int j = 0; j < hitung; j++)
                {
                    if ((minterm1[j] & dcare1[j]) == 1)
                    {
                        cekCare = true;
                        Console.Write("Input Don't Care Salah ! [enter]");
                        Console.ReadLine();
                        Console.WriteLine();
                        break;
                    }
                    else
                    {
                        cekCare = false;

                        //Gabungkan kedua hasil menjadi satu, dengan :
                        //0 -> maxterm
                        //1 -> minterm
                        //2 -> don't care
                        if (dcare1[j] == 1)
                            hasil[j] = 2;
                        else
                            hasil[j] = minterm1[j];
                    }

                }
            } while (cekCare);

            //deklarasi jumlah baru untuk menyimpan bit untuk printing
            hexbil = new int[banyakbit];
            Console.WriteLine();
            Console.WriteLine("Tabel Kebenaran");
            //Print table header
            Console.Write("N  ");
            for (int i = 0; i < banyakbit; i++)
            {
                Console.Write(((char)(65 + i)) + " ");
            }
            Console.WriteLine(" Z");

            //Print tabel kebenaran
            for (int i = 0; i < digits; i++)
            {
                Console.Write(i + " ");
                if (i < 10)
                    Console.Write(" ");
                //cetak biner per angka
                hexbil = gey.Biner(i, banyakbit);
                hexbil = gey.BalikBiner(hexbil, banyakbit);
                foreach (int cetak in hexbil)
                {
                    Console.Write(cetak + " ");
                }

                Console.Write(" ");
                if (hasil[i] == 2)
                    Console.Write("X");
                else
                    Console.Write(hasil[i]);
                Console.WriteLine();
            }

            //Interpretasi kedalam SingeLink -- Tahap 2
            desimal = new int[1];
            string bin;
            int cekvcc = 0;
            for (int i = 0; i < digits; i++)
            {
                if (hasil[i] == 1 || hasil[i] == 2)
                {
                    desimal[0] = i;
                    hexbil = gey.Biner(i, banyakbit);
                    hexbil = gey.BalikBiner(hexbil, banyakbit);
                    bin = string.Join(" ", hexbil);
                    cekvcc++;
                    ay.MasukkanData(desimal, bin);
                }
            }
            Console.WriteLine();

            if (cekvcc == digits)
            {
                Console.WriteLine("Solusi : ");
                Console.WriteLine(" Logic 1 : VCC");
                Console.ReadLine();
                return;
            }
            else if (cekvcc == 0)
            {
                Console.WriteLine("Solusi : ");
                Console.WriteLine(" Logic 0 : GND");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Hasil Seleksi Minterm & Don't care");
            //Print table header
            Console.Write("N" + "\t");
            for (int i = 0; i < banyakbit; i++)
            {
                Console.Write(((char)(65 + i)) + " ");
            }
            Console.WriteLine(" Z");
            //Print hasil SingLink
            ay.CetakData(false);

            //tahap 3
            ay.SeleksiPI();

            Console.WriteLine();

            Console.WriteLine("Prime Implicant : ");
            //Print table header
            Console.Write("N\t");
            for (int i = 0; i < banyakbit; i++)
            {
                Console.Write(((char)(65 + i)) + " ");
            }
            Console.WriteLine(" Z");
            //Print hasil SingLink
            ay.CetakData(false);

            Console.WriteLine();

            Console.WriteLine("Solusi : ");

            ay.CariEPI(minterm1);

            Console.WriteLine();
            Console.WriteLine("TEST");
            Console.ReadLine();
        }
    }
}
