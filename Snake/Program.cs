using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Snake";
            Vector2D pantalla;
            //pantalla.x = 51;
            //pantalla.y = 21;
            pantalla.x = 100;
            pantalla.y = 30;
            Console.SetWindowSize(pantalla.x + 2, pantalla.y + 10);
            Random rand = new Random();
            
            string op = "";
            do
            {
                Console.CursorVisible = false;
                Console.Clear();
                Muros muros = new Muros(pantalla);

                Snake snake = new Snake();
                snake.colorCabeza = ConsoleColor.Cyan;
                snake.colorCuerpo = ConsoleColor.DarkBlue;
                snake.posicion.x = rand.Next(1, 30);
                snake.posicion.y = rand.Next(1, 10);

                Comida comida = new Comida();
                comida.GenerarPosicion(snake);

                Juego juego = new Juego(pantalla, muros, snake, comida);
                juego.Iniciar();
                Console.WriteLine("Quieres volver a jugar? <S> o <N>");
                Console.Write("-> ");
                Console.CursorVisible = true;
                op = Console.ReadLine().ToUpper();
            } while (op == "S");
            Console.WriteLine();
        }
    }
    public class Juego
    {
        private Vector2D pantalla;
        private Snake snake;
        private Comida comida;
        private Muros muros;
        private int puntuacion = 0;
        private bool jugando;
        private int fps = 120;
        public Juego(Vector2D tamaño, Snake snake, Comida comida)
        {
            this.pantalla = tamaño;
            this.snake = snake;
            this.comida = comida;
            muros = new Muros(pantalla);
        }
        public Juego(Vector2D tamaño, Muros muros,Snake snake, Comida comida)
        {
            this.pantalla = tamaño;
            this.snake = snake;
            this.comida = comida;
            this.muros = muros;
        }
        public void Iniciar()
        {
            muros.Dibujar();
            MostrarPuntuacion();
            CuentaAtras();
            snake.CrearCabeza();
            comida.Dibujar();
            jugando = true;
            while (jugando) {
                DetectarPulsaciones();
                if (!VerificarCollision())
                {
                    snake.Borrar();
                    snake.Mover();
                    snake.Dibujar();
                }
                else
                {
                    jugando = false;
                   
                }
                Thread.Sleep(fps);
            }
            Console.ResetColor();
            string txtPerdiste = "Has perdido!";
            Console.SetCursorPosition(((pantalla.x + 13) / 2) - txtPerdiste.Length, pantalla.y + 3);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(txtPerdiste);
            Console.ResetColor();
            Console.SetCursorPosition(0, pantalla.y + 5);
        }
        private void DetectarPulsaciones()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey teclaPresionada = Console.ReadKey().Key;
                switch (teclaPresionada)
                {
                    case ConsoleKey.RightArrow:
                        snake.direccion.x = 1;
                        snake.direccion.y = 0;
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.direccion.x = -1;
                        snake.direccion.y = 0;
                        break;
                    case ConsoleKey.UpArrow:
                        snake.direccion.x = 0;
                        snake.direccion.y = -1;
                        break;
                    case ConsoleKey.DownArrow:
                        snake.direccion.x = 0;
                        snake.direccion.y = 1;
                        break;
                    case ConsoleKey.Escape:
                        jugando = false;
                        Console.SetCursorPosition(0,pantalla.y + 4);
                        break;
                }
            }
        }
        private bool VerificarCollision()
        {
            Vector2D posicionFutura = new Vector2D();
            posicionFutura.x = snake.posicion.x + snake.direccion.x;
            posicionFutura.y = snake.posicion.y + snake.direccion.y;

            if (muros.listaDeMuros.Contains(posicionFutura))
            {
                Console.Beep(330, 1000);
                return true;
            }
            else if ((snake.posicion.x == comida.posicion.x) &&
                (snake.posicion.y == comida.posicion.y))
            {
                snake.Crecer();
                if (puntuacion == 0) snake.Crecer();
                Console.Beep();
                puntuacion++;
                MostrarPuntuacion();
                comida.GenerarPosicion(snake);
                comida.Dibujar();
            }
            else if (snake.cuerpo.Contains(posicionFutura))
            {
                Console.Beep(330, 1000);
                return true;
            }
            return false;
        }
        private void MostrarPuntuacion()
        {
            Console.ResetColor();
            string txtPuntuacion = $"Puntuacion: {puntuacion}";
            Console.SetCursorPosition(((pantalla.x + 15) / 2) - txtPuntuacion.Length, pantalla.y + 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(txtPuntuacion);
            Console.ResetColor();
        }

        private void CuentaAtras()
        {
            int x = (pantalla.x + 1) / 2;
            int y = (pantalla.y + 1) / 2;
            Console.Beep(523, 200);
            Console.SetCursorPosition(x - 3, y - 2);
            Console.Write("###");
            Console.SetCursorPosition(x - 3, y - 1);
            Console.Write("  #");
            Console.SetCursorPosition(x - 3, y);
            Console.Write("###");
            Console.SetCursorPosition(x - 3, y + 1);
            Console.Write("  #");
            Console.SetCursorPosition(x - 3, y + 2);
            Console.Write("###");
            Thread.Sleep(1000);
            Console.Beep(523, 200);
            Console.SetCursorPosition(x - 3, y - 2);
            Console.Write("###");
            Console.SetCursorPosition(x - 3, y - 1);
            Console.Write("  #");
            Console.SetCursorPosition(x - 3, y);
            Console.Write("###");
            Console.SetCursorPosition(x - 3, y + 1);
            Console.Write("#  ");
            Console.SetCursorPosition(x - 3, y + 2);
            Console.Write("###");
            Thread.Sleep(1000);
            Console.Beep(523, 200);
            Console.SetCursorPosition(x - 3, y - 2);
            Console.Write(" # ");
            Console.SetCursorPosition(x - 3, y - 1);
            Console.Write(" # ");
            Console.SetCursorPosition(x - 3, y);
            Console.Write(" # ");
            Console.SetCursorPosition(x - 3, y + 1);
            Console.Write(" # ");
            Console.SetCursorPosition(x - 3, y + 2);
            Console.Write(" # ");
            Thread.Sleep(1000);
            Console.SetCursorPosition(x - 3, y - 2);
            Console.Write("   ");
            Console.SetCursorPosition(x - 3, y - 1);
            Console.Write("   ");
            Console.SetCursorPosition(x - 3, y);
            Console.Write("   ");
            Console.SetCursorPosition(x - 3, y + 1);
            Console.Write("   ");
            Console.SetCursorPosition(x - 3, y + 2);
            Console.Write("   ");
            Console.Beep(880, 500);
        }
    }
    public class Muros
    {
        private Vector2D dimension;
        public char sprite = '#';
        public ConsoleColor color = ConsoleColor.DarkGreen;
        public List<Vector2D> listaDeMuros = new List<Vector2D>();
        private Vector2D pos;
        public Muros(Vector2D dimension)
        {
            this.dimension = dimension;
        }
        public void Dibujar()
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < dimension.x; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(sprite);
                pos.x = i;
                pos.y = 0;
                listaDeMuros.Add(pos);
                Console.SetCursorPosition(i, dimension.y - 1);
                Console.Write(sprite);
                pos.x = i;
                pos.y = dimension.y - 1;
                listaDeMuros.Add(pos);
                if (i < dimension.y)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write(sprite);
                    pos.x = 0;
                    pos.y = i;
                    listaDeMuros.Add(pos);
                    Console.SetCursorPosition(dimension.x -1, i);
                    Console.Write(sprite);
                    pos.x = dimension.x - 1;
                    pos.y = i;
                    listaDeMuros.Add(pos);
                }
            }
            Console.ResetColor();
        }
    }
    public class Snake
    {
        public char sprite = '0';
        public Vector2D direccion;
        public Vector2D posicion;
        public List<Vector2D> cuerpo = new List<Vector2D>();
        private Vector2D cabeza;
        public ConsoleColor colorCabeza = ConsoleColor.Yellow;
        public ConsoleColor colorCuerpo = ConsoleColor.DarkCyan;
        public Snake()
        {
            direccion.x = 1;
            direccion.y = 0;
        }
        public void Dibujar()
        {
            Console.ResetColor();
            Console.ForegroundColor = colorCuerpo;

            for (int i = cuerpo.Count - 1; i >= 0; i--)
            {
                if (cuerpo[i].x == cabeza.x && cuerpo[i].y == cabeza.y)
                {
                    Console.ForegroundColor = colorCabeza;
                }
                Console.SetCursorPosition(cuerpo[i].x, cuerpo[i].y);
                Console.Write(sprite);
                Console.ForegroundColor = colorCuerpo;
            }
        }
        public void Borrar()
        {
            for (int i = cuerpo.Count -1; i >= 0; i--)
            {
                Console.SetCursorPosition(cuerpo[i].x, cuerpo[i].y);
                Console.Write(' ');
            }
        }
        public void Mover()
        {
            posicion.x += direccion.x;
            posicion.y += direccion.y;
            cabeza = posicion;
            cuerpo[0] = cabeza;

            for (int i = cuerpo.Count - 1; i > 0; i--)
            {
                cuerpo[i] = cuerpo[i - 1];
            }
            
        }
        public void CrearCabeza()
        {
            cuerpo.Add(posicion);
            cabeza = posicion;
        }
        public void Crecer()
        {
            cuerpo.Add(posicion);
        }
    }
    public class Comida
    {
        public char sprite = '*';
        public ConsoleColor color = ConsoleColor.Red;
        public Vector2D posicion = new Vector2D();
        public void GenerarPosicion(Snake snake)
        {
            Random rand = new Random();
            posicion.x = rand.Next(1, 40);
            posicion.y = rand.Next(1, 10);
            if (snake.cuerpo.Contains(posicion)) GenerarPosicion(snake);
        }
        public void Dibujar()
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(posicion.x, posicion.y);
            Console.Write(sprite);
            Console.ResetColor();
        }
    }
    public struct Vector2D
    {
        public int x;
        public int y;
    }
}