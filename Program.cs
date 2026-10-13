using System.Drawing;
using JuegoSnake;
using Juegosnake;

Ventana ventana;
Snake snake;
Comida comida;
bool jugar = false;
bool ejecucion = true;

void Iniciar()
{
    ventana = new Ventana("Snake", 65, 20, ConsoleColor.Black, ConsoleColor.White,
        new Point(5, 3), new Point(59, 18));
    ventana.DibujarMarco();
    comida = new Comida(new Point(10, 10), ConsoleColor.Green, ventana);
    snake = new Snake(new Point(8, 4), ConsoleColor.Red, ConsoleColor.White, ventana, comida);
}

void Game()
{
    while (ejecucion)
    {
        // Menú (la serpiente magenta se mueve sola dentro de Menu)
        ventana.Menu();
        ventana.Teclado(ref ejecucion, ref jugar, snake);
        Thread.Sleep(100);

        // Juego
        while (jugar)
        {
            snake.Mover();
            if (!snake.Vivo)
            {
                jugar = false;   // Gameover ya limpió la pantalla, volvemos al menú
            }
            Thread.Sleep(100);
        }
    }
}

Iniciar();
Game();
Console.ResetColor();
Console.Clear();