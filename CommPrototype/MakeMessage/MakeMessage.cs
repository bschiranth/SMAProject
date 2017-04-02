///////////////////////////////////////////////////////////////////
// MessageMaker.cs - Construct ICommService Messages            //
// ver 1.0                                                      //
// Application:  Demonstration for CSE681 SMA , Project#4       //
// Language:         C#, ver 6.0, Visual Studio 2015            //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10         //
// Author : Chiranth Bangalore Sathyaprakash                    //
// SUID: 258380492  , csathyap@syr.edu, 315-751-1129            //
// Original Author:Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com           //
///////////////////////////////////////////////////////////////////

/*
 * Purpose:
 *----------
 * This is a placeholder for application specific message construction
 *
 * Additions to C# Console Wizard generated code:
 * - references to ICommService and Utilities
 */
/*
/* Public Interface - 
 * -------------------------
 * makeMessage() - creates a format for 
 * printing from url , to url and content
 * along with message count
 *
 * Maintenance History:
 * --------------------
 * ver 1.1 : Nov 17 2015
 * - changed namespace to Project4Starter
 * ver 1.0 : 29 Oct 2015
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

namespace Project4Starter
{
  public class MessageMaker
  {
    public static int msgCount { get; set; } = 0;
        //<------ gets URl and formats the content ------------
        public Message makeMessage(string fromUrl, string toUrl)
    {
      Message msg = new Message();
      msg.fromUrl = fromUrl;        
      msg.toUrl = toUrl;
      msg.content = String.Format("\n  message #{0}", ++msgCount);
      return msg;
    }
#if (TEST_MESSAGEMAKER)
    static void Main(string[] args)
    {
      MessageMaker mm = new MessageMaker();
      Message msg = mm.makeMessage("fromFoo", "toBar");
      Utilities.showMessage(msg);
      Console.Write("\n\n");
    }
#endif
  }
}
