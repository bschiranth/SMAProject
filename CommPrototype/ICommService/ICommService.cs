//////////////////////////////////////////////////////////////////
// ICommService.cs - Contract for WCF message-passing service   //
// ver 1.2                                                      //
// Application:  Demonstration for CSE681 SMA , Project#4       //
// Language:         C#, ver 6.0, Visual Studio 2015            //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10         //
// Author : Chiranth Bangalore Sathyaprakash                    //
// SUID: 258380492  , csathyap@syr.edu, 315-751-1129            //
// Original Author:Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com           //
//////////////////////////////////////////////////////////////////

/*
 * Additions to C# Console Wizard generated code:
 * - Added reference to System.ServiceModel
 * - Added using System.ServiceModel
 * - Added reference to System.Runtime.Serialization
 * - Added using System.Runtime.Serialization
 */
/*
/* Public Interface - 
 * -------------------------
 * sendMessage() - sends message 
 *
 * Maintenance History:
 * --------------------
 * ver 1.2 : Nov 17 2015
 * - changed namespace to Project4Starter
 * ver 1.1 : 29 Oct 2015
 * - added comment in data contract
 * ver 1.0 : 18 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Project4Starter
{
  [ServiceContract (Namespace ="Project4Starter")]
  public interface ICommService
  {
    [OperationContract(IsOneWay = true)]
    void sendMessage(Message msg);
  }

  [DataContract]
  public class Message
  {
    [DataMember]
    public string fromUrl { get; set; }
    [DataMember]
    public string toUrl { get; set; }
    [DataMember]
    public string content { get; set; }  // will hold XML defining message information
  }
}
