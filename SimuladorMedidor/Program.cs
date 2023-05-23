using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorMedidor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenido al Simulador de Medidor Eléctrico");
            Console.WriteLine("---------------------------------------------");

            Console.WriteLine("Ingrese la dirección IP del servidor de comunicaciones:");
            string ipAddress = Console.ReadLine();

            Console.WriteLine("Ingrese el puerto del servidor de comunicaciones:");
            int port;
            if (!int.TryParse(Console.ReadLine(), out port))
            {
                Console.WriteLine("Puerto inválido. El programa se cerrará.");
                return;
            }

            Console.WriteLine("Ingrese el número de medidor:");
            int medidorId;
            if (!int.TryParse(Console.ReadLine(), out medidorId))
            {
                Console.WriteLine("Número de medidor inválido. El programa se cerrará.");
                return;
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("--- Ingresar Nueva Lectura ---");
                Console.WriteLine("Ingrese la fecha y hora en el formato yyyy-MM-dd HH:mm:ss:");
                string fechaHoraString = Console.ReadLine();
                DateTime fechaHora;
                if (!DateTime.TryParseExact(fechaHoraString, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out fechaHora))
                {
                    Console.WriteLine("Fecha y hora inválidas. Inténtelo nuevamente.");
                    continue;
                }

                Console.WriteLine("Ingrese el valor de consumo:");
                decimal valorConsumo;
                if (!decimal.TryParse(Console.ReadLine(), out valorConsumo))
                {
                    Console.WriteLine("Valor de consumo inválido. Inténtelo nuevamente.");
                    continue;
                }

                // Crear el mensaje de lectura en el formato nroMedidor|fecha|valorConsumo
                string lectura = $"{medidorId}|{fechaHora.ToString("yyyy-MM-dd HH:mm:ss")}|{valorConsumo}";

                // Enviar la lectura al servidor de comunicaciones
                EnviarLectura(ipAddress, port, lectura);

                Console.WriteLine("Lectura enviada correctamente.");

                Console.WriteLine("¿Desea ingresar otra lectura? (S/N)");
                string respuesta = Console.ReadLine().ToLower();
                if (respuesta != "s")
                    break;
            }

            Console.WriteLine("Gracias por utilizar el Simulador de Medidor Eléctrico. El programa se cerrará.");
        }

        static void EnviarLectura(string ipAddress, int port, string lectura)
        {
            try
            {
                using (TcpClient client = new TcpClient(ipAddress, port))
                using (StreamWriter writer = new StreamWriter(client.GetStream()))
                {
                    writer.WriteLine(lectura);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar la lectura: " + ex.Message);
            }
        }
    }
}
