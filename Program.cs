using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class ServidorSocket
{
    static void Main()
    {
        // Configuración del servidor
        string serverIP = "127.0.0.1";  // IP del servidor
        int serverPort = 12345;  // Puerto del servidor

        // Crear un socket TCP/IP
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            // Asociar el socket a la dirección IP y al puerto
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(serverIP), serverPort));

            // Escuchar conexiones entrantes
            serverSocket.Listen(1);
            Console.WriteLine("Esperando conexiones entrantes...");

            while (true)
            {
                // Esperar a que un cliente se conecte
                Socket clientSocket = serverSocket.Accept();
                Console.WriteLine("Cliente conectado: " + clientSocket.RemoteEndPoint);

                // Enviar mensaje de bienvenida al cliente
                string mensajeBienvenida = "Hola, bienvenido a Chat Konecta. Para nosotros es un placer atenderte. ¿En que podemos ayudarte?";
                byte[] mensajeBytes = Encoding.ASCII.GetBytes(mensajeBienvenida);
                clientSocket.Send(mensajeBytes);
                Console.WriteLine(mensajeBienvenida);



                while (true)
                {
                    // Recibir la pregunta del cliente
                    byte[] preguntaBytes = new byte[1024];
                    int bytesRecibidos = clientSocket.Receive(preguntaBytes);
                    string pregunta = Encoding.ASCII.GetString(preguntaBytes, 0, bytesRecibidos);
                    Console.WriteLine("Pregunta del cliente: " + pregunta);

                    // Procesar la pregunta y obtener la respuesta
                    string respuesta = ProcesarPregunta(pregunta);
                    Console.WriteLine("Respuesta al cliente: " + respuesta);

                    // Enviar respuesta al cliente
                    byte[] respuestaBytes = Encoding.ASCII.GetBytes(respuesta);
                    clientSocket.Send(respuestaBytes);

                    // Verificar si se debe terminar la conexión con el cliente
                    if (pregunta.ToLower() == "adios")
                        break;
                }

                // Cerrar la conexión con el cliente
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            // Cerrar el socket del servidor
            serverSocket.Close();
        }
    }
    static string ProcesarPregunta(string pregunta)
    {
        string respuesta = "";

        if (pregunta.Contains("certificado") && pregunta.Contains("bancario"))
        {
            respuesta = "Para obtener un certificado bancario, debes seguir estos pasos:\n" +
                "1. Ingresar a la URL de Bancolombia https://www.bancolombia.com/personas\n" +
                "2. Ingresar a la opcion 'Solicita Documentos'.\n" +
                "3. Seleccionar la opción de 'Certificado Bancario'.\n" +
                "4. Llenar el formulario.\n" +
                "5. Esperar el tiempo necesario para que el banco emita el certificado.\n" +
                "Recuerda que esto puede tardar unas horas y se te notificara por correo electronico cuando el certificado este disponible para descargar.";
        }
        else if (pregunta.Contains("como") && pregunta.Contains("estas"))
        {
            respuesta = "Excelentemente y espero que te encuentres super bien.";
        }
        else if (pregunta.Contains("necesito") && pregunta.Contains("ayuda"))
        {
            respuesta = "Si, por favor, dame mas detalles para atender tu solicitud.";
        }
        else if (pregunta.ToLower() == "adios")
        {
            respuesta = "Hasta luego. ¡Que tengas un buen dia!";
        }
        else if (pregunta.Contains("te") && pregunta.Contains("amo"))
        {
            respuesta = "No comprendo el sentimiento humano pero aprecio haberte generado esa emocion en ti :D";
        }
        else
        {
            respuesta = "Lo siento, no puedo responder a esa pregunta en este momento.";
        }

        return respuesta;
    }
}