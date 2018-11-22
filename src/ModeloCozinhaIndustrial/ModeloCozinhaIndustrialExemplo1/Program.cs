using System;
using System.Collections.Generic;

namespace ModeloCozinhaIndustrial
{
    class Program
    {
        public class Ingrediente
        {
            public string Nome;
            public decimal Quantidade;
        }

        public class ItemCardapio { }

        public class Ovo : Ingrediente { }
        public class Manteiga : Ingrediente { }
        public class Leite : Ingrediente { }
        public class Sal : Ingrediente { }

        public class OvoMexido : ItemCardapio { }

        public class Cozinha
        {
            public OvoMexido PrepararOvoMexido(List<Ingrediente> ingredientes)
            {
                Console.WriteLine("Preparando ovo mexido" );
                throw new NotImplementedException();
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
