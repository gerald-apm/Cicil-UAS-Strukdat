using System;
using System.Globalization;

namespace Cicilan_UAS_FIX
{
    public class Paket
    {
        public int[] dec;
        public string biner;
        public bool centang;
        public Paket next;
        public Paket(int[] angka, string bin)
        {

            //Masukkan Semua isi ke dalam "PAKET" baru
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
            //True = Cetak PI Saja (False sign)
            //False = Cetak Semuanya
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
            //Seleksi dengan metode "bubble sorting", traverse semua SLL sampai tak tersisa
            St = Head;
            while (St != null)
            {
                //String to char
                char[] satu = St.biner.ToCharArray();
                Di = St.next;
                while (Di != null)
                {
                    //String to char
                    char[] dua = Di.biner.ToCharArray();

                    //Cek apakah char dapat dibandingkan 
                    if (CekBandingChar(satu, dua))
                    {
                        char[] baru = new char[Di.biner.Length];

                        //Bikin Char baru & string baru
                        baru = BandingkanChar(satu, dua);
                        string biner = new string(baru);

                        //Jika Ada paket yang isinya sama, maka centang salah satunya, jgn bikin paket lagi
                        if (biner == St.biner)
                        {
                            St.centang = true;
                            Di.centang = false;
                        }
                        else
                        {
                            //Bikin Nilai desimal yang baru (index > 1)
                            int[] decbaru = new int[St.dec.Length + Di.dec.Length];
                            Array.Copy(St.dec, decbaru, St.dec.Length);
                            Array.Copy(Di.dec, 0, decbaru, St.dec.Length, Di.dec.Length);

                            //Buat paket baru
                            MasukkanData(decbaru, biner);

                            //Centang Paket yang sudah diseleksi
                            St.centang = true;
                            Di.centang = true;
                        }
                    }
                    Di = Di.next;
                }
                St = St.next;
            }
        }

        private bool CekBandingChar(char[] satu, char[] dua)
        {
            int cekh = 0;
            //Kriteria Pembandingan :
            //Jika 0 ketemu X atau 1 ketemu X, maka status = false
            //Jika Beda Char lebih dari satu, maka tak dapat dibandingkan = false
            //Selain itu statusnya = true
            for (int i = 0; i < satu.Length; i++)
            {
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
            //kalau ketemu 0 dan 1, maka hasil = X
            //selain itu nilainya tetap
            for (int i = 0; i < satu.Length; i++)
            {
                if (satu[i] == dua[i])
                    hasil[i] = satu[i];
                else if (((satu[i] == '0') && (dua[i] == '1')) || ((satu[i] == '1') && (dua[i] == '0')))
                    hasil[i] = 'X';
            }
            return hasil;
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
                        Console.Write((char)(65 + hh) + "'");
                    hh++;
                }
            }
        }

        public void CariEPI(int[] minterm)
        {
            int[] bindua = new int[minterm.Length];
            int[] bintiga = new int[minterm.Length];
            int[] jumlahterm = new int[minterm.Length];
            int hitung = 0;
            int jumlahpers = 0;

            //Cari banyaknya "1" dalam array minterm
            int jumlahmin = 0;
            foreach (int ab in minterm)
            {
                if (ab == 1)
                    jumlahmin++;
            }

            Bantu = Head;
            hitung = 0;

            //Cari jumlah PI yang tersedia (jumlahpers)
            while (Bantu != null)
            {
                if (Bantu.centang == false)
                {
                    hitung++;
                }
                Bantu = Bantu.next;
            }
            jumlahpers = hitung;

            //Tabel Potential EPI
            int[,] tabelepi = new int[jumlahpers, minterm.Length];

            //String persamaan untuk cetak hasil
            string[] epibin = new string[jumlahpers];

            Bantu = Head;
            hitung = 0;

            //Memindah value dari SLL ke dalam Tabel EPI dan epibin
            while (Bantu != null)
            {
                if (Bantu.centang == false)
                {
                    foreach (uint dec in Bantu.dec)
                    {
                        //Seleksi hanya yang minterm saja yang dikeluarkan !!! (don't care excluded)
                        if (minterm[dec] == 1)
                            tabelepi[hitung, dec] = 1;
                        else
                            tabelepi[hitung, dec] = 0;
                    }
                    epibin[hitung] = Bantu.biner;
                    hitung++;
                }
                Bantu = Bantu.next;
            }

            //Sorting TabelEPI (bubble sort) DESCENDING
            string tmp;
            int[] temp1 = new int[minterm.Length];
            int[] temp2 = new int[minterm.Length];
            for (int ab = 0; ab < jumlahpers; ab++)
            {
                for (int ac = ab + 1; ac < jumlahpers; ac++)
                {
                    //Tampung sementara nilai untuk dicari jumlah minterm yang tersedia
                    temp1 = AmbilTabel(tabelepi, ab, minterm.Length);
                    temp2 = AmbilTabel(tabelepi, ac, minterm.Length);
                    if (CekBanyakIsi(temp2) > CekBanyakIsi(temp1))
                    {
                        //Tukar data dari baris yang dituju (per kolom)
                        for (int ad = 0; ad < minterm.Length; ad++)
                        {
                            tabelepi[ab, ad] = temp2[ad];
                            tabelepi[ac, ad] = temp1[ad];
                        }

                        //tukar string
                        tmp = epibin[ab];
                        epibin[ab] = epibin[ac];
                        epibin[ac] = tmp;
                    }
                }
            }

            //Cari jumlah total persamaan yang tersedia (akan dijadikan acuan backtracking)
            for (int ac = 0; ac < minterm.Length; ac++)
            {
                for (int ad = 0; ad < jumlahpers; ad++)
                {
                    jumlahterm[ac] += tabelepi[ad, ac];
                }
            }

            //Cetak urutan
            Console.WriteLine("Banyak Minterm dalam Prime Implicant : ");
            for (int ad = 0; ad < jumlahpers; ad++)
            {
                temp1 = AmbilTabel(tabelepi, ad, minterm.Length);
                Console.Write(CekBanyakIsi(temp1) + "  ");
            }
            Console.WriteLine();
            Console.WriteLine();

            //Cetak Tabel Potential EPI
            Console.WriteLine("Tabel EPI :  ");
            CetakTabelEPI(tabelepi, jumlahterm, minterm, jumlahpers, minterm.Length);
            Console.WriteLine();

            //Mencetak Solusi persamaan 
            //Mulai proses Backtracking 
            Console.Write("Solusi : ");

            //Mulai ketika jumlah total persamaan lebih dari 0
            while (CekBanyakIsi(jumlahterm) > 0)
            {
                //Traverse dari baris tabel EPI paling atas
                for (int ac = 0; ac < jumlahpers; ac++)
                {
                    //Kalau posisi index tabel EPI yang dituju ketemu...
                    if (tabelepi[ac, CariTerkecil(jumlahterm)] == 1)
                    {
                        //Cetak Hasilnya !!! (Solusi Ketemu)
                        CetakHasil(epibin[ac]);

                        //Ketika solusi ketemu, maka NOL-kan semua value yang berada di baris tabel EPI
                        //beserta di jumlahterm (menandakan bahwa sudah dikunjungi [visited])
                        for (int ad = 0; ad < minterm.Length; ad++)
                        {
                            if (tabelepi[ac, ad] == 1)
                            {
                                tabelepi[ac, ad] = 0;
                                jumlahterm[ad] = 0;
                            }
                        }
                        break;
                    }
                }
                //Cetak " + " ketika solusi lebih dari satu persamaan
                if (!(CekBanyakIsi(jumlahterm) == 0))
                    Console.Write(" + ");
            }
            Console.WriteLine();
        }

        private int[] AmbilTabel(int[,] tabelepi, int idx, int kolom)
        {
            //Ekstrak tabel epi ke dalam tabel satuan
            int[] tabs = new int[kolom];
            for (int ac = 0; ac < kolom; ac++)
            {
                tabs[ac] = tabelepi[idx, ac];
            }
            return tabs;
        }

        private int CariTerkecil(int[] urut)
        {
            //Cari bilangan terkecil dalam satu Array, mulai dari urutan paling awal
            int posisi = -1, pivot = 0, ace = 0;
            for (int aa = 0; aa < urut.Length; aa++)
            {
                if (urut[aa] != 0)
                {
                    ace = urut[aa];
                    pivot = aa;
                    break;
                }
            }

            for (int aa = 0; aa < urut.Length; aa++)
            {
                if (urut[aa] != 0)
                {
                    if (urut[aa] < ace)
                        pivot = aa;
                }
            }
            posisi = pivot;
            return posisi;
        }

        private int CekBanyakIsi(int[] urut)
        {
            //Mencari banyaknya elemen bukan 0 didalam sebuah Array
            int posisi = 0;
            for (int aa = 0; aa < urut.Length; aa++)
            {
                if (urut[aa] != 0)
                {
                    posisi++;
                }
            }
            return posisi;
        }

        public void CetakTabelEPI(int[,] tabel, int[] jumlahterm, int[] mint, int baris, int kolom)
        {
            //Cetak Tabel EPI
            //Baris pertama, persamaan minterm dipisah dengan 2 enter
            //Baris kedua sampai akhir, persamaan tabel Potential EPI sebagai calon solusi, 1 enter
            //Baris ketiga penjumlahan semua solusi secara vertikal

            //Minterm
            for (int kl = 0; kl < kolom; kl++)
            {
                Console.Write(mint[kl] + "  ");
            }
            Console.WriteLine();
            Console.WriteLine();
            
            //Tabel EPI
            for (int br = 0; br < baris; br++)
            {
                for (int kl = 0; kl < kolom; kl++)
                {
                    Console.Write(tabel[br, kl] + "  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            //Jumlah persamaan yang sama
            for (int kl = 0; kl < kolom; kl++)
            {
                Console.Write(jumlahterm[kl] + "  ");
            }
            Console.WriteLine();
        }
    }

    class Mahayadnya
    {
        // Konversi dari decimal ke binary
        //Membalik value binary mengingat pembacaan dimulai dari bawah
        public static int[] Biner(int hex, int jumlah)
        {
            //ubah dari desimal ke array integer
            int[] digits = new int[jumlah];
            int i = 0;
            while (hex > 0)
            {
                digits[i] = hex % 2;
                i++;
                hex /= 2;
            }

            //balik urutan array
            int[] fix = new int[jumlah];
            int j = jumlah - 1;
            for (i = 0; i < jumlah; i++)
            {
                fix[i] = digits[j];
                j -= 1;
            }
            return fix;
        }

        //Perpangkatan bilangan
        public static int Power(int angka, int pangkat)
        {
            if (pangkat == 0)
                return 1;
            else
                return angka * Power(angka, pangkat - 1);
        }

        static void Main(string[] args)
        {
            //SingeLink untuk Penampung minterm & Don't care
            SingleLink ayng;
            ayng = new SingleLink();
            int[] desimal;

            //Declare var, not war
            int digits, digithex, banyakbit, tempbil, hitung;
            int[] hasil, minterm1, dcare1, hexbil;
            char[] hasilhex, dcarehex;
            bool cekHex;
            string angka, hexcek;
            Console.WriteLine("Cicilan UAS FIX : Gerald Mahayadnya ");

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
            } while (!(int.TryParse(angka, out banyakbit)) || !(banyakbit <= 20) || !(banyakbit >= 2));

            //menentukan banyak bit, dan banyak digit hex
            digits = Power(2, banyakbit);
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
                        tempbil = 0;

                        //ubah char -> string
                        hexcek = hasilhex[i].ToString(CultureInfo.InvariantCulture);
                        if (int.TryParse(hexcek, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out tempbil))
                        {
                            //string -> Hex integer
                            tempbil = int.Parse(hexcek, NumberStyles.HexNumber);

                            //integer -> binary
                            hexbil = Biner(tempbil, 4);

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
                        tempbil = 0;
                        hexcek = hasilhex[i].ToString(CultureInfo.InvariantCulture);
                        if (int.TryParse(hexcek, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out tempbil))
                        {
                            tempbil = int.Parse(hexcek, NumberStyles.HexNumber);
                            hexbil = Biner(tempbil, 4);
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
            Console.Write("N\t");
            for (int i = 0; i < banyakbit; i++)
            {
                Console.Write(((char)(65 + i)) + " ");
            }
            Console.WriteLine("\tZ");

            //Print tabel kebenaran
            for (int i = 0; i < digits; i++)
            {
                Console.Write(i + "\t");
                //cetak biner per angka
                hexbil = Biner(i, banyakbit);
                foreach (int cetak in hexbil)
                {
                    Console.Write(cetak + " ");
                }

                Console.Write("\t");
                if (hasil[i] == 2)
                    Console.Write("X");
                else
                    Console.Write(hasil[i]);
                Console.WriteLine();
            }

            //Interpretasi kedalam SingleLink
            desimal = new int[1];
            string bin;
            int cekvcc = 0;
            for (int i = 0; i < digits; i++)
            {
                if (hasil[i] == 1 || hasil[i] == 2)
                {
                    desimal[0] = i;
                    hexbil = Biner(i, banyakbit);
                    bin = string.Join(" ", hexbil);
                    if (hasil[i] == 1)
                        cekvcc++;
                    ayng.MasukkanData(desimal, bin);
                }
            }
            Console.WriteLine();

            //Cek apakah hasil VCC / GND
            if (cekvcc == digits)
            {
                Console.Write("Solusi : ");
                Console.WriteLine(" Logic 1 : VCC");
                Console.ReadLine();
                return;
            }
            else if (cekvcc == 0)
            {
                Console.Write("Solusi : ");
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
            ayng.CetakData(false);

            ayng.SeleksiPI();

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
            ayng.CetakData(true);

            Console.WriteLine();

            //Cari Essential Prime Implicant
            ayng.CariEPI(minterm1);

            Console.WriteLine();
            Console.WriteLine();

            //Proses Selesai :D
            Console.WriteLine("SELESAI");
            Console.ReadLine();
        }
    }
}
