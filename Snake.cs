using System.Drawing;
using Juegosnake;

namespace JuegoSnake
{
    enum Direccion { Arriba, Abajo, Izquierda, Derecha }

    internal class Snake
    {
        public bool Vivo { get; set; }
        public ConsoleColor ColorCabeza { get; set; }
        public ConsoleColor ColorCuerpo { get; set; }
        public Ventana VentanaC { get; set; }
        public List<Point> Cuerpo { get; set; }
        public Point Cabeza { get; set; }
        public Comida ComidaC { get; set; }

        public int Puntaje { get; set; }
        public int PuntajeMaximo { get; set; }
        public Point PosicionInicial { get; set; }
        private Direccion _direccion = Direccion.Derecha;
        private bool _comiendo;

        public Snake(Point posicion, ConsoleColor colorCabeza, ConsoleColor colorCuerpo, Ventana ventana, Comida comida)
        {
            ColorCabeza = colorCabeza;
            ColorCuerpo = colorCuerpo;
            VentanaC = ventana;
            Cabeza = posicion;
            ComidaC = comida;
            Cuerpo = new List<Point>();
            Puntaje = 0;
            PuntajeMaximo = 0;
            PosicionInicial = posicion;
            _direccion = Direccion.Derecha;
        }

        public void init()
        {
            Cuerpo.Clear();
            Cabeza = PosicionInicial;
            IniciarCuerpo(2);
            Vivo = true;
            Puntaje = 0;
            _direccion = Direccion.Derecha;
            ComidaC.GenerarComida(this);
            Informacion(0, 0);
        }

        public void IniciarCuerpo(int numPartes)
        {
            int x = Cabeza.X - 1;
            for (int i = 0; i < numPartes; i++)
            {
                Console.SetCursorPosition(x, Cabeza.Y);
                Console.Write("█");
                Cuerpo.Add(new Point(x, Cabeza.Y));
                x--;
            }
        }

        public void Mover()
        {
            Teclado();
            Point posCabezaAnterior = Cabeza;
            MoverCabeza();
            MoverCuerpo(posCabezaAnterior);
            ColisionesComida();
            if (ColisionesCuerpo())
            {
                Muerte();
                VentanaC.Gameover("TU NO SILVE");
            }
        }

        private bool ColisionesCuerpo()
        {
            foreach (Point item in Cuerpo)
            {
                if (Cabeza == item)
                {
                    Vivo = false;
                    return true;
                }
            }
            return false;
        }

        public void MoverCabeza()
        {
            Console.ForegroundColor = ColorCabeza;
            Console.SetCursorPosition(Cabeza.X, Cabeza.Y);
            Console.Write(" ");

            switch (_direccion)
            {
                case Direccion.Derecha:
                    Cabeza = new Point(Cabeza.X + 1, Cabeza.Y);
                    break;
                case Direccion.Izquierda:
                    Cabeza = new Point(Cabeza.X - 1, Cabeza.Y);
                    break;
                case Direccion.Arriba:
                    Cabeza = new Point(Cabeza.X, Cabeza.Y - 1);
                    break;
                case Direccion.Abajo:
                    Cabeza = new Point(Cabeza.X, Cabeza.Y + 1);
                    break;
            }
            ColisionesMarco();
            Console.SetCursorPosition(Cabeza.X, Cabeza.Y);
            Console.Write("█");
        }

        private void MoverCuerpo(Point posCabezaAnterior)
        {
            Console.ForegroundColor = ColorCuerpo;
            Console.SetCursorPosition(posCabezaAnterior.X, posCabezaAnterior.Y);
            Console.Write("█");
            Cuerpo.Insert(0, posCabezaAnterior);

            if (_comiendo)
            {
                _comiendo = false;
                return; // no borra la cola, así la serpiente crece
            }

            Console.SetCursorPosition(Cuerpo[Cuerpo.Count - 1].X, Cuerpo[Cuerpo.Count - 1].Y);
            Console.Write(" ");
            Cuerpo.RemoveAt(Cuerpo.Count - 1);
        }

        private void ColisionesComida()
        {
            if (Cabeza == ComidaC.Posicion)
            {
                Puntaje++;
                if (Puntaje > PuntajeMaximo)
                    PuntajeMaximo = Puntaje;

                Informacion(0, 0); // actualiza el texto en pantalla

                if (!ComidaC.GenerarComida(this))
                {
                    Vivo = false;
                    VentanaC.Gameover("TU ERES LA PARA");
                }
                _comiendo = true;
            }
        }

        private void Teclado()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.RightArrow && _direccion != Direccion.Izquierda)
                    _direccion = Direccion.Derecha;
                if (tecla.Key == ConsoleKey.LeftArrow && _direccion != Direccion.Derecha)
                    _direccion = Direccion.Izquierda;
                if (tecla.Key == ConsoleKey.UpArrow && _direccion != Direccion.Abajo)
                    _direccion = Direccion.Arriba;
                if (tecla.Key == ConsoleKey.DownArrow && _direccion != Direccion.Arriba)
                    _direccion = Direccion.Abajo;
            }
        }

        private void ColisionesMarco()
        {
            if (Cabeza.X <= VentanaC.LimiteSuperior.X)
                Cabeza = new Point(VentanaC.LimiteInferior.X - 1, Cabeza.Y);
            if (Cabeza.X >= VentanaC.LimiteInferior.X)
                Cabeza = new Point(VentanaC.LimiteSuperior.X + 1, Cabeza.Y);
            if (Cabeza.Y <= VentanaC.LimiteSuperior.Y)
                Cabeza = new Point(Cabeza.X, VentanaC.LimiteInferior.Y - 1);
            if (Cabeza.Y >= VentanaC.LimiteInferior.Y)
                Cabeza = new Point(Cabeza.X, VentanaC.LimiteSuperior.Y + 1);
        }

        public void Muerte()
        {
            Console.ForegroundColor = ColorCuerpo;
            foreach (Point item in Cuerpo)
            {
                if (item == Cabeza)
                    continue;
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write("░");
                Thread.Sleep(120);
            }
        }

        public void Informacion(int distanciaX1, int distanciaX2)
        {
            string texto = $"Puntaje: {Puntaje}     Puntaje Máximo: {PuntajeMaximo}";

            int centroMarco = (VentanaC.LimiteSuperior.X + VentanaC.LimiteInferior.X) / 2;
            int x = centroMarco - texto.Length / 2;
            int y = VentanaC.LimiteSuperior.Y - 1;

            // Limpiar la fila para que no queden restos del texto anterior
            Console.SetCursorPosition(VentanaC.LimiteSuperior.X, y);
            Console.Write(new string(' ', VentanaC.LimiteInferior.X - VentanaC.LimiteSuperior.X + 1));

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(x, y);
            Console.Write(texto);
        }

        public void MoverMenu()
        {
            _direccion = Direccion.Derecha;
            Point posCabezaAnterior = Cabeza;
            MoverCabeza();
            MoverCuerpo(posCabezaAnterior);
        }
    }
}