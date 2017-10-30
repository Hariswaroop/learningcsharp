using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myPatient;
using Entities;
using Repository;
namespace TestClient
{
    class Test
    {
        static void Main(string[] args)
        {

            Repository.Messages msg = new Repository.Messages();
            TCPcommn tc = new TCPcommn();
            int choice;
            //PV1 loc = new PV1();
            int suffix=1;
            //myMenu
            do
            {
                Console.WriteLine("\n\n/************my Menu***********/\n1.Add Patient\n2.Send ADT A01\n3.Send ADT A03\n4.Send ADt A02\n5.Exit");
                Console.WriteLine("Select your choice");
                //Console.WriteLine("Select from Menu Enter 1 for Add Patient\n2 for Send ADT A01\n3 for send ADT A03\n4 Exit");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        {
                            Entities.Patient pat = new Patient();   // for new instance of every patient
                            Entities.PV1 loc = new PV1();

                            /*Console.WriteLine("Enter your Patient details MRN:");
                                 pat.MRN=Console.ReadLine();
                                   pat.FirstName = Console.ReadLine();
                                   pat.Lastname = Console.ReadLine();
                                   pat.Address = Console.ReadLine();
                                   loc.Facility = Console.ReadLine();
                                   pat.Location.Facility = loc.Facility;
                                   loc.Careunit = Console.ReadLine();
                                   pat.Location.Careunit = loc.Careunit;
                                   loc.RoomBed = Console.ReadLine();
                                   pat.Location.RoomBed = loc.RoomBed;
                                   */
                            //save patient 
                            //Console.WriteLine("enter mrn:");
                            //pat.MRN= Console.ReadLine();
                            pat.MRN = "MRN" + suffix;
                            pat.FirstName = "Firstname";
                            pat.LastName = "Lastname";
                            //Address:: street address, City,State,Zip
                            pat.Address = "ABCD^^BLR^KA^560045";
                            
                            //pV1.3
                            loc.Facility = "FACILITY1";
                            pat.Location.Facility = loc.Facility;
                            loc.Careunit = "UNIT1";
                            pat.Location.Careunit = loc.Careunit;
                            loc.RoomBed = "1";
                            pat.Location.RoomBed = loc.RoomBed;

                            msg.AddPatientDetails(pat);
                            suffix++;
                            Console.WriteLine("The patient successfully added");
                             break;
                            
                        } 
                    case 2:   
                        {
                            // msg.GenerateHL7ADTMessage(pat.MRN);  //generate ADT A01.  

                            //Console.ReadKey();

                            //  TCPcommn tCPcommn = new TCPcommn();
                            // tCPcommn.TCPCliennt("abcd", "127.0.0.1", 10002);   //AdT port

                            //      TCPcommn.sendADT("A01");  //sendADT for top patient

                /*            foreach (var p in msg.GetAllPatient())
                            {
                                Console.WriteLine(p.MRN + "\n");
                            }
                            Console.WriteLine("Enter the pat MRN for Admit");
                            string val = "MRN1";// Console.ReadLine();
                            TCPcommn.TCPCliennt(msg.GenerateHL7ADTMessage(val, "A01"), "127.0.0.1", 10002);  //generate A03 for  entered mrn

    */
                            testTcpClient.testTCPcclient();
                            break;
                            
                        }
                    case 3:
                        {
                            //get list of patient and create discharge for entered mrn
                            
                            foreach(var p in msg.GetAllPatient())
                            {
                                Console.WriteLine(p.MRN+"\n");
                            }
                            Console.WriteLine("Enter the pat MRN for discharge");  
                            string val= Console.ReadLine();
                            
                            TCPcommn.TCPCliennt(msg.GenerateHL7ADTMessage(val, "A03"),"127.0.0.1",10002);  //generate A03 for  entered mrn
                        //   foreach(var item in msg.GetAllPatient().SelectMany(p=>p.MRN==val,pat))  //.Find(p=>p.MRN=="abcd"))
                        //    {
                        //        
                        //        TCPcommn.TCPCliennt(msg.GenerateHL7ADTMessage(item.MRN, "A03"), "127.0.0.1", 10002);
                        //
                        //    }
                            //TCPcommn.sendADT("A03");   //send A03
                            break;
                            
                        }

                    case 4:
                        {
                            //transfer
                            break;
                        }
                   
                }

            }while(choice!=5);
            
        }
    }
}
