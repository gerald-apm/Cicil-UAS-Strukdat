using System;
using System.Globalization;

namespace Tes_input
{
    class Program
    {
        static void Main(string[] args)
        {
            string bilangan;
            int banyakvariabel;
            char[] minterm_char;
            char[] dontcare_char;
            Console.WriteLine("Cicilan UAS 1");

            //-----INPUT BILANGAN---
            do
            {
                Console.Write("Banyak Variabel [2..20] : ");
                bilangan = Console.ReadLine();
                if (int.TryParse(bilangan, out banyakvariabel) == true)
                {
                    banyakvariabel = int.Parse(bilangan);
                    Console.WriteLine("Bilangan Bisa diubah ! ");
                }
                else
                {
                    Console.WriteLine("Bilangan tak bisa diubah ! ");
                    Console.WriteLine();
                }
            } while((int.TryParse(bilangan, out banyakvariabel) == false) || (banyakvariabel < 2) || (banyakvariabel > 20));
            //----INPUT BILANGAN---   
            Console.WriteLine();
            Console.WriteLine("Bilangan = " + banyakvariabel );
            

            //jumlah persamaan
            //casting
            int jumlahpersamaan = (int)Math.Pow(2, banyakvariabel);

            //jumlah digit HEX
            int digithex = jumlahpersamaan / 4;
            Console.WriteLine("Digit HEX = " + digithex);
            Console.WriteLine();

            string hexa;
            minterm_char = new char[digithex];
            dontcare_char = new char[digithex];
            int[] hexsementara = new int[4];
            int[] hexfix = new int[4];
            int[] minterm = new int[jumlahpersamaan];
            int[] dontcare = new int[jumlahpersamaan];
            int[] hasil = new int[jumlahpersamaan];

            bool cek_konflik = false;
            do
            {

                bool cek_konversi = false;
                do
                {
                    do
                    {
                        Console.Write("Minterm [HEX] = ");
                        hexa = Console.ReadLine();
                        if (hexa.Length == digithex)
                        {
                            Console.WriteLine("Digit Sesuai !");
                            minterm_char = hexa.ToCharArray();
                        }
                        else
                            Console.WriteLine("Panjang Digit tidak sah !");
                    } while (hexa.Length != digithex);

                    int sementara;
                    int hitungterm = 0;
                    string str_sementara;
                    for (int apa = 0; apa < digithex; apa++)
                    {
                        hexsementara = new int[4];
                        hexfix = new int[4];
                        str_sementara = minterm_char[apa].ToString();
                        if (int.TryParse(str_sementara, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out sementara) == true)
                        {
                            cek_konversi = true;
                            sementara = int.Parse(str_sementara, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                            Console.WriteLine("Bilangan Bisa diubah ! ");
                            Console.WriteLine(sementara);
                            //ubah banyakvariabel ke bilangan binary

                            int cek = 0;
                            while (sementara > 0)
                            {
                                hexsementara[cek] = sementara % 2;
                                cek++;
                                sementara = sementara / 2;
                            }

                            cek = 4;
                            for (int apaya = 0; apaya < 4; apaya++)
                            {
                                cek--;
                                hexfix[apaya] = hexsementara[cek];
                            }

                            for (int apaya = 0; apaya < 4; apaya++)
                            {
                                Console.Write(hexfix[apaya] + " ");
                            }
                            Console.WriteLine();

                            for (int apaya = 0; apaya < 4; apaya++)
                            {
                                minterm[hitungterm] = hexfix[apaya];
                                hitungterm++;
                            }
                            //SELESAI
                        }
                        else
                        {
                            cek_konversi = false;
                            Console.WriteLine("Bilangan tak bisa diubah ke Desimal ");
                            Console.WriteLine();
                            break;
                        }
                    }
                } while (cek_konversi == false);

                //input Dont Care
                cek_konversi = false;
                do
                {
                    do
                    {
                        Console.Write("Don't Care [HEX] = ");
                        hexa = Console.ReadLine();
                        if (hexa.Length == digithex)
                        {
                            Console.WriteLine("Digit Sesuai !");
                            dontcare_char = hexa.ToCharArray();
                        }
                        else
                            Console.WriteLine("Panjang Digit tidak sah !");
                    } while (hexa.Length != digithex);

                    int sementara;
                    int hitungterm = 0;
                    string str_sementara;
                    for (int apa = 0; apa < digithex; apa++)
                    {
                        hexsementara = new int[4];
                        hexfix = new int[4];
                        str_sementara = dontcare_char[apa].ToString();
                        if (int.TryParse(str_sementara, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out sementara) == true)
                        {
                            cek_konversi = true;
                            sementara = int.Parse(str_sementara, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                            Console.WriteLine("Bilangan Bisa diubah ! ");
                            Console.WriteLine(sementara);
                            //ubah banyakvariabel ke bilangan binary

                            int cek = 0;
                            while (sementara > 0)
                            {
                                hexsementara[cek] = sementara % 2;
                                cek++;
                                sementara = sementara / 2;
                            }

                            cek = 4;
                            for (int apaya = 0; apaya < 4; apaya++)
                            {
                                cek--;
                                hexfix[apaya] = hexsementara[cek];
                            }

                            for (int apaya = 0; apaya < 4; apaya++)
                            {
                                Console.Write(hexfix[apaya] + " ");
                            }
                            Console.WriteLine();

                            for (int apaya = 0; apaya < 4; apaya++)
                            {
                                dontcare[hitungterm] = hexfix[apaya];
                                hitungterm++;
                            }
                            //SELESAI
                        }
                        else
                        {
                            cek_konversi = false;
                            Console.WriteLine("Bilangan tak bisa diubah ke Desimal ");
                            Console.WriteLine();
                            break;
                        }
                    }
                } while (cek_konversi == false);

                for (int apaya = 0; apaya < jumlahpersamaan; apaya++)
                {
                    if ((minterm[apaya] & dontcare[apaya]) == 1)
                    {
                        Console.WriteLine("Persamaan Konflik !!!");
                        cek_konflik = true;
                        break;
                    }
                    else
                    {
                        cek_konflik = false;
                        if (dontcare[apaya] == 1)
                            hasil[apaya] = 2;
                        else
                            hasil[apaya] = minterm[apaya];
                    }
                }

            } while (cek_konflik == true);

            Console.WriteLine();

            int temporer = 0;
            hexsementara = new int[banyakvariabel];
            for (int apaya = 0; apaya < jumlahpersamaan; apaya++)
            {
                temporer = apaya;
                Console.Write(apaya + " ");

                //Console.Write(minterm[apaya] + " ");
                //Console.Write(dontcare[apaya] + " ");

                int cek = 0;
                while (temporer > 0)
                {
                    hexsementara[cek] = temporer % 2;
                    cek++;
                    temporer = temporer / 2;
                }

                for (int tes = banyakvariabel - 1; tes >=0; tes--)
                {
                    Console.Write(hexsementara[tes] + " ");
                }

                if (hasil[apaya] == 2)
                    Console.WriteLine("X");
                else
                    Console.WriteLine(hasil[apaya]);
            }

            Console.ReadLine();
        }
    }
}
