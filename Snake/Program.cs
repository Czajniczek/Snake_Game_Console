using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(85, 35);
            Console.SetBufferSize(85, 35);
            Console.CursorVisible = false;
            Console.Title = "SNAKE";
            
            Menu menu = new Menu();
            
            menu.Navigation();
        }
    }
}