using System;
using System.Globalization;

namespace Cicilan_UAS_Backtrack
{
    //deklarasi Class paket untuk SingleLinkList
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
        //Kebutuhan dasar singleLink
        private Paket Head;
        private Paket Tail;
        private Paket Baru;
        private Paket Bantu;

        //Seleksi PI
        private Paket St;
        private Paket Di;

        //inisialisasi SingleLink dengan status = null
        public SingleLink()
        {
            Head = null;
            Tail = null;
            Bantu = null;
            Baru = null;
            St = null;
            Di = null;
        }

        //Masukkan data ke dalam singleLink
        public void MasukkanData(int[] angka, string bin)
        {
            Baru = new Paket(angka, bin);
            if (Head == null && Tail == null)
            {
                //sisip awal
                Head = Baru;
                Tail = Baru;
            }
            else
            {
                //sisip belakang
                Tail.next = Baru;
                Tail = Baru;
            }
        }

        //Cetak data dari SingleLink
        public void CetakData(int banyakbit)
        {
            //Print table header
            Console.Write("N\t");
            for (int i = 0; i < banyakbit; i++)
            {
                Console.Write(((char)(65 + i)) + "  ");
            }
            Console.WriteLine(" Z");

            //Print hasil SingLink
            Bantu = Head;
            while (Bantu != null)
            {
                for (int hitung = 0; hitung < Bantu.dec.Length; hitung++)
                {
                    Console.Write(Bantu.dec[hitung]);
                    if (!(hitung == Bantu.dec.Length - 1))
                        Console.Write(",");
                }
                Console.Write("\t");
                Console.Write(Bantu.biner);
                Console.Write("   ");
                if (Bantu.centang)
                    Console.Write("v");
                else
                    Console.Write("-");
                Console.WriteLine();
                Bantu = Bantu.next;
            }
        }

        //Cetak Prime Implicant dengan return jumlah PI
        public int CetakPI(int banyakbit)
        {
            int jumlahpersamaan = 0;

            //Print table header
            Console.Write("N\t");
            for (int i = 0; i < banyakbit; i++)
            {
                Console.Write(((char)(65 + i)) + "  ");
            }
            Console.WriteLine(" Z");

            Bantu = Head;
            while (Bantu != null)
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
                    Console.Write("   ");
                    if (Bantu.centang)
                        Console.Write("v");
                    else
                        Console.Write("-");
                    Console.WriteLine();

                    jumlahpersamaan++;
                }
                Bantu = Bantu.next;
            }

            return jumlahpersamaan;
        }


        //Seleksi Prime Implicant
        public void SeleksiPI()
        {
            //Pakai paket satu dan paket dua untuk perbandingan, menggunakan "bubble sort"
            St = Head;
            while (St != null)
            {
                char[] satu = St.biner.ToCharArray();
                Di = St.next;
                while (Di != null)
                {
                    //Bandingkan apakah string biner di "satu" dan "dua" dapat dibandingkan ?
                    char[] dua = Di.biner.ToCharArray();
                    if (CekBandingChar(satu, dua) == true)
                    {
                        //Buat char baru sebagai hasil dari perbandingan
                        char[] baru = new char[Di.biner.Length];
                        baru = BandingkanChar(satu, dua); //banding char
                        string biner = new string(baru); //buat string dari array of char

                        //Jika Ternyata paket sama persis...
                        if (biner == St.biner)
                        {
                            //centang salah satu paketnya saja
                            St.centang = true;
                            Di.centang = false;
                        }
                        else
                        {
                            //Buat array baru dengan gabungan dari dec di paket satu dan paket dua
                            int[] decbaru = new int[St.dec.Length + Di.dec.Length];
                            Array.Copy(St.dec, decbaru, St.dec.Length);
                            Array.Copy(Di.dec, 0, decbaru, St.dec.Length, Di.dec.Length);

                            //bikin paket baru dan masukkan ke SLL
                            MasukkanData(decbaru, biner);

                            //centang semua paket
                            St.centang = true;
                            Di.centang = true;
                        }
                    }
                    Di = Di.next;
                }
                St = St.next;
            }
        }

        //Cek apakah char dapat dibandingkan
        private bool CekBandingChar(char[] satu, char[] dua)
        {
            //Tidak bisa dibandingkan jika 0 atau 1 bertemu dengan don't care "X"
            //Jika beda kurang dari atau sama dengan satu, maka bisa dibandingkan !
            //selain itu salah !
            int cekh = 0;
            for (int i = 0; i < satu.Length; i++)
            {
                if (((satu[i] == 'X') && (dua[i] != 'X')) || ((satu[i] != 'X') && (dua[i] == 'X')))
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
            //Membuat array of char baru untuk membandingkan char
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

        private void CetakHasil(string bin)
        {
            //Cetak Hasil di Tahap 5
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

        //Mencari Essential Prime Implicant
        public void CariEPI(int[] minterm, int banyakminterm, int jumlahpi)
        {
            int[] bindua = new int[minterm.Length];
            int[] bintiga = new int[minterm.Length];
            int hitung = 0;
            int jumlahpers = 0;

            //Cari banyaknya "1" dalam array minterm
            int jumlahmin = banyakminterm;

            jumlahpers = jumlahpi;

            //Tabel yang berisi mapping dari minterm yang telah diubah ke biner
            int[,] tabelepi = new int[jumlahpers, minterm.Length];

            //Array string dari persamaan, mapping mengikuti tabelepi
            string[] epibin = new string[jumlahpers];
            Bantu = Head;
            hitung = 0;
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

            //Sorting TabelEPI agar berindex dari jumlah minterm terbanyak menuju yang terkecil
            SortingTabel(tabelepi, minterm, epibin, jumlahpers, minterm.Length);

            //Cetak Tabel EPI
            Console.WriteLine("Tabel EPI :  ");
            CetakTabelEPI(tabelepi, minterm, jumlahpers, minterm.Length);
            Console.WriteLine();

            string[] tempbin;
            int[,] indexsementara;
            bool isKetemu = false;

            //Mulai BackTracking dari kombinasi satu-persatu hingga semua solusi ditemukan
            for (int a3 = 1; a3 <= jumlahpers; a3++)
            {
                tempbin = new string[a3];
                indexsementara = new int[a3, minterm.Length];
                if (BacktrackEPI(tabelepi, minterm, jumlahpers, jumlahmin, epibin, tempbin, a3, 0, indexsementara, 0) == true)
                {
                    isKetemu = true;

                    //hentikan looping tepat pada kombinasi terkecil (EPI)
                    break;
                }
            }

            if (isKetemu == false)
                Console.WriteLine("Solusi Tidak Ketemu !!!");
        }

        private void SortingTabel (int[,] tabelepi, int[]minterm, string[]epibin, int jumlahpers, int jumlahminterm)
        {
            //Sorting TabelEPI agar berindex dari jumlah minterm terbanyak menuju yang terkecil
            string tmp;
            int[] temp1 = new int[jumlahminterm];
            int[] temp2 = new int[jumlahminterm];
            for (int ab = 0; ab < jumlahpers; ab++)
            {
                for (int ac = ab + 1; ac < jumlahpers; ac++)
                {
                    //Tampung sementara nilai untuk dicari jumlah minterm yang tersedia
                    temp1 = AmbilTabel(tabelepi, ab, jumlahminterm);
                    temp2 = AmbilTabel(tabelepi, ac, jumlahminterm);
                    if (CekBanyakIsi(temp2, minterm) > CekBanyakIsi(temp1, minterm))
                    {
                        //Tukar data dari baris yang dituju (per kolom)
                        for (int ad = 0; ad < jumlahminterm; ad++)
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

        private int CekBanyakIsi(int[] urut, int[] minterm)
        {
            //Mencari banyaknya elemen bukan 0 didalam sebuah Array
            int posisi = 0;
            for (int aa = 0; aa < urut.Length; aa++)
            {
                if (urut[aa] != 0 && (minterm[aa] == 1))
                    posisi++;
            }
            return posisi;
        }

        private bool BacktrackEPI(int[,] tabelepi, int[] minterm, int jumlahpers, int jumlahmin, string[] epibin, string[] tempbin, int urut, int index, int[,] indexsementara, int posisi)
        {
            int[] cekmin = new int[minterm.Length];
            //compare 
            //Kalau data mentok..
            if (index == urut)
            {
                //masukkan ke dalam array sementara untuk dicocokkan
                for (int a0 = 0; a0 < urut; a0++)
                {
                    for (int a1 = 0; a1 < minterm.Length; a1++)
                    {
                        cekmin[a1] = cekmin[a1] | indexsementara[a0, a1];
                    }
                }

                //cocokkan sama minterm
                int hitung = 0;
                for (int a1 = 0; a1 < minterm.Length; a1++)
                {
                    if ((cekmin[a1] & minterm[a1]) == 1)
                        hitung++;
                }

               /* 
                //debug output : 
                for (int a1 = 0; a1 < minterm.Length; a1++)
                {
                    Console.Write(cekmin[a1] + " ");
                }
                Console.WriteLine();
                for (int a1 = 0; a1 < minterm.Length; a1++)
                {
                    Console.Write(minterm[a1] + " ");
                }
                Console.WriteLine();
                Console.WriteLine();
                */

                //Jika ternyata solusi ketemu...
                if (hitung == jumlahmin)
                {
                    Console.Write("Solusi Ketemu : ");
                    for (int a4 = 0; a4 < urut; a4++)
                    {
                        CetakHasil(tempbin[a4]);
                        if (a4 != urut - 1)
                            Console.Write(" + ");
                    }
                    Console.WriteLine();
                    return true;
                }
                else
                    return false;
            }

            //Kalau posisi lebih besar dari jumlah
            if (posisi >= jumlahpers)
                return false;

            //assign dari tabel EPI ke indexsementara (sebesar urut kombinasi)
            for (int a1 = 0; a1 < minterm.Length; a1++)
            {
                indexsementara[index, a1] = tabelepi[posisi, a1];
            }
            //assign tabel EPI strings
            tempbin[index] = epibin[posisi];

            
			//Memberi semua solusi yang dapat dicapai !!!
			//Pindah ke posisi index berikutnya dan mulai index baru di indexsementara
            bool a = BacktrackEPI(tabelepi, minterm, jumlahpers, jumlahmin, epibin, tempbin, urut, index + 1, indexsementara, posisi + 1);

            //Pindah ke posisi index berikutnya, jangan pindah index di 
            bool b = BacktrackEPI(tabelepi, minterm, jumlahpers, jumlahmin, epibin, tempbin, urut, index, indexsementara, posisi + 1);

            return (a || b);
       }

        private void CetakTabelEPI(int[,] tabel, int[] mint, int baris, int kolom)
        {
            //Cetak Tabel EPI
            //Baris pertama, persamaan minterm dipisah dengan 2 enter
            //Baris kedua sampai akhir, persamaan tabel Potential EPI sebagai calon solusi, 1 enter

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
                    Console.Write(tabel[br, kl] + "  ");

                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    class McCluskey
    {
        //SingleLink untuk Penampung minterm & Don't care
        static SingleLink single;

        //inisialisasi ulang SLL Setiap kali mengulang program
        private static void InitSingleLink()
        {
            single = new SingleLink();
        }

        // Konversi dari decimal ke binary
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

        //Cetak tabel kebenaran dengan return = banyak jumlah minterm
        public static int CetakTabelKebenaran(int banyakbit, int[] hasil)
        {
            int[] desimal = new int[1];
            string bin;
            int cekvcc = 0;

            int digits = Power(2, banyakbit);
            int[] hexbil = new int[banyakbit];
            Console.WriteLine("Tabel Kebenaran");
            Console.WriteLine();
            Console.Write("N  ");
            for (int i = 0; i < banyakbit; i++)
            {
                Console.Write(((char)(65 + i)) + "  ");
            }
            Console.WriteLine(" Z");

            //Print tabel kebenaran
            for (int i = 0; i < digits; i++)
            {
                Console.Write(i + " ");
                if (i < 10)
                    Console.Write(" ");
                //cetak biner per angka
                hexbil = Biner(i, banyakbit);
                foreach (int cetak in hexbil)
                {
                    Console.Write(cetak + "  ");
                }

                Console.Write(" ");
                if (hasil[i] == 2)
                    Console.Write("X");
                else
                    Console.Write(hasil[i]);
                Console.WriteLine();

                if (hasil[i] == 1 || hasil[i] == 2)
                {
                    desimal[0] = i;
                    hexbil = Biner(i, banyakbit);
                    bin = string.Join("  ", hexbil);

                    //jika hasil samadengan 1 --
                    if (hasil[i] == 1)
                        cekvcc++;

                    single.MasukkanData(desimal, bin);
                }
            }
            return cekvcc;
        }

        static void Main(string[] args)
        {
            string ulang = null;
            do
            {
                //Inisialisasi SingleLinkList
                InitSingleLink();

                //Declare var, not war
                int digits, digithex, banyakbit, tempbil, hitung;
                int[] hasil, minterm, dcare, hexbil;
                char[] hasilhex, dcarehex;
                bool cekHex;
                string angka, hexcek;

                Console.Clear();
                Console.WriteLine("UAS Struktur Data Fix : Gerald Mahayadnya (18410200053)");
                Console.WriteLine("Program Quine-McCluskey");
                Console.WriteLine();

                //input banyak variabel
                do
                {
                    Console.WriteLine();
                    Console.Write("Masukkan Banyak Variabel [2..20] : ");
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
                Console.WriteLine();
                Console.WriteLine("Banyak Digit Persamaan : " + digits);
                Console.WriteLine("Max Digit Hex : " + digithex);

                //deklarasi array penyimpan hasil konversi hex
                hasil = new int[digits];
                minterm = new int[digits];
                dcare = new int[digits];

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
                            Console.Write("Masukkan Minterm [HEX] : ");
                            hasilhex = null;

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
                                    minterm[hitung] = hexbil[j];
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
                            Console.Write("Masukkan Don't Care [HEX] : ");
                            hasilhex = null;

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
                                    dcare[hitung] = hexbil[j];
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
                        if ((minterm[j] & dcare[j]) == 1)
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
                            if (dcare[j] == 1)
                                hasil[j] = 2;
                            else
                                hasil[j] = minterm[j];
                        }

                    }
                } while (cekCare);

                //Print table header dan
                //Interpretasi kedalam SingleLink -- Tahap 2, return banyak vcc nya
                Console.WriteLine();
                int cekvcc = CetakTabelKebenaran(banyakbit, hasil);
                Console.WriteLine();

                if (cekvcc == digits)
                    Console.WriteLine("Solusi : Logic 1 -> VCC");
                else if (cekvcc == 0)
                    Console.WriteLine("Solusi : Logic 0 -> GND");
                else
                {
                    //Tahap 2
                    Console.WriteLine("Hasil Seleksi Minterm & Don't care");
                    single.CetakData(banyakbit);

                    //Seleksi Prime Implicant - tahap 3
                    single.SeleksiPI();
                    Console.WriteLine();
                    Console.WriteLine("Prime Implicant : ");
                    single.CetakData(banyakbit);

                    //cetak banyakbit dan mencari jumlah PI yang tersedia : 
                    int jumlahpi = single.CetakPI(banyakbit);

                    //Seleksi EPI dan cetak hasil - tahap 4 dan tahap 5
                    Console.WriteLine();
                    single.CariEPI(minterm, cekvcc, jumlahpi);

                    Console.WriteLine();
                    Console.WriteLine("Proses Selesai !");
                }

                Console.WriteLine();
                Console.Write("Ingin Mengulang Lagi ? [Y/N] : ");
                ulang = Console.ReadLine();
            } while (ulang == "y" || ulang == "Y");
        }
    }
}
