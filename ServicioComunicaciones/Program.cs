using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Medidores.DAL;
using Medidores;

namespace ServicioComunicaciones
{
    class Program
    {
        static void Main(string[] args)
        {
            int puerto = 3000;
            string filePath = "lecturas.txt";
            LecturaDAL lecturaDAL = new LecturaDAL(filePath);

            TcpListener listener = new TcpListener(IPAddress.Any, puerto);
            listener.Start();
            Console.WriteLine("Servicio de comunicaciones iniciado. Escuchando en el puerto: " + puerto);

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Cliente conectado: " + client.Client.RemoteEndPoint);

                StreamReader reader = new StreamReader(client.GetStream());

                // Mostrar el menú
                Console.WriteLine("--- Menú ---");
                Console.WriteLine("1. Ingresar Lecturas");
                Console.WriteLine("2. Mostrar lecturas");
                Console.WriteLine("3. Salir");
                Console.WriteLine("Ingrese el número de la opción deseada:");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        IngresarLectura(lecturaDAL, client);
                        break;

                    case "2":
                        MostrarLecturas(lecturaDAL);
                        break;

                    case "3":
                        Console.WriteLine("Saliendo del programa...");
                        listener.Stop();
                        return;

                    default:
                        Console.WriteLine("Opción inválida. Por favor, ingrese un número válido.");
                        break;
                }

                client.Close();
            }
        }

        static void MostrarLecturas(LecturaDAL lecturaDAL)
        {
            Console.WriteLine("--- Lecturas registradas ---");
            var lecturas = lecturaDAL.ObtenerLecturas();

            foreach (var lectura in lecturas)
            {
                Console.WriteLine($"ID Medidor: {lectura.MedidorId}, Fecha: {lectura.Fecha}, Valor Consumo: {lectura.ValorConsumo}");
            }
        }

        static void IngresarLectura(LecturaDAL lecturaDAL, TcpClient client)
        {
            // Solicitar información al usuario
            Console.WriteLine("Ingrese el ID del medidor:");
            int medidorId;
            if (!int.TryParse(Console.ReadLine(), out medidorId))
            {
                Console.WriteLine("ID de medidor inválido. Se omitirá la lectura.");
                return;
            }

            Console.WriteLine("Ingrese la fecha y hora en el formato yyyy-MM-dd hh:mm:ss :");
            string fechaHoraString = Console.ReadLine();
            DateTime fechaHora;
            if (!DateTime.TryParseExact(fechaHoraString, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out fechaHora))
            {
                Console.WriteLine("Fecha y hora inválidas. Se omitirá la lectura.");
                return;
            }

            Console.WriteLine("Ingrese el valor de consumo:");
            decimal valorConsumo;
            if (!decimal.TryParse(Console.ReadLine(), out valorConsumo))
            {
                Console.WriteLine("Valor de consumo inválido. Se omitirá la lectura.");
                return;
            }

            // Crear la lectura y guardarla
            Lectura lectura = new Lectura
            {
                MedidorId = medidorId,
                Fecha = fechaHora,
                ValorConsumo = valorConsumo
            };

            lecturaDAL.GuardarLectura(lectura);
            Console.WriteLine("Lectura guardada correctamente.");
        }
    }
}
