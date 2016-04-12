using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace praktika2
{
    class Game
    {        
        int[,] arr;
        private int sum = 1;     
        private double Length;
        public int find1;
        public int find2;
        private int f1;
        private int f2;
        private int d1;
        private int d2;
        public Game(int size)
        {
            Length = Math.Sqrt(size);           
            arr = new int[(int)Length, (int)Length];            
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    arr[i, j] = sum;
                    sum++;                   
                }                
            }
            arr[(int)Length - 1, (int)Length - 1] = 0;
        }
        public int this[int index1, int index2]
        {
            get
            {
                return arr[index1, index2];
            }

            set
            {
                arr[index1, index2] = value;
            }
        }
        public void Print()
        {
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    Console.Write(arr[i, j] + "\t");                                    
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public Tuple <int, int> GetLocation(int value)
        {
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if (arr[i, j] == value) { find1 = i; find2 = j; }                   
                }               
            }
            var loc = Tuple.Create(find1, find2);
            return loc;           
        }
        public void Shift(int value)
        {
            f1 = GetLocation(value).Item1;
            f2 = GetLocation(value).Item2;
            d1 = GetLocation(0).Item1;
            d2 = GetLocation(0).Item2;

            if (Convert.ToBoolean(Math.Abs(f1 - d1)) ^ (Convert.ToBoolean(Math.Abs(f2 - d2))))
            {
                arr[f1, f2] = 0;
                arr[d1, d2] = value;
            }
            else
            {
                throw new Exception();
            }
           // Console.WriteLine(GetLocation(value).Item1);
           // Console.WriteLine(GetLocation(value).Item2);
        }        
    }
    class Program
    {
        static void Main(string[] args)
        {
            Game g = new Game(16);           
             g.Print();
             g.Shift(12);
            g.Print();
            g.Shift(11);
            g.Print();
             g.Shift(10);
             g.Print();
             g.Shift(6);
             g.Print();
             g.Shift(7);
             g.Print();
            g.Shift(12);
            g.Print();
            Console.ReadLine();
        }
    }
}
