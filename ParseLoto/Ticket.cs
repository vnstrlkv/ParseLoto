using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketns
{
    class Ticket
    {
     public   int numberticket { get; set; }
        public int amount { get; set; }
     public   int[] number = new int[100];
        public int this[int index]
        {
            get
            {
                return number[index];
            }
            set
            {
                number[index] = value;
            }
        }
     public void Write ()
        {
            Console.WriteLine("Ticket number - {0}, amount - {1}", numberticket,amount);
            foreach(int i in number)
            {
                if(number[i]!=0)
                Console.WriteLine("i={0} num={1}", i, number[i]);
            }

        }
    }
}
