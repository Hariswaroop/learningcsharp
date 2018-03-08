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
                            loc.RoomBed = suffix.ToString();   //increment bed order
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

                              TCPcommn tCPcommn = new TCPcommn();
                            Console.WriteLine("Patient MRN List\n");
                            foreach (var p in msg.GetAllPatient())
                            {
                                Console.WriteLine(p.MRN + "\n");
                            }
                            Console.WriteLine("Enter the pat MRN for Admit");
                            string val = Console.ReadLine();
                            if(msg.GetPatientDetails(val)!=null)
                                tCPcommn.HariTCPclient(msg.GenerateHL7ADTMessage(val, "A01"), "127.0.0.1", 10002);
                            break;
                            
                        }
                    case 3:
                        {
                            //get list of patient and create discharge for entered mrn
                            TCPcommn tCPcommn = new TCPcommn();
                            Console.WriteLine("Patient MRN List\n");
                            foreach(var p in msg.GetAllPatient())
                            {
                                Console.WriteLine(p.MRN+"\n");
                            }
                            Console.WriteLine("Enter the pat MRN for discharge");  
                            string val= Console.ReadLine();
                            if (msg.GetPatientDetails(val) != null)
                                    tCPcommn.HariTCPclient(msg.GenerateHL7ADTMessage(val, "A03"),"127.0.0.1",10002);  //generate A03 for  entered mrn
                            break;
                            
                        }

                    case 4:
                        {
                            //transfer
                            
                            TCPcommn tCPcommn = new TCPcommn();
                           
                            Console.WriteLine("Patient MRN List\n");
                            foreach (var p in msg.GetAllPatient())
                            {
                                Console.WriteLine(p.MRN + "\n");
                            }
                            Console.WriteLine("Enter the pat MRN for transfer");
                            string mrn = Console.ReadLine();

                            Console.WriteLine("Enter the pat new location\nEnter Facility\n");
                            string fac = Console.ReadLine();
                            Console.WriteLine("Enter CareUnit\n");
                            string cunit = Console.ReadLine();
                            Console.WriteLine("Enter RoomBed\n");
                            string rb = Console.ReadLine();
                            Console.WriteLine("RoomBed: {0}",msg.GetPatientDetails(mrn).Location.RoomBed);
                            Entities.PV1 loc1 = new PV1();
                            loc1.Facility = fac;
                            loc1.Careunit = cunit;
                            loc1.RoomBed = rb;
                            msg.UpdateLocation(mrn, loc1);
                            if (msg.GetPatientDetails(mrn) != null)
                                tCPcommn.HariTCPclient(msg.GenerateHL7ADTMessage(mrn, "A02"), "127.0.0.1", 10002);  
                            break;
                        }
                   
                }

            }while(choice!=5);
            
        }
    }
}
