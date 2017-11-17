using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Entities
{
    //A User Defined type (UDT) to represent a real world object.
    public class Patient
    { 
      /*  public string MRN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set;}
        public string Address { get; set; }
        */

        
          private string _MRN;
              public string MRN
              {
                  get { return _MRN; }
                  set { _MRN = value; }
              }

              private string _FirstName;

              public string FirstName
              {
                  get { return _FirstName; }
                  set { _FirstName = value; }
              }

              private string _LastName;

              public string LastName
              {
                  get { return _LastName; }
                  set { _LastName = value; }
              }

              private string _Address;        

              public string Address
              {
                  get { return _Address; }
                  set { _Address = value; }
              }

        /*  private PV1 _Location;

          public PV1 Location
          {
              get { return _Location; }
              set { _Location = value; }
          }
          */
        //  public PV1 location;
        public PV1 Location
        {
            get;set;
        }

        public Patient()   //constructor to initalize location
        {
            Location = new PV1();
        }
      
    }


    public class PV1
    {

        public string Facility { get; set; }
        public string Careunit { get; set; }

        public string RoomBed { get; set; }
       /* private string _Facility;

        public string Facility
        {
           get { return _Facility; }
            set { _Facility = value; }
        }

        private string _CareUnit;

        public string CareUnit
        {
            get { return _CareUnit; }
            set { _CareUnit = value; }
        }

        private string _RoomBed;

        public string RoomBed
        {
            get { return _RoomBed; }
            set { _RoomBed = value; }
        }*/


    }
}

namespace Repository    //it provide database related operations like add, search,update...
{
    using Entities;
    

    public interface IHL7Messages
    {
        /// <summary>
        /// create patients demographics  
        /// </summary>
        /// 
        void AddPatientDetails(Patient p);

              
        /// <summary>
        ///Get patient detail based on MRN. 
        /// </summary>
        /// <param name="mrn">MRN for which details to be fetched</param>
        /// <returns></returns>
        Patient GetPatientDetails(string mrn);

        /// <summary>
        /// Returns the list of all avaible patient
        /// </summary>
        /// <returns></returns>
        List<Patient> GetAllPatient();
        
        /// <summary>
        /// Generate sample ADT HL7 message 
        /// </summary>
        /// <param name="mrn">patient ID for which hl7 message to be generated</param>
        string GenerateHL7ADTMessage(string mrn, string triggerEvent);
    }

    public interface IHL7PtMessages:IHL7Messages
    {
        /// <summary>
        /// To generate patient data for an existing patient 
        /// </summary>
        /// <param name="mrn"></param>
        /// <returns></returns>
        string GenerateHL7PtMessage(string mrn);
        
        /// <summary>
        /// Update patient location 
        /// </summary>
        void UpdateLocation(String pat, PV1 location);
    }

   public class Messages : IHL7PtMessages
    {
        private static List<Patient> _allPatients= new List<Patient>();
        private static List<PV1> _alllocation = new List<PV1>();
        //private static string test = "ahri";
        private string filecontent="";
        public int[] keyEnd= new int[50];  //to keep track off end of Segments;
        

        public virtual void AddPatientDetails(Patient p)
        {
            //throw new NotImplementedException();
            if (p == null)
                throw new Exception("Patient details are not set");
            _allPatients.Add(p);
            _alllocation.Add(p.Location);
        }    //add record to list

        public virtual string GenerateHL7ADTMessage(string mrn, string triggerEvent)     //fetch sample message from root directory and update required field
        {
            Dictionary<int, string> fields = new Dictionary<int, string>();  //eachfields
            Dictionary<int, string> segments = new Dictionary<int, string>(); //each segment
            Console.WriteLine("\nADT {0} for MRN::{1}",triggerEvent, mrn);
            FilereaderHL7();   //read sample ADT file
            try
            {
            
            //if (filecontent != null)
            //    Console.WriteLine(filecontent);
                           
                
                fields = MakeField();  //separates fileds
                segments = Makesegment();

                /*        foreach(var item in segments)
                                 Console.WriteLine("Key=>{0}, Value=>{1}\n", item.Key, item.Value);    //print dict items
                         foreach (var item in fields)
                             Console.WriteLine("Key=>{0}, Value=>{1}\n", item.Key,item.Value);    //print dict items
                             */

                //          Console.ReadLine();


                if (mrn!="")
                {
                    //update value in dictionary
                    //MSH-6
                    fields.Remove(6);
                    fields.Add(6, DateTime.Now.ToString("yyyyMMddHHmmss") + "|");
                                        
                    //EVN-2  (23)
                    fields.Remove(keyEnd[0] + 2);
                    fields.Add((keyEnd[0] + 2), DateTime.Now.ToString("yyyyMMddHHmmss") + "|");

                    
                    //pv1-44 (AdmitDateTime)
                    fields.Remove(keyEnd[2] + 44);
                    fields.Add((keyEnd[2] + 44), DateTime.Now.ToString("yyyyMMddHHmmss") + "|");

                    //PID (30)
                    fields.Remove(keyEnd[1] + 3);
                    fields.Add(keyEnd[1] + 3, mrn + "^^^MRENTR^MR|");

                    //Name(32)
                    fields.Remove(keyEnd[1] + 5);
                    fields.Add(keyEnd[1] + 5, _allPatients.Find((p => p.MRN == mrn)).FirstName + "^" + _allPatients.Find((p => p.MRN == mrn)).LastName + "|");

                    // Address(38)
                    fields.Remove(keyEnd[1] + 11);
                    fields.Add(keyEnd[1] + 11, _allPatients.Find((p => p.MRN == mrn)).Address + "|");

                    //Location(61)
                    fields.Remove(keyEnd[2] + 3);
                    //fields.Add(61, _allPatients.Find((p => p.MRN == mrn)).Location.Careunit + "^" + (_allPatients.IndexOf(_allPatients.Find((p=>p.MRN==mrn)))+1) + "^" + (_allPatients.IndexOf(_allPatients.Find((p => p.MRN == mrn))) + 1) + "^" + _allPatients.Find((p => p.MRN == mrn)).Location.Facility);

                    fields.Add(keyEnd[2] + 3, (_allPatients.Find((p => p.MRN == mrn)).Location.Careunit) + "^" + (_allPatients.Find(p => p.MRN == mrn).Location.RoomBed) + "^" + (_allPatients.Find(p => p.MRN == mrn).Location.RoomBed) + "^" + (_allPatients.Find(p => p.MRN == mrn).Location.Facility + "|"));

                    //Trigger Event set; 8,22  (in MSH and EVN)
                    fields[8] = "ADT^" + triggerEvent + "^ADT_" + triggerEvent + "|";
                    fields[keyEnd[0] + 1] = triggerEvent + "|";

                    //MSH Control ID update
                    fields[9]="PLW21220171109202727 - " + DateTime.Now.ToString("ss")+"|";

                      //  PLW21230569996527992 - 54
                     //   PLW21220171109202727

                }
                                
                string Output ="";
                for (int v = 0; v < keyEnd.Length; v++)
                {
                    keyEnd[v] = keyEnd[v] - 1;     //21 , 27, 58,103  reduced -1 
                }
                int no_of_segments = 0;
                foreach(var count in keyEnd)
                {
                    if(count!=-1)
                        no_of_segments++;
                }

              //  Console.WriteLine("lambda:: " + keyEnd.F;//       Select(x => x == -1).Count());
                Console.WriteLine("No of segments: " + no_of_segments);


                //combining fields to form a message
                foreach (var item in fields)
                {

                    //if(item.Key==20 ||item.Key==26||item.Key==57) 
                   
                   
                    if (keyEnd.Contains(item.Key))
                    {
                        Output = Output + item.Value + "\r";
                        if ((triggerEvent!="A01") && (Array.IndexOf(keyEnd,item.Key)==3))   //hardcoded 
                            {
                            break;
                            //fields.Select(x=>x.Value=="MSH")
                        }

                    }
                    else
                    {
                        Output = Output + item.Value;
                    }
                    
                }
                Console.WriteLine("\nADT Message is :" + Output);
                return Output;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw new Exception("The file is empty");
                
            }
        }

        public Dictionary<int,string> MakeField()
        {

            string[] line = filecontent.Split(new[] { "\\r", "\\r\n", "\n" }, StringSplitOptions.None); //split each segments then each fields
            Dictionary<int, string> field = new Dictionary<int, string>();
            int i = 0;
            int j = 0;
            //int[] keyEnd;  //to keep track off end of Segments;
            do
            {

                foreach (string item in line[i].Split('|'))
                {
                    field.Add(j, item+"|");     //| at end of field
                    j++;
                    
                }
                keyEnd[i] = j;
                i++;
                
            } while (i < line.Count()); 

            return field;

        }

        public Dictionary<int,string> Makesegment()
        {
            string[] line = filecontent.Split(new[] { "\\r", "\\r\n", "\n" }, StringSplitOptions.None); //segments
            Dictionary<int, string> seg = new Dictionary<int, string>();
            int i = 0;
            foreach (var item in line)
            {
                if (item!=null) { 
                seg.Add(i, item);
                i++;
                }
            }

            return seg;
        }

        
        public void FilereaderHL7()
        {
            //read from root directory

            // string filecontent = "";
            //string path = "C:/Users/Public/Documents/testMessages/ADTA01.txt";
            string path = "..\\testMessages\\test.txt"; // ADTA01.txt";
            filecontent = "";
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;//= sr.ReadLine();

                    while ((line = sr.ReadLine()) != null)
                    {
                        filecontent = filecontent + line;    
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file cannot be read at path {0}", path + e.Message);
            }

            //return filecontent;
        }


        public Patient GetPatientDetails(string mrn)
        {
            //throw new NotImplementedException();

           // try
            //{
                foreach (var pat in _allPatients)
                {
                    if (pat.MRN == mrn)
                    {
                        return pat;
                    }

                }
            //}
            //catch (Exception e)
            //{
              //  Console.WriteLine(e.StackTrace);
                //return 1;
                throw new Exception("\nPatient not found");
            
            //}
                        
        }    //return patient details from List

        public List<Patient> GetAllPatient()
        {

            // if (_allPatients != null)
                   //Console.WriteLine(_allPatients.First());
            return _allPatients;
            


           
        }
        public string GenerateHL7PtMessage(string mrn)   //for ptData
        {
            throw new NotImplementedException();
        }

        
        public void UpdateLocation(string pat, PV1 location)
        {
           
            _allPatients.Where(x => x.MRN == pat).First().Location = location;  //UpdateLocation of patient
            
            //if new bed add add it to list
            var val = _alllocation.Find(item => item.RoomBed == location.RoomBed);
            if (val == null)
                _alllocation.Add(location);
                        
        }
    }

}


namespace myPatient
{
    using Repository;
    using Entities;
  
    public class TCPcommn
    {
        static IHL7Messages mess = new Messages();
        static Patient p = new Patient();

       //public static  void sendADT(string triggerEvent)
       // {

       //     List<Patient> mypatList;

       //     //Read list and generate ADT for that patient.
       //     mypatList = mess.GetAllPatient();

       //     if (triggerEvent == "A01")
       //     {
       //         foreach (var pat in mypatList)
       //         {
       //             TCPCliennt(mess.GenerateHL7ADTMessage(pat.MRN, triggerEvent), "127.0.0.1", 10002);
       //         }
       //     }

       //     // for each patient send ADT
            
            
       // }
        
        //server 
        public static void TCPServerr()
        {
            try
            {
                string strServerip = "127.0.0.1"; //  Console.ReadLine();
                IPAddress ip = IPAddress.Parse(strServerip);

                //Initialize listeners
                TcpListener tcpListener = new TcpListener(ip, 10002);

                //start listening at specified port
                tcpListener.Start();

                Console.WriteLine("The server is running at port {0} ...", "10002");
                Console.WriteLine("The local End point is  :" +
                          tcpListener.LocalEndpoint);
                Console.WriteLine("Waiting for a connection...");

                //waiting for connection
                Socket s = tcpListener.AcceptSocket();

                Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);


            }
            catch (Exception e)
            {

                throw new Exception(e.StackTrace);
            }

        }

        public static void TCPCliennt(string message, string strHostname, int port)
        {

            try
            {
                TcpClient tc = new TcpClient();
                Console.WriteLine("Connecting ...");
                string strclientIP = "127.0.0.1"; // strHostname   // Console.ReadLine();
                tc.Connect(strclientIP, port);
                
                Console.WriteLine("Connected");
            //    Console.Write("The string transmitting:\n {0} \n", message);

                string str = message; //"mymessage";
                //str = "hariswaroop";
               // Console.WriteLine();
                Stream stream = tc.GetStream();
                
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                Console.WriteLine("\nTransmitting.....");

                stream.Write(ba, 0, ba.Length);
                //stream.Flush();
                
                //to read ACk from Server
            /*      byte[] bb = new byte[100];
                  int k = stream.Read(bb, 0, 100);

                 for (int i = 0; i < k; i++)
                   Console.Write(Convert.ToChar(bb[i]));
                   */
               // tc.Close();
                return;
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nUnable to connect server\n"+ex.Message+"\n"+ ex.StackTrace);
                //Console.ReadKey();
                //return;
                //throw;
            }
            
        }   //not working


        public void HariTCPclient(string message, string strHostname, int port)       //working TCP client
        {
            try
            {

           
            
            TcpClient tcpClient=new TcpClient();
            tcpClient.Connect(strHostname, port);
            Console.WriteLine("Connected");

                NetworkStream ns = tcpClient.GetStream();
                StreamWriter sr = new StreamWriter(ns);

                //add header (\x0B) and trailer(\x1C\x0D)
                message = "\x0B" + message + "\x1C\x0D";
                char[] b = message.SelectMany(x => x.ToString()).ToArray();   //converting message in char array
                                                                              

                //ASCIIEncoding asen = new ASCIIEncoding();
                //byte[] ba = asen.GetBytes(b);
                sr.AutoFlush = true;
                sr.Write(b);    //passed char array as stream
                sr.Flush();
                sr.Close();
                
                                
               /* using (StreamWriter swriter = new StreamWriter(ns))
                {
                    swriter.Write(message);
                    swriter.Flush();
                }*/
                
               // tcpClient.Close();

            }
            catch (Exception e)
            {

                //throw new SocketException();
                Console.WriteLine("exception::" + e.StackTrace);
            }

        }

    }
}

/*
 * 
 *         TcpClient tcpClient = new TcpClient("localhost", port);

        //Connects fine
        NetworkStream ns = tcpClient.GetStream();
        StreamWriter sw = new StreamWriter(ns);

        //The code moves on but nothing seems to be sent unless I do
        //a sw.Close() after this line. That would however close the  
        //ns and prevent me from reading the response further down
        sw.Write("hello");

        //I am using a stream reader with ReadToEnd() on the tcpListener
        //which never receives the string from this piece of code

        //Since the above never actually send I get stuck here
        string response = new StreamReader(ns).ReadToEnd();

        sw.Close();
        tcpClient.Close();
 * */
