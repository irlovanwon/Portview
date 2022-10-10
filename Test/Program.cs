///Copyright(c) 2013,HIT All rights reserved.
///Summary：Test
///Author：Irlovan
///Date：2013-04-03
///Description：This is a Test
///Modification：


using Irlovan.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using Irlovan.Register;
using System.IO.Ports;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Timers;
using System.Text;

namespace Irlovan
{
    class Program
    {
        static void Main(string[] args) {
            XElement origon = XElement.Load(@"C:\Users\IrlovanWon\Desktop\vur\origin.txt");
            Dictionary<string, XElement> configList = new Dictionary<string, XElement>();
            foreach (var item in origon.Elements("Item")) {
                string name = item.Attribute("Name").Value;
                if (!configList.ContainsKey(name)) {
                    configList.Add(name, item);
                }
            }
            XElement result = new XElement("Hoist");
            int index = 1;
            foreach (var item in configList) {
                XElement child = new XElement("Data");
                child.SetAttributeValue("Name",item.Key);
                child.SetAttributeValue("Title", item.Key);
                child.SetAttributeValue("ID", index);
                result.Add(child);
                index++;
            }
            System.IO.File.AppendAllText(@"C:\Users\IrlovanWon\Desktop\vur\origin.txt", result.ToString());
            Console.ReadLine();
        }

        private static void OriginXML() {
            XElement xml = XElement.Load(new StringReader(File.ReadAllText("d:/DriverConverter/origin.txt", Encoding.UTF8)));
            Dictionary<string, XElement> resultItems = new Dictionary<string, XElement>();
            IEnumerable<XElement> items = xml.Elements("Group");
            IEnumerable<XElement> targets = Targets();
            foreach (var item in items) {
                foreach (var dataItem in item.Elements("Data")) {
                    XElement resultDataXML = FindData(targets, dataItem.Attribute("RealtimeDataName").Value);
                    if (resultDataXML == null) { resultDataXML = FindData(targets, dataItem.Attribute("Addr").Value); }
                    if (resultDataXML == null) { dataItem.SetAttributeValue("Apple", "****************************************************************"); }
                    else { dataItem.SetAttributeValue("Addr", resultDataXML.Attribute("Addr").Value); }
                }
            }
            File.WriteAllText("d:/DriverConverter/result.txt", xml.ToString());
        }

        private static IEnumerable<XElement> Targets() {
            List<XElement> result = new List<XElement>();
            XElement xml = XElement.Load(new StringReader(File.ReadAllText("d:/DriverConverter/target.txt", Encoding.UTF8)));
            foreach (var item in xml.Elements("Data")) {
                result.Add(item);
            }
            return result;
        }

        private static XElement FindData(IEnumerable<XElement> targets, string name) {
            foreach (var item in targets) {
                if (name == item.Attribute("RealtimeDataName").Value) { return item; }
            }
            return null;
        }

        private delegate void SendHandler();

        public static void createListener(String server, String message, int port) {
            // Create an instance of the TcpListener class.
            TcpListener tcpListener = null;
            IPAddress ipAddress = Dns.GetHostEntry(server).AddressList[0];
            try {
                // Set the listener on the local IP address 
                // and specify the port.
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
            }
            catch (Exception e) {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            //TcpClient tcpClient = tcpListener.AcceptTcpClient();
            Socket socket = tcpListener.AcceptSocket();

            Byte[] init = new Byte[256];
            SocketFlags flag = SocketFlags.None;
            EndPoint endPoint = socket.RemoteEndPoint;
            IPPacketInformation info;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            socket.Send(msg, 0, msg.Length, 0);
            //socket.Receive(init, init.Length, 0);
            socket.ReceiveFrom(init, ref endPoint);
            socket.Send(msg);
            //socket.Receive(init, init.Length);
            socket.ReceiveFrom(init, ref endPoint);


            //NetworkStream stream = tcpClient.GetStream();


            //SendHandler send = new SendHandler(() => {
            //    System.Timers.Timer timer;
            //    Helper.Helper.SetInterval(3000, (object o, ElapsedEventArgs e) => {
            //        stream.Write(msg, 0, msg.Length);
            //    }, out timer);
            //});
            //send.BeginInvoke(null,null);

            //Int32 a = stream.Read(init, 0, init.Length);
            //string initData = System.Text.Encoding.ASCII.GetString(init, 0, a);
            //tcpListener.Server.Send(msg, 0, msg.Length, SocketFlags.None);

            //while (true) {
            //    System.Threading.Thread.Sleep(100);
            //    stream.ReadByte();
            //    //Byte[] result = new Byte[256];
            //    //stream.Read(result, 0, result.Length);
            //}
        }

        static void Connect(String server, String message, int port) {
            try {
                // Create a TcpClient. 
                // Note, for this client to work you need to have a TcpServer  
                // connected to the same address as specified by the server, port 
                // combination.
                //Int32 port = 10011;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing. 
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);


                // Receive the TcpServer.response. 

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e) {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e) {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }


        private static void CreateRegister() {
            Register.Register R = new Register.Register(@"c:/opc/", "RAM");
            R.RAM = new Singularity("T9", R);
            ISingularity Alarm = new Singularity("Alarm", R);
            IEther<string> m01Alarm = new Ether<string>("TT628Alarm", R);
            Alarm.Container.Push(m01Alarm);
            R.RAM.Container.Push(Alarm);
            R.RecordtoHD();
        }

        public static void SQLHanlderTest() {

        }

        public static void Expression() {
            //Catalog source = Irlovan.Database.Config.ReadXML(XDocument.Load(@"E:\WebArchitecture\Main\bin\Debug\Project\Core\RealtimeData"));
            //Irlovan.Expression.Expression expression = new Irlovan.Expression.Expression("(3+{RealtimeData.Analog.BOOM_CURRENT})/2");
            //IIndustryData<double> data = (IIndustryData<double>)source.AcquireData("RealtimeData.Analog.BOOM_CURRENT");
            //data.DataChange += (object sender, DataChangeEventArgs e) => {
            //    dynamic result = expression.Eval(new dynamic[] { data.Value });
            //};
            //data.ReadValue(55.6);
        }

        public static void OPCTest() {
        }



        public static void DotNetTest() {
            //string a = "hello";
            //string[] b = a.Split('.');
            //RealtimeDataTest();
            Catalog a = new Catalog("a");
            Catalog b = new Catalog("b");
            Catalog c = new Catalog("c");
            List<Catalog> list = new List<Catalog>();
            list.Add(a);
            list.Add(b);
            list.Add(c);
            List<Catalog> result = new List<Catalog>();
            result.Add(list[0]);
            //result[0].FullName = "fuck";


            //test b = new test(a);

        }
        private class test
        {
            //public test(Catalog a) { a.FullName = "Irlovan"; }
        }
    }

    [Serializable]
    public class HelloW : ISerializable
    {
        public HelloW() {

        }

        public string a = "sssssssssssssssssasfdwefwefewfefwef";
        public int b = 100;
        HelloV c = new HelloV();
        // Implement this method to serialize data. The method is called  
        // on serialization. 
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            // Use the AddValue method to specify serialized values.
            info.AddValue("props", a, typeof(string));
            info.AddValue("xx", b, typeof(int));
            //info.AddValue("yy", c, typeof(HelloV));

        }

        //sThe special constructor is used to deserialize values. 
        public HelloW(SerializationInfo info, StreamingContext context) {
            foreach (var item in info) {
                string m = (string)item.Name;
            }
            // Reset the property value using the GetValue method.
            a = (string)info.GetValue("props", typeof(string));
            b = (int)info.GetValue("xx", typeof(int));
            //c = (HelloV)info.GetValue("yy", typeof(HelloV));

        }
    }

    [Serializable]
    public class HelloV : ISerializable
    {
        public HelloV() {

        }

        public string a = "mmm";
        public int b = 200;
        // Implement this method to serialize data. The method is called  
        // on serialization. 
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            // Use the AddValue method to specify serialized values.
            info.AddValue("ff", a, typeof(string));
            info.AddValue("gg", b, typeof(int));

        }

        //sThe special constructor is used to deserialize values. 
        public HelloV(SerializationInfo info, StreamingContext context, int mmss) {
            // Reset the property value using the GetValue method.
            a = (string)info.GetValue("ff", typeof(string));
            b = (int)info.GetValue("gg", typeof(int));
        }
    }


    public class Test1
    {
        public Test1() {
            Hello();
        }
        internal virtual void Hello() {
            a = "dd";

        }

        public string a = "mmm";
        public int b = 200;

    }

    public class Test2 : Test1
    {
        public Test2()
            : base() {
        }
        internal override void Hello() {
            //base.Hello();
            b = 200;
        }

        public string a = "mmm";
        public int b = 200;

    }
}
