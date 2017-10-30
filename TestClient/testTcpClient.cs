using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace TestClient
{
    public class testTcpClient
    {
        private static void Main()
        {
        }

        public static void  testTCPcclient() { 
            using (TcpClient client = new TcpClient())

            {
                Console.WriteLine("Attempting to connect to the server ", "on port 10003.");

                try
                {
                    client.Connect(IPAddress.Parse("127.0.0.1"), 10003);

                    using (NetworkStream networkStream = client.GetStream())
                    {
                        /*  using (BinaryWriter writer = new BinaryWriter(networkStream))
                          {
                              writer.Write("MSH|^~\\&|Carecast|MMC|Philips|MMC|20171030213304||ADT^A01^ADT_A01|PLW21230569996527992-1|P|2.4|||AL|NE|||||ADT|\nEVN | A01 | 20171030213304 || OPACC | 13RCASTILL ^ CASTILLO ^ RICHARD |\nPID || 001323429 | MRN1 ^^^ MRENTR ^ MR || Firstname ^ Lastname || 19631201 | M || 4 | ABCD ^^ BLR ^ KA ^ 560045 | 58 | 1111111111 || EN | M | CA | ACC123 | SSN123 | DL123 |||||||||| Y |\nPV1 || E | UNIT1 ^ 1 ^ 1 ^ FACILITY1 ||||| 1832 ^ DocFN ^ DocGN || A04 |||| 7 ||||| VN123 | HA |||||||||| 00000000 ||||| 00000000 |||| EINST || 1 ||| 20171030213304 | ");

                              using (BinaryReader reader = new BinaryReader(networkStream))
                              {
                                  Console.WriteLine(reader.ReadString());
                              }
                          }*/

                        using (StreamWriter sr = new StreamWriter(networkStream))
                        {
                            sr.WriteLine("MSH|^~\\&|Carecast|MMC|Philips|MMC|20171030213304||ADT^A01^ADT_A01|PLW21230569996527992-1|P|2.4|||AL|NE|||||ADT|\nEVN | A01 | 20171030213304 || OPACC | 13RCASTILL ^ CASTILLO ^ RICHARD |\nPID || 001323429 | MRN1 ^^^ MRENTR ^ MR || Firstname ^ Lastname || 19631201 | M || 4 | ABCD ^^ BLR ^ KA ^ 560045 | 58 | 1111111111 || EN | M | CA | ACC123 | SSN123 | DL123 |||||||||| Y |\nPV1 || E | UNIT1 ^ 1 ^ 1 ^ FACILITY1 ||||| 1832 ^ DocFN ^ DocGN || A04 |||| 7 ||||| VN123 | HA |||||||||| 00000000 ||||| 00000000 |||| EINST || 1 ||| 20171030213304 | ");

                            using (BinaryReader reader = new BinaryReader(networkStream))
                            {
                                Console.WriteLine(reader.ReadString());
                            }
                        }
                    }
                }
                catch(SocketException e)
                {
                     Console.WriteLine(e.StackTrace);
                }
            }
        }
    }
}

          