using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;


namespace Calendar
{
    class Program
    {
        struct Unnepek
        {
            public int UnNap;
            public int UnHonap;
        }
        static List<Unnepek> List_Unnep = new List<Unnepek>();

        const int STD_OUTPUT_HANDLE = -11;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        const string UNDERLINE = "\x1B[4m";
        const string RESET = "\x1B[0m";

        static int ev;
        static int honap;
        static int[,] naptar = new int[6, 7];
        private static DateTime datum;

        static void Main(string[] args)
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            uint mode;
            GetConsoleMode(handle, out mode);
            mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            SetConsoleMode(handle, mode);

            //Console.Write("Kérem az évet: ");
            ev = DateTime.Now.Year;
            //Console.Write("Kérem a hónapot: ");
            honap = DateTime.Now.Month; // Convert.ToInt32(Console.ReadLine());

            Feltolt();
            datum = new DateTime(ev, honap, 1);
            Fejresz();
            NaptarFeltoltes();
            Naptar();
            bool vege = false;
            do
            {
                ConsoleKeyInfo _Key = Console.ReadKey();
                switch (_Key.Key)
                {
                    case ConsoleKey.RightArrow:
                        Console.Clear();
                        if (honap == 12)
                        {
                            honap = 1;
                            ev++;
                        }
                        else
                            honap++;
                        Fejresz();
                        NaptarFeltoltes();
                        Naptar();
                        break;
                    case ConsoleKey.LeftArrow:
                        Console.Clear();
                        if (honap == 1)
                        {
                            honap = 12;
                            ev--;
                        }
                        else
                            honap--;
                        Fejresz();
                        NaptarFeltoltes();
                        Naptar();
                        break;
                    case ConsoleKey.DownArrow:
                        Console.Clear();
                        vege = true;
                        break;
                }
            } while (!vege);
        }

        static void Fejresz()
        {
            Console.ForegroundColor = ConsoleColor.White;
            //Console.Write("\n\n");
            Console.WriteLine(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(honap) + " " + ev);
            Console.WriteLine();
            Console.WriteLine(UNDERLINE + "Mo\tTu\tWe\tTh\tFr\tSa\tSu" + RESET);
        }

        static void NaptarFeltoltes()
        {
            int napok = DateTime.DaysInMonth(ev, honap);
            int mainap = 1;
            var nap_Het = (int)datum.DayOfWeek;
            for (int i = 0; i < naptar.GetLength(0); i++)
            {
                for (int j = 0; j < naptar.GetLength(1) && mainap - nap_Het + 1 <= napok; j++)
                {
                    if (i == 0 && honap > j)
                    {
                        naptar[i, j] = 0;
                    }
                    else
                    {
                        naptar[i, j] = mainap - nap_Het + 1;
                        mainap++;
                    }
                }
            }
        }

        static void Naptar()
        {
            for (int i = 0; i < naptar.GetLength(0); i++)
            {
                for (int j = 0; j < naptar.GetLength(1); j++)
                {
                    if (naptar[i, j] > 0)
                    {
                        if (j % 6 == 0 && j != 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else
                                Console.ForegroundColor = ConsoleColor.White;
                        if (naptar[i, j] < 10)
                        {
                            if (Unnep(honap, naptar[i, j]))
                                Console.Write(naptar[i, j] + "*\t");
                            else
                                Console.Write(" " + naptar[i, j] + "\t");
                        }
                        else
                        {
                            if (Unnep(honap, naptar[i, j]))
                                Console.Write(naptar[i, j] + "*\t");
                            else
                                Console.Write(naptar[i, j] + "\t");
                        }
                    }

                    else
                    {
                        Console.Write("\t");
                    }
                }
                Console.WriteLine("");
            }
        }
        static void Feltolt()
        {
            int[] napok = new int[]
            {
                1,1,3,15,3,30,4,1,4,2,5,1,5,20,5,21,8,20,10,23,11,1,12,25,12,26
            };
            for (int i = 0; i < napok.Length; i+=2)
            {
                Unnepek S = new Unnepek();
                S.UnHonap = napok[i];
                S.UnNap = napok[i + 1];
                List_Unnep.Add(S);
            }
        }
        static bool Unnep(int honap, int nap)
        {
            for (int i = 0; i < List_Unnep.Count; i++)
            {
                if (honap == List_Unnep[i].UnHonap)
                    if (nap == List_Unnep[i].UnNap)
                        return true;
            }
            return false;
        }
    }
}