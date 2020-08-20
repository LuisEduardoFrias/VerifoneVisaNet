

namespace varifoneVisaNet
{
    using System;
    using System.IO.Ports;

    class Program
    {
        private static string _Port;
        private static int _Speed, _DataBit;
        private static Parity _Parity;
        private static StopBits _StropBits;
        private static Handshake _Handshake;

        static void Main(string[] args)
        {
            Console.WriteLine("introduce monto.");
            string Monto;

            _Port = System.Configuration.ConfigurationManager.AppSettings.Get("Port");

            _Speed = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Speed"));

            _Parity = (Parity)Enum.Parse(typeof(Parity),
                System.Configuration.ConfigurationManager.AppSettings.Get("Parity"),
                true);

            _DataBit = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("DataBit"));

            _StropBits = (StopBits)Enum.Parse(typeof(StopBits),
                System.Configuration.ConfigurationManager.AppSettings.Get("StropBits"),
                true);

            _Handshake = (Handshake)Enum.Parse(typeof(Handshake),
                System.Configuration.ConfigurationManager.AppSettings.Get("Handshake"),
                true);

            Monto = System.Console.ReadLine();

            ConectionVisaNet comunicacionPort = ConectionVisaNet.GetIntances("COM3",9600, Parity.None,8, StopBits.One);

            string trama = "\u0002CN00\u001c" + Monto + "00\u001c000000000160\u001c000000000040\u001c000001\u001c02\u0003\u0016";

                             comunicacionPort.OpenComunication(_Handshake);
            var respuesta =  comunicacionPort.SendDataAsync(trama); //,new System.TimeSpan(9,32,25)

            Console.WriteLine("\n");
            Console.WriteLine(respuesta);
            Console.ReadLine();
        }

    }
}



/*
            string Monto;

            Monto = Console.ReadLine();

           SerialPort port = ComunicacionPuertoSerie("COM3",9600,Parity.None,8,StopBits.One);
           Console.WriteLine( "Coneccion estableciada" );

           string trama = "\u0002CN00\u001c"+Monto+"00\u001c000000000160\u001c000000000040\u001c000001\u001c02\u0003\u0016";

            EnviarPuertoSerie(port,trama);
            Console.WriteLine("Trama enviada: " + trama);


            Console.ReadLine();

            var repuesta = LeerPuerto(port);

            if(repuesta.Count != 0)
            {
                foreach(var rep in repuesta)
                {
                    Console.WriteLine("Data: " + rep);
                }
            }
            else Console.WriteLine("Sin repuesta: " + repuesta);


            port.Close();

            Console.ReadLine();
        }

        private static SerialPort ComunicacionPuertoSerie(string puerto,int velocidad,Parity paridad,int dato,StopBits stopBits)
        {
            try
            {

                SerialPort port = new SerialPort(puerto,velocidad,paridad,dato,stopBits);
                port.Close();

                port.Handshake = Handshake.None;

                port.RtsEnable = false;
                port.DtrEnable = true;

                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                port.ErrorReceived += new SerialErrorReceivedEventHandler(port_ErrorReceived);

                port.ReadTimeout = 500;
                port.WriteTimeout = 500;

                port.Open();

                return port;
            }
            catch(ArgumentOutOfRangeException Ex)
            {
                Console.WriteLine("AbrirPuertoSerie. " + Ex.Message);
            }
            catch(InvalidOperationException Ex)
            {
                Console.WriteLine("AbrirPuertoSerie. " + Ex.Message);
            }
            catch(ArgumentException Ex)
            {
                Console.WriteLine("AbrirPuertoSerie. " + Ex.Message);
            }
            catch(IOException Ex)
            {
                Console.WriteLine("AbrirPuertoSerie. " + Ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("AbrirPuertoSerie. " + ex.Message);
            }

            return null;
        }

        private static void port_DataReceived(object sender,SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("Se recivieron datos." + sender.ToString());
        }

        private static void port_ErrorReceived(object sender,SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("Hubo un error en el puerto" + sender.ToString());
        }

        private static void EnviarPuertoSerie(SerialPort port,string trama)
        {
            try
            {
                if((port != null))
                {
                    port.Write(trama);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("EnviarPuertoSerie. " + ex.Message);
            }
        }


        private static List<byte> LeerPuerto(SerialPort port)
        {
            List<byte> Respuesta = new List<byte>();

            try
            {
                while(port.BytesToRead > 0)
                {
                    Respuesta.Add((byte)port.ReadByte());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("LeerPuerto. " + ex.Message);
            }

            return Respuesta;
        }

        #region async
        //public List EnviarPuertoSerieAsync(byte[] Datos)
        //{
        //    List read = new List();
        //    sRespuesta Respuesta = new sRespuesta();
        //    bool TramaOK = false;

        //    try
        //    {
        //        EnviarPuertoSerie(Datos);
        //        read = LeerPuerto();

        //        //Comprobamos que la trama tenga la longitud
        //        //Necesaria, si no es así volvemos a leer
        //        DateTime startToWait = DateTime.Now; //start timeout
        //        bool isTimeout = false;

        //        while(!TramaOK && !isTimeout)
        //        {
        //            List Lectura = new List();
        //            //Volvemos a leer y añadimos lo leido a la lista read
        //            Lectura = LeerPuerto();
        //            if(Lectura.Count > 0)
        //                read.AddRange(Lectura);

        //            if(read.Count > 0)
        //            {

        //                //Detectamos el final de la trama
        //                if(read.Count > 0)
        //                {
        //                    int BuscaETX = read.FindLastIndex(x => x == (byte)enComandosDecimal.ETX);
        //                    if(BuscaETX > 0)
        //                    {
        //                        if((read.Count == BuscaETX + 5)
        //                            && (BuscaETX > 0)
        //                            && (read[BuscaETX - 1] != (byte)enComandosDecimal.ESC) //Comprobamos que no venga precedido por un caracter de escape
        //                            && (read[BuscaETX - 2] != (byte)enComandosDecimal.STX)) //Comprobamos que no sea la respuesta rápida
        //                            TramaOK = true;
        //                    }
        //                }
        //            }
        //            isTimeout = (DateTime.Now.Subtract(_timeout) >= startToWait);
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        _Errores.add("EnviarPuertoSerieSinc. " + ex.Message);
        //    }
        //    return read;
        //}
        #endregion*/
