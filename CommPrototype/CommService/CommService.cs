//////////////////////////////////////////////////////////////////
// CommService.cs - Implementation of WCF message-passing service                              //
// ver 1.2                                                      //
// Application:  Demonstration for CSE681 SMA , Project#4       //
// Language:         C#, ver 6.0, Visual Studio 2015            //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10         //
// Author : Chiranth Bangalore Sathyaprakash                    //
// SUID: 258380492  , csathyap@syr.edu, 315-751-1129            //
// Original Author:Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com           //
///////////////////////////////////////////////////////////////////
/*
 * Additions to the C# Console Wizard code:
 * - added reference to System.ServiceModel
 * - added using System.ServiceModel
 * - added reference to Project4Starter.ICommService
 * - copied BlockingQueue.cs into project folder
 * - Added BlockingQueue.cs to project
 */
/* Public Interface - 
 * -------------------------
 * sendMessage() - sends message 
 * getMessage() - obtains the message that was sent
 *
 * Maintenance History:
 * --------------------
 * ver 1.2 : Nov 17
 * - changed namespace to Project4Starter
 * ver 1.1 : 24 Oct 2015
 * - added verbose mode to support debugging and learning
 * - to see more detail about what is going on in Sender and Receiver
 *   set Utilities.verbose = true
 * ver 1.0 : 18 Oct 2015
 * - first release
 * Required Files - 
 * ------------------------------
 * ICommService.cs, Utilities.cs
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Project4Starter
{
  using SWTools;
  using Util = Utilities;

  [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession)]
  public class CommService : ICommService
  {
    // static rcvrQueue is shared by all instances of this class

    private static SWTools.BlockingQueue<Message> rcvrQueue = 
      new SWTools.BlockingQueue<Message>();

    //----< called by clients, will only block briefly >-----------------

    public void sendMessage(Message msg)
    {
      if(Util.verbose)
        Console.Write("\n  this is CommService.sendMessage");
      rcvrQueue.enQ(msg);
    }
    //----< called by server, blocks caller while empty >----------------
    /*
     * Note: this is NOT a service method - see interface definition
     */
    public Message getMessage()
    {
      if(Util.verbose)
        Console.Write("\n  this is CommService.getMessage");
      return rcvrQueue.deQ();
    }
  }
}
