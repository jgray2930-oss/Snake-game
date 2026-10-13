using System.Drawing;
using JuegoSnake;

namespace Juegosnake
{
    internal class Comida
    {
        public Point Posicion { get; set; }
        public ConsoleColor Color { get; set; }
        public Ventana VentanaC { get; set; }

        public Comida(Point posicion, ConsoleColor color, Ventana ventana)
        {
            Posicion = posicion;
            Color = color;
            VentanaC = ventana;
        }

        public void Dibujar()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(Posicion.X, Posicion.Y);
            Console.Write("$");
        }
        public bool GenerarComida(Snake snake)
        {
            int longSnake = snake.Cuerpo.Count + 1;
            if(( VentanaC.area - longSnake) <= 0)
            {
                return false;
            }
            Random random = new Random();
            int x = random.Next(VentanaC.LimiteSuperior.X+1,VentanaC.LimiteInferior.X);
            int y = random.Next(VentanaC.LimiteSuperior.Y+1, VentanaC.LimiteInferior.Y);
            Posicion = new Point (x,y);

            foreach(Point item in snake.Cuerpo)
            if(( x==item.X&&y==item.Y)||
            (x == snake.Cabeza.X && y == snake.Cabeza.Y))
                {
                    if( GenerarComida(snake))
                    return true;
                }
            Dibujar();
            return true;
        }

        internal void GenerarComida()
        {
            throw new NotImplementedException();
        }
    }
}