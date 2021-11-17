using System;
using System.Globalization;

namespace Cicilan_UAS_1
{

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
            } while (!(int.TryParse(angka, out banyakbit)) || !(banyakbit <= 20) || !(banyakbit > 2) );

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

            Console.ReadLine();
        }
    }
}
