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
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    namespace ServicioComunicaciones
    {
        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("Servicio de Comunicaciones - Escuchando peticiones...");

                // Configuración del servidor
                int port = 3000;
                IPAddress ipAddress = IPAddress.Any;

                TcpListener listener = new TcpListener(ipAddress, port);
                listener.Start();

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Cliente conectado");

                    // Procesar la lectura recibida
                    ProcesarLectura(client);

                    client.Close();
                    Console.WriteLine("Cliente desconectado");

                    // Mostrar el menú después de procesar una lectura
                    MostrarMenu();
                    string opcion = Console.ReadLine();
                    if (opcion == "1")
                    {
                        MostrarLecturasGuardadas();
                    }
                    else if (opcion == "2")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Opción inválida. Inténtelo nuevamente.");
                    }
                }

                Console.WriteLine("El programa se cerrará. Gracias por utilizar el Servicio de Comunicaciones.");
            }

            static void ProcesarLectura(TcpClient client)
            {
                try
                {
                    StreamReader reader = new StreamReader(client.GetStream());
                    string lectura = reader.ReadLine();
                    Console.WriteLine("Lectura recibida: " + lectura);

                    // Almacenar la lectura en un archivo
                    AlmacenarLectura(lectura);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al procesar la lectura: " + ex.Message);
                }
            }

            static void AlmacenarLectura(string lectura)
            {
                try
                {
                    string filePath = "lecturas.txt";
                    using (StreamWriter writer = File.AppendText(filePath))
                    {
                        writer.WriteLine(lectura);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al almacenar la lectura: " + ex.Message);
                }
            }

            static void MostrarLecturasGuardadas()
            {
                try
                {
                    string filePath = "lecturas.txt";
                    if (File.Exists(filePath))
                    {
                        string[] lecturas = File.ReadAllLines(filePath);
                        Console.WriteLine("--- Lecturas Guardadas ---");
                        foreach (string lectura in lecturas)
                        {
                            Console.WriteLine(lectura);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se encontraron lecturas guardadas.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al mostrar las lecturas guardadas: " + ex.Message);
                }
            }

            static void MostrarMenu()
            {
                Console.WriteLine();
                Console.WriteLine("===== Menú =====");
                Console.WriteLine("1. Mostrar lecturas guardadas");
                Console.WriteLine("2. Salir");
                Console.WriteLine("=================");
                Console.Write("Ingrese la opción deseada: ");
            }
        }
    }

}
