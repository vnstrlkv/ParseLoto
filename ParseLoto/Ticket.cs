using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketns
{
    [Serializable]
    public class Ticket
    {
     public   int numberticket { get; set; }
        public int amount { get; set; }
     public   int[] number = new int[91];
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

    [Serializable]
public  class CountTicket
    {
        public int numberOfTickets;
        private List<Ticket> listTickets = new List<Ticket>();
        public int[] field = new int[91];
        public int[] number = new int[91];
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
        public void AddList(Ticket tic)
        {
            listTickets.Add(tic);
        }
        public bool ExistsTicket(Ticket tic)
        {
            if (listTickets.Exists((x) => x == tic))
                return true;
            return false;
        }

        public  CountTicket()
        {
   
        }
        
        public void NullAll()
        {
            numberOfTickets = 0;
            listTickets = new List<Ticket>();
            for (int i = 0; i < 91; i++)
            {
                field[i]=i;
                number[i] = 0;
            }
        }
        public void Write()
        {
            Console.WriteLine("Ticket - {0}", numberOfTickets);
            for (int i=0;i<91;i++)
            {
                if (i % 10 == 0)
                    Console.WriteLine();
                    Console.Write("{0} - {1} || ", i, number[i]);
            }

        }

        public void CountUp()
        {
            foreach (Ticket tic in listTickets)
            {
                numberOfTickets++;

                for (int i = 1; i < 91; i++)
                    number[i] += tic.number[i];
            }
        }
    }
}
