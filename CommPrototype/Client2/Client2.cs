///////////////////////////////////////////////////////////////////
// Client2.cs - CommService client sends and receives messages   //
// ver 2.1                                                       //
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
 * - Added using System.Threading
 * - Added reference to ICommService, Sender, Receiver, Utilities
 *
 * Note:
 * - in this incantation the client has Sender and now has Receiver to
 *   retrieve Server echo-back messages.
 * - If you provide command line arguments they should be ordered as:
 *   remotePort, remoteAddress, localPort, localAddress
 */
 /*
  * Required Files - 
 * ------------------------------
 * ICommService.cs, Utilities.cs, Receiver.cs
 * Sender.cs, Server.cs
/*
/* Public Interface - 
* -------------------------
* processCommandLine()- retrieve urls from the CommandLine 
* doAction() -  defines retrieveAction action delegate

 * Maintenance History:
 * --------------------
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
    string localUrl { get; set; } = "http://localhost:8082/CommService";
    string remoteUrl { get; set; } = "http://localhost:8080/CommService";

    //----< retrieve urls from the CommandLine if there are any >--------

    public void processCommandLine(string[] args)
    {
      if (args.Length == 0)
        return;
      localUrl = Util.processCommandLineForLocal(args, localUrl);
      remoteUrl = Util.processCommandLineForRemote(args, remoteUrl);
    }
        //--------------------------------------------------------------------------------//
        public Action doAction(Receiver rcvr)
        {
            Action recieveAction = () =>
            {
                Message msg = null;
                while (true)
                {
                    msg = rcvr.getMessage();   // note use of non-service method to deQ messages
                    Console.Write("\n  Received message:");

                    if (msg.content == "closeReceiver") break;
                    if (msg.content == "connection start message") continue;
                    //<-----------Code for Processing------------>
                    Console.WriteLine("Message = {0}", msg.content);
                    XElement xe = XElement.Parse(msg.content);
                    //Console.Write("\n======Testing parsing below========\n");
                    //Console.Write(xe.ToString());
                    string response = xe.Element("Result").Value;
                    Console.Write("\n========Displaying Server Response Below========\n");
                    Console.WriteLine(response);
                }
            };
            return recieveAction;
        }
        //----------------------------------------------------------------------------------------//
        static void Main(string[] args) {
            Thread.Sleep(2753);
         Console.Write("\n  starting CommService client"); Console.Write("\n =============================\n");
         Console.Title = "Client #2";

      Client clnt = new Client();
      clnt.processCommandLine(args);
      string localPort = Util.urlPort(clnt.localUrl);
      string localAddr = Util.urlAddress(clnt.localUrl);
      Receiver rcvr = new Receiver(localPort, localAddr);

      if (rcvr.StartService()){
      Action act = clnt.doAction(rcvr);
      rcvr.doService(act); }

      Sender sndr = new Sender(clnt.localUrl);  // Sender needs localUrl for start message
      Message msg = new Message();
      msg.fromUrl = clnt.localUrl;
      msg.toUrl = clnt.remoteUrl;
      Console.Write("\n  sender's url is {0}", msg.fromUrl);
      Console.Write("\n  attempting to connect to {0}\n", msg.toUrl);

      if (!sndr.Connect(msg.toUrl))  {
        Console.Write("\n  could not connect in {0} attempts", sndr.MaxConnectAttempts);
        sndr.shutdown();
        rcvr.shutDown();
        return; }
         
            XDocument xd = XDocument.Load("./../../../ReadDB.xml");
            XElement xe = xd.Element("root");      
            Console.WriteLine("<================[ XML is loaded]==========>");
            Console.Write(xd.ToString());
            Console.WriteLine("*****************\nxe=\n{0}", xe.ToString());
            int xCount = Int32.Parse(xe.Element("xCount").Value);
            for (int m = 0; m < xCount; m++)
            {foreach (var v in xe.Elements("Client2Message")){
                    msg = new Message();
                    msg.fromUrl = clnt.localUrl;
                    msg.toUrl = clnt.remoteUrl;
                    msg.content = v.ToString();
                    Console.Write("\n<<----------Sending Each message------------->>\n");
                    sndr.sendMessage(msg);
                    Thread.Sleep(400);}}
            Console.WriteLine("<==================#######===========================>");
            // Wait for user to press a key to quit.
            // Ensures that client has gotten all server replies.
            Util.waitForUser();
      // shut down this client's Receiver and Sender by sending close messages
      rcvr.shutDown();
      sndr.shutdown();
      Console.Write("\n\n"); }}}

