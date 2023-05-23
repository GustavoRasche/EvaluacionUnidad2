using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medidores.DAL
{
    public class LecturaDAL
    {
        private string filePath;

        public LecturaDAL(string filePath)
        {
            this.filePath = filePath;
        }

        public void GuardarLectura(Lectura lectura)
        {
            string lecturaString = $"{lectura.MedidorId}|{lectura.Fecha.ToString("yyyy-MM-dd HH:mm:ss")}|{lectura.ValorConsumo}";

            try
            {
                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine(lecturaString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar la lectura: " + ex.Message);
            }
        }

        public List<Lectura> ObtenerLecturas()
        {
            List<Lectura> lecturas = new List<Lectura>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] lecturaData = line.Split('|');

                        if (lecturaData.Length == 3)
                        {
                            int medidorId;
                            if (int.TryParse(lecturaData[0], out medidorId))
                            {
                                DateTime fecha;
                                if (DateTime.TryParseExact(lecturaData[1], "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out fecha))
                                {
                                    decimal valorConsumo;
                                    if (decimal.TryParse(lecturaData[2], out valorConsumo))
                                    {
                                        Lectura lectura = new Lectura
                                        {
                                            MedidorId = medidorId,
                                            Fecha = fecha,
                                            ValorConsumo = valorConsumo
                                        };
                                        lecturas.Add(lectura);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener las lecturas: " + ex.Message);
            }

            return lecturas;
        }
    }
}
