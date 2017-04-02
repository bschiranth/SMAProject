//////////////////////////////////////////////////////////////////
// Client1.cs - CommService client sends and receives messages  //
// ver 2.2                                                      //
// Application:  Demonstration for CSE681 SMA , Project#4       //
// Language:     C#, ver 6.0, Visual Studio 2015                //
// Platform:     Dell Inspiron 15, Core-i5, Windows 10          //
// Author  :     Chiranth Bangalore Sathyaprakash               //
// SUID    :    258380492, csathyap@syr.edu, 315-751-1129       //
// Original Author:Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com           //
//////////////////////////////////////////////////////////////////

/*
 * Additions to C# Console Wizard generated code:
 * - Added using System.Threading
 * - Added reference to ICommService, Sender, Receiver, Utilities
 *
 * Note:
 * - in this incantation the client has Sender and now has Receiver to
 *   retrieve Server echo-back messages.
 * - If you provide command line arguments they should be ordered as:
 *   remotePort, remoteAddress, localPort, localAddress
 */
/* Public Interface - 
* -------------------------
* processCommandLine()- retrieve urls from the CommandLine 
* doAction() -  defines retrieveAction action delegate
/*
* Maintenance History:
* --------------------
* ver 2.5 : Nov 21 2015
* - added delegate to get replay message 
*   from server
* ver 2.4: Nov 17 2015
* - changed namespace to Project4Starter
* ver 2.3 : 22 
* ver 2.2 : 18 Nov 2015
* - Changed namespace to Project4Starter
* ver 2.1 : 29 Oct 2015
* - fixed bug in processCommandLine(...)
* - added rcvr.shutdown() and sndr.shutDown() 
* ver 2.0 : 20 Oct 2015
* - replaced almost all functionality with a Sender instance
* - added Receiver to retrieve Server echo messages.
* - added verbose mode to support debugging and learning
* - to see more detail about what is going on in Sender and Receiver
*   set Utilities.verbose = true
* ver 1.0 : 18 Oct 2015
* - first release
 * Required Files - 
 * ------------------------------
 * ICommService.cs, Utilities.cs,Receiver.cs
 * Sender.cs, Server.cs
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Project4Starter
{
    using Util = Utilities;

    ///////////////////////////////////////////////////////////////////////
    // Client class sends and receives messages in this version
    // - commandline format: /L http://localhost:8085/CommService 
    //                       /R http://localhost:8080/CommService
    //   Either one or both may be ommitted

    class Client
    {
        string localUrl { get; set; } = "http://localhost:8081/CommService";
        string remoteUrl { get; set; } = "http://localhost:8080/CommService";

        //----< retrieve urls from the CommandLine if there are any >--------

        public void processCommandLine(string[] args)
        {
            if (args.Length == 0)
                return;
            localUrl = Util.processCommandLineForLocal(args, localUrl);
            remoteUrl = Util.processCommandLineForRemote(args, remoteUrl);
        }

        //<-----------Action delegate is defined here------------>
        public Action doAction(Receiver rcvr)
        {
            //<-----Action delegate gets the recieved message from server----------->
            Action recieveAction = () =>
            {             
                Message msg = null;
                while (true)
                {
                    msg = rcvr.getMessage();   // note use of non-service method to deQ messages
                    Console.Write("\n  Received message:\n");
            
                    if (msg.content == "closeReceiver")             break;
                    if (msg.content == "connection start message") continue;
                    //<--------The message content is copied to an xelement and its
                    //   value is displayed  ------------>
                    Console.WriteLine("Message = {0}", msg.content);
                 XElement xe = XElement.Parse(msg.content);
                 string response = xe.Element("Result").Value;  // getting the response from serever
                   Console.Write("\n========Displaying Server Response Below========\n");
                 Console.WriteLine(response);
                    Console.WriteLine();
                }     
            };
            return recieveAction;
        }
        static void Main(string[] args)        {
            Thread.Sleep(2753);             
            Console.Write("\n  starting CommService client");
            Console.Write("\n =============================\n");
            Console.Title = "Client #1";
            Client clnt = new Client();                             // create new client object
            clnt.processCommandLine(args);
            string localPort = Util.urlPort(clnt.localUrl);         // Get the port and address value
            string localAddr = Util.urlAddress(clnt.localUrl);
            Receiver rcvr = new Receiver(localPort, localAddr);
           if (rcvr.StartService())             {                   // Get message if service is started
               Action act= clnt.doAction(rcvr);
                rcvr.doService(act);            }
            Sender sndr = new Sender(clnt.localUrl);  // Sender needs localUrl for start message
            Message msg = new Message();
            msg.fromUrl = clnt.localUrl;
            msg.toUrl = clnt.remoteUrl;
            Console.Write("\n  sender's url is {0}", msg.fromUrl);
            Console.Write("\n  attempting to connect to {0}\n", msg.toUrl);
            if (!sndr.Connect(msg.toUrl))            {  
                Console.Write("\n  could not connect in {0} attempts", sndr.MaxConnectAttempts);
                sndr.shutdown();        // shutdown if url was not connected
                rcvr.shutDown();
                return;            }
                XDocument LoadDoc = XDocument.Load("./../../../XMLdb.xml");
                Console.WriteLine("\n=============================DB will be loaded below=====================>\n");
                XElement xe = LoadDoc.Element("root");
                Console.WriteLine("=======******************************************====");
            
            Console.WriteLine("*****************\nxe=\n{0}", xe.ToString());
            int xCount = Int32.Parse(xe.Element("xCount").Value);// get count from client xml
            for (int k = 0; k < xCount; k++)             {
                foreach (var v in xe.Elements("ClientMessage"))                {
                     msg = new Message();
                    msg.fromUrl = clnt.localUrl;
                    msg.toUrl = clnt.remoteUrl;
                    msg.content = v.ToString();
                    Console.WriteLine("<<<<<<<<<<-----------------------------<<<<<<<<<<<");
                    sndr.sendMessage(msg);
                    Thread.Sleep(200);                }            }
                 Thread.Sleep(100);
                Util.waitForUser();
               rcvr.shutDown();
                sndr.shutdown();                    // send each message
            Console.Write("\n\n");            }        }    }

