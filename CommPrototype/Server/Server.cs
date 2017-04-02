//////////////////////////////////////////////////////////////////
// Server.cs - CommService server                               //
// ver 2.2                                                      //
// Application:  Demonstration for CSE681 SMA , Project#4       //
// Language:         C#, ver 6.0, Visual Studio 2015            //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10         //
// Author : Chiranth Bangalore Sathyaprakash                    //
// SUID: 258380492  , csathyap@syr.edu, 315-751-1129            //
// Original Author:Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com           //
///////////////////////////////////////////////////////////////////

/*
 * Additions to C# Console Wizard generated code:
 * - Added reference to ICommService, Sender, Receiver, Utilities
 *
 * Note:
 * - This server now receives and then sends back received messages.
 */
/*
 * Plans:
 * - Add message decoding and NoSqlDb calls in performanceServiceAction.
 * - Provide requirements testing in requirementsServiceAction, perhaps
 *   used in a console client application separate from Performance 
 *   Testing GUI.
 */
/* * Required Files - 
 * ------------------------------
 * ICommService.cs, Utilities.cs,Receiver.cs
 * Sender.cs, HiResTimer.cs,P2Lib.cs, PersistDB.cs,XMLtoDB.cs
 */
/* Public Interface - 
* -------------------------
* processCommandLine()- retrieve urls from the CommandLine 
* send_WPF_msg() - send the performance results to WPF
/*

 * Maintenance History:
 * --------------------
 * ver 2.3 : 29 Oct 2015
 * - added handling of special messages: 
 *   "connection start message", "done", "closeServer"
 * ver 2.2 : 25 Oct 2015
 * - minor changes to display
 * ver 2.1 : 24 Oct 2015
 * - added Sender so Server can echo back messages it receives
 * - added verbose mode to support debugging and learning
 * - to see more detail about what is going on in Sender and Receiver
 *   set Utilities.verbose = true
 * ver 2.0 : 20 Oct 2015
 * - Defined Receiver and used that to replace almost all of the
 *   original Server's functionality.
 * ver 1.0 : 18 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Project4Starter
{
    using Project4Starter;
    using Util = Utilities;

    class Server
    {
        string address { get; set; } = "localhost";
        string port { get; set; } = "8080";

        //----< quick way to grab ports and addresses from commandline >-----

        public void ProcessCommandLine(string[] args)
        {
            if (args.Length > 0)
            {
                port = args[0];
            }
            if (args.Length > 1)
            {
                address = args[1];
            }
        }

        //<--- send the performance data to WPf to be displayed---->
        public void send_WPF_msg(ulong process_time,Sender sndr)
        {
            Message wpf_msg = new Message();
            wpf_msg.toUrl = "http://localhost:8081/CommService";
            wpf_msg.fromUrl = "http://localhost:8080/CommService";

            string latency = "Server(8080) : Processing Time = " + process_time + " micro-seconds";
            wpf_msg.content = latency;
            sndr.sendMessage(wpf_msg);
        }

        static void Main(string[] args)
        {            Util.verbose = false;
            Server srvr = new Server();
            srvr.ProcessCommandLine(args);
            Console.Title = "Server";
            Console.Write(String.Format("\n  Starting CommService server listening on port {0}\n\n", srvr.port));
            Sender sndr = new Sender(Util.makeUrl(srvr.address, srvr.port));
            //Sender sndr = new Sender();
            Receiver rcvr = new Receiver(srvr.port, srvr.address);
            // - serviceAction defines what the server does with received messages
            // - This serviceAction just announces incoming messages and echos them
            //   back to the sender.  
            // - Note that demonstrates sender routing works if you run more than
            //   one client.
              Action serviceAction = () => {
                DBEngine<int, DBElement<int, string>> dbengine = new DBEngine<int, DBElement<int, string>>();
                QueryEngine qe = new QueryEngine();
               PersistEngine pe = new PersistEngine();
                HiResTimer hrt = new HiResTimer();
                Message msg = null;
                while (true)  {
                    msg = rcvr.getMessage();   // note use of non-service method to deQ messages
                    Console.Write("\n  Received message:\n");
                    Console.WriteLine("<--------------------------------------------->");
                    Console.Write("\n  sender is {0}\n", msg.fromUrl);
                    Console.Write("\n  content is {0}\n", msg.content);
                    if (msg.content == "connection start message")
                        continue; // don't send back start message
                    if (msg.content == "done")                    {
                        Console.Write("\n  client has finished\n");   continue;                    }
                    if (msg.content == "closeServer")
                    {
                        Console.Write("received closeServer");
                        break;
                    }
                    hrt.Start();            //start timer
                    XElement parsedElement = XElement.Parse(msg.content);
                    XElement res = new XElement("Result","Method not found");
                    RemoteDBEditor rdb = new RemoteDBEditor();
                    Console.WriteLine("<---------Write Client Operations----------->");
                   //<---------all the tags are checked for various read write client operations---->
                    if (parsedElement.Element("QueryType").Value.Equals("Insert"))
                    res = rdb.InsertDBElement(parsedElement, dbengine); 
                    else if (parsedElement.Element("QueryType").Value.Equals("Delete"))
                    res = rdb.DeleteKey(parsedElement, dbengine);       
                    else if (parsedElement.Element("QueryType").Value.Equals("Edit"))
                    res = rdb.EditTextMetadata(parsedElement, dbengine);
                    else if (parsedElement.Element("QueryType").Value.Equals("gettingValue"))
                    res = rdb.getValue(parsedElement,dbengine,qe);
                    else if (parsedElement.Element("QueryType").Value.Equals("gettingChildren"))
                    res = rdb.getChildren(parsedElement, dbengine, qe);
                    else if (parsedElement.Element("QueryType").Value.Equals("KeyPatternMatching"))
                    res = rdb.KeyPatternMatching(parsedElement, dbengine, qe);
                    else if (parsedElement.Element("QueryType").Value.Equals("Persist"))
                    res = rdb.persist(dbengine,pe);
                    else if(parsedElement.Element("QueryType").Value.Equals("Load"))
                    res = rdb.load(dbengine, pe);
                    else
                    Console.WriteLine(" Operation cannot be done, problem in XML");// failure message
                    Console.WriteLine("<----------------Sending Response back to Client--------------------->");
                    XElement Response = new XElement("ResponseMessage");
                    Response.Add(res);
                    hrt.Stop();     //stop timer
                    Console.WriteLine("\n THe elapsed time is = {0}",hrt.ElapsedMicroseconds + "microSeconds");
                    srvr.send_WPF_msg(hrt.ElapsedMicroseconds, sndr);   //send elapsed time data to WPF
                    // swap urls for outgoing message
                    Util.swapUrls(ref msg); 
                    msg.content = Response.ToString(); 
                    sndr.sendMessage(msg);          // response message back to client
                }
            };
             if (rcvr.StartService())
                rcvr.doService(serviceAction); // This serviceAction is asynchronous
Util.waitForUser();        }    }}
