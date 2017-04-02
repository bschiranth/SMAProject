//////////////////////////////////////////////////////////////////
// TestExec.cs - CommService server                               //
// ver 1.0                                                     //
// Application:  Demonstration for CSE681 SMA , Project#4       //
// Language:         C#, ver 6.0, Visual Studio 2015            //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10         //
// Author : Chiranth Bangalore Sathyaprakash                    //
// SUID: 258380492  , csathyap@syr.edu, 315-751-1129            //
// Original Author:Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com           //
///////////////////////////////////////////////////////////////////
/*
 * This is the main package where testing is done
/*
/* Public Interface - 
* -------------------------
* launch_WPF() - starts WPF
* launch_Server() - starts server
* launch_Readers()- start the read process
* launch_Writers() - start the write process
/*
 * Maintenance History:
 * --------------------
 * ver 1.0
 * Nov 22 2015
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Project4Starter
{
    class TestExecutive
    {
       private static int port_count = 3;
        //<----launch WPF----> 
        public void launch_WPF()
        {
            ProcessStartInfo wpf_client = new ProcessStartInfo(Path.GetFullPath("..\\..\\..\\WpfClient\\bin\\Debug\\WpfApplication1.exe"));
            Process.Start(wpf_client);
        }
        //<----launch server---->
        public void launch_Server()
        {
            ProcessStartInfo server = new ProcessStartInfo(Path.GetFullPath("..\\..\\..\\Server\\bin\\Debug\\Server.exe"));
            Process.Start(server);
        }

        //<----launch read client
        public void launch_Readers(string[] args)
        {
            int read_count = number_of_readers(args);
            string par_disp = "";
            if (processCommandLineForPartialLog(args))
                par_disp = " -Reader_Partial_Display";
            for (int i = 1; i <= read_count; i++)
            {
                ProcessStartInfo reader = new ProcessStartInfo(Path.GetFullPath("..\\..\\..\\Client2\\bin\\Debug\\Client2.exe"));
                reader.Arguments = "/L http://localhost:" + (8080 + port_count).ToString() + "/CommService" + par_disp;
                Process.Start(reader);

                ++port_count;
            }
        }
        //<-----launch write clients------------
        public void launch_Writers(string[] args)
        {
            int write_count = number_of_writers(args);
            string send_log = "";
            if (processCommandLineForWriteLog(args))
                send_log = " -Writer_Send_Message_Log";

            for (int i = 1; i <= write_count; i++)
            {
                ProcessStartInfo writer = new ProcessStartInfo(Path.GetFullPath("..\\..\\..\\Client\\bin\\Debug\\Client.exe"));
                writer.Arguments = "/L http://localhost:" + (8080 + port_count).ToString() + "/CommService" + send_log;
                Process.Start(writer);
                ++port_count;
            }
        }

        
        //<------get the number of read clients------------
        public int number_of_readers(string[] args)
        {
            int readers = 1;
            for (int i = 0; i < args.Length; ++i)
            {
                if ((args.Length > i + 1) && args[i].ToUpper() == "-READER_COUNT")
                {
                    readers = Int32.Parse(args[i + 1]);
                }
            }
            return readers;
        }
        
        //<------get the number of writeclients------------
        public int number_of_writers(string[] args)
        {
            int readers = 1;
            for (int i = 0; i < args.Length; ++i)
            {
                //Console.WriteLine("args = {0}", args[i]);
                //Console.WriteLine("args[{0}] = {1}", i, args[i]);
                if ((args.Length > i + 1) && args[i].ToUpper() == "-WRITER_COUNT")
                {
                    readers = Int32.Parse(args[i + 1]);
                }
            }
            return readers;
        }
        
        public bool processCommandLineForPartialLog(string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i].ToUpper() == "-READER_PARTIAL_DISPLAY")
                {
                    return true;
                }
            }
            return false;
        }

        public bool processCommandLineForWriteLog(string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i].ToUpper() == "-WRITER_SEND_MESSAGE_LOG")
                {
                    return true;
                }
            }
            return false;
        }


        static void Main(string[] args)
        {
            Console.Title = "Test Executive";
           
            TestExecutive tst = new TestExecutive();
            tst.launch_WPF();

            Thread.Sleep(500);
            tst.launch_Server();

            Thread.Sleep(500);

            Console.WriteLine("The read and write clients will be launched now");

            tst.launch_Writers(args);

            tst.launch_Readers(args);


        }
    }
}
