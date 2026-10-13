using System.Drawing;
using System.IO.Pipes;

namespace JuegoSnake
{
    internal class Ventana
    {
        public string Titulo { get; set; }
        public int Ancho { get; set; }
        public int Altura { get; set; }
        public ConsoleColor ColorFondo { get; set; }
        public ConsoleColor ColorLetra { get; set; }
        public Point LimiteSuperior { get; set; }
        public Point LimiteInferior { get; set; }
        public int area { get; set; }
        public Snake SnakeC { get; set; }

        public Ventana(string titulo, int ancho, int altura, ConsoleColor colorFondo, ConsoleColor colorLetra,
                       Point limiteSuperior, Point limiteInferior)
        {
            Titulo = titulo;
            Ancho = ancho;
            Altura = altura;
            ColorFondo = colorFondo;
            ColorLetra = colorLetra;
            LimiteSuperior = limiteSuperior;
            LimiteInferior = limiteInferior;
            area = ((LimiteInferior.X - LimiteSuperior.X) - 1) * ((LimiteInferior.Y - LimiteSuperior.Y) - 1);
            Init();
        }

        public void Init()
        {
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(Ancho, Altura);
            }
            Console.Title = Titulo;
            Console.CursorVisible = false;
            Console.BackgroundColor = ColorFondo;
            Console.ForegroundColor = ColorLetra;
            Console.Clear();
            SnakeC = new Snake(new Point(LimiteInferior.X / 2, LimiteInferior.Y - 3),
                ConsoleColor.Magenta, ConsoleColor.White, this, null);
            SnakeC.IniciarCuerpo(4);
        }

        public void DibujarMarco()
        {
            // Líneas de arriba y de abajo
            for (int x = LimiteSuperior.X; x <= LimiteInferior.X; x++)
            {
                Console.SetCursorPosition(x, LimiteSuperior.Y);
                Console.Write("═");
                Console.SetCursorPosition(x, LimiteInferior.Y);
                Console.Write("═");
            }

            // Líneas de la izquierda y de la derecha
            for (int y = LimiteSuperior.Y; y <= LimiteInferior.Y; y++)
            {
                Console.SetCursorPosition(LimiteSuperior.X, y);
                Console.Write("║");
                Console.SetCursorPosition(LimiteInferior.X, y);
                Console.Write("║");
            }

            // Esquinas
            Console.SetCursorPosition(LimiteSuperior.X, LimiteSuperior.Y);
            Console.Write("╔");
            Console.SetCursorPosition(LimiteInferior.X, LimiteSuperior.Y);
            Console.Write("╗");
            Console.SetCursorPosition(LimiteSuperior.X, LimiteInferior.Y);
            Console.Write("╚");
            Console.SetCursorPosition(LimiteInferior.X, LimiteInferior.Y);
            Console.Write("╝");
        }

        public void Menu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(LimiteSuperior.X + (LimiteInferior.X / 2) - 12,
                LimiteSuperior.Y + (LimiteInferior.Y / 2) - 4);
            Console.Write("J U E G O   S N A K E");
            Console.SetCursorPosition(LimiteSuperior.X + (LimiteInferior.X / 2) - 8,
                LimiteSuperior.Y + (LimiteInferior.Y / 2) - 2);
            Console.Write("Enter - JUGAR");
            Console.SetCursorPosition(LimiteSuperior.X + (LimiteInferior.X / 2) - 8,
                LimiteSuperior.Y + (LimiteInferior.Y / 2) - 1);
            Console.Write("Esc - SALIR");

            SnakeC.MoverMenu();
        }

        public void Teclado(ref bool ejecucion, ref bool jugar, Snake snake)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    jugar = true;
                    Console.Clear();
                    DibujarMarco();
                    snake.init();
                }
                if (tecla.Key == ConsoleKey.Escape)
                {
                    ejecucion = false;
                }
            }
        }

        public void Gameover(string text)
        {
            Console.Clear();
            DibujarMarco();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(LimiteSuperior.X + (LimiteInferior.X / 2) - 10,
                LimiteSuperior.Y + (LimiteInferior.Y / 2) - 2);
            Console.Write(text);
            Thread.Sleep(3000);
            Console.Clear();
            DibujarMarco();
        }
    }
}



 
    



