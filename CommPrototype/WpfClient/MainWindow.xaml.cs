//////////////////////////////////////////////////////////////////
// MainWindows.xaml.cs - CommService GUI Client                 //
// ver 2.0                                                      //
// Application:  Demonstration for CSE681 SMA , Project#4       //
// Language:         C#, ver 6.0, Visual Studio 2015            //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10         //
// Author : Chiranth Bangalore Sathyaprakash                    //
// SUID: 258380492  , csathyap@syr.edu, 315-751-1129            //
// Original Author:Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com           //
///////////////////////////////////////////////////////////////////
/*
 * Required Files - 
 * ------------------------------
 * ICommService.cs, Utilities.cs,Receiver.cs
 * Sender.cs

 /* Public Interface - 
* -------------------------
* sendAttemptNotify() - shows how may attempts to send message were done
* sendExceptionNotify() -  sends the exception message that was got
 */
/*
 * Additions to C# WPF Wizard generated code:
 * - Added reference to ICommService, Sender, Receiver, MakeMessage, Utilities
 * - Added using Project4Starter
 *
 * Note:
 * - This client receives and sends messages.
 */
/*
 * Plans:
 * - Add message decoding and NoSqlDb calls in performanceServiceAction.
 * - Provide requirements testing in requirementsServiceAction, perhaps
 *   used in a console client application separate from Performance 
 *   Testing GUI.
 */
/*
 * Maintenance History:
 * --------------------
 * ver 2.0 : 29 Oct 2015
 * - changed Xaml to achieve more fluid design
 *   by embedding controls in grid columns as well as rows
 * - added derived sender, overridding notification methods
 *   to put notifications in status textbox
 * - added use of MessageMaker in send_click
 * ver 1.0 : 25 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Project4Starter;

namespace WpfApplication1
{
  public partial class MainWindow : Window
  {
    static bool firstConnect = true;
    static Receiver rcvr = null;
    static wpfSender sndr = null;
    string localAddress = "localhost";
    string localPort = "8081";
    string remoteAddress = "localhost";
    string remotePort = "8080";

    /////////////////////////////////////////////////////////////////////
    // nested class wpfSender used to override Sender message handling
    // - routes messages to status textbox
    public class wpfSender : Sender
    {
      TextBox lStat_ = null;  // reference to UIs local status textbox
      System.Windows.Threading.Dispatcher dispatcher_ = null;

      public wpfSender(TextBox lStat, System.Windows.Threading.Dispatcher dispatcher)
      {
        dispatcher_ = dispatcher;  // use to send results action to main UI thread
        lStat_ = lStat;
      }
            //<--------send the notification message------>
      public override void sendMsgNotify(string msg)
      {
        Action act = () => { lStat_.Text = msg; };
        dispatcher_.Invoke(act);

      }
            //<-------sends the exception message that was got
            public override void sendExceptionNotify(Exception ex, string msg = "")
      {
        Action act = () => { lStat_.Text = ex.Message; };
        dispatcher_.Invoke(act);
      }
            //<------shows how may attempts to send message were done
            public override void sendAttemptNotify(int attemptNumber)
      {
        Action act = null;
        act = () => { lStat_.Text = String.Format("attempt to send #{0}", attemptNumber); };
        dispatcher_.Invoke(act);
      }
    }
    public MainWindow()
    {
      InitializeComponent();
      lAddr.Text = localAddress;
      lPort.Text = localPort;
      rAddr.Text = remoteAddress;
      rPort.Text = remotePort;
      Title = "Prototype WPF Client";
            setupChannel();                 // setup the channel
      //send.IsEnabled = false;
    }
    //----< trim off leading and trailing white space >------------------

    string trim(string msg)
    {
      StringBuilder sb = new StringBuilder(msg);
      for(int i=0; i<sb.Length; ++i)
        if (sb[i] == '\n')
          sb.Remove(i,1);
      return sb.ToString().Trim();
    }
    //----< indirectly used by child receive thread to post results >----

    public void postRcvMsg(string content)
    {
      TextBlock item = new TextBlock();
      item.Text = trim(content);
      item.FontSize = 16;
      rcvmsgs.Items.Insert(0, item);
    }
    //----< used by main thread >----------------------------------------

    public void postSndMsg(string content)
    {
      TextBlock item = new TextBlock();
      item.Text = trim(content);
      item.FontSize = 16;
      //sndmsgs.Items.Insert(0, item);
    }
    //----< get Receiver and Sender running >----------------------------

    void setupChannel()
    {
      rcvr = new Receiver(localPort, localAddress);
      Action serviceAction = () =>
      {
        try
        {
          Message rmsg = null;
          while (true)
          {
            rmsg = rcvr.getMessage();
            Action act = () => { postRcvMsg(rmsg.content); };
            Dispatcher.Invoke(act, System.Windows.Threading.DispatcherPriority.Background);
          }
        }
        catch(Exception ex)
        {
              // Action act = () => { lStat.Text = ex.Message; };
              //   Dispatcher.Invoke(act);
              Console.WriteLine(ex.Message);
          }
      };
      if (rcvr.StartService())
      {
        rcvr.doService(serviceAction);
      }

     // sndr = new wpfSender(lStat, this.Dispatcher);
    }
    //----< set up channel after entering ports and addresses >----------

    private void start_Click(object sender, RoutedEventArgs e)
    {
      localPort = lPort.Text;
      localAddress = lAddr.Text;
      remoteAddress = rAddr.Text;
      remotePort = rPort.Text;

      if (firstConnect)
      {
        firstConnect = false;
        if (rcvr != null)
          rcvr.shutDown();
        setupChannel();
      }
      //rStat.Text = "connect setup";
    ///  send.IsEnabled = true;
   //   connect.IsEnabled = false;
      lPort.IsEnabled = false;
      lAddr.IsEnabled = false;
    }
    //----< send a demonstraton message >--------------------------------

    private void send_Click(object sender, RoutedEventArgs e)
    {try{
        #region
        /////////////////////////////////////////////////////
        // This commented code was put here to allow
        // user to change local port and address after
        // the channel was started. 
        // It does what is intended, but would throw 
        // if the new port is assigned a slot that
        // is in use or has been used since the
        // TCP tables were last updated.
        // if (!localPort.Equals(lPort.Text))
        //{
        //   localAddress = rcvr.address = lAddr.Text;
        //   localPort = rcvr.port = lPort.Text;
        //   rcvr.shutDown();
        //   setupChannel();
        // }
        #endregion
        if (!remoteAddress.Equals(rAddr.Text) || !remotePort.Equals(rPort.Text))
        { remoteAddress = rAddr.Text;
          remotePort = rPort.Text;}
        // - Make a demo message to send
        // - You will need to change MessageMaker.makeMessage
        //   to make messages appropriate for your application design
        // - You might include a message maker tab on the UI
        //   to do this.

        MessageMaker maker = new MessageMaker();
        Message msg = maker.makeMessage(
          Utilities.makeUrl(lAddr.Text, lPort.Text), 
          Utilities.makeUrl(rAddr.Text, rPort.Text)
        );
        //lStat.Text = "sending to" + msg.toUrl;
        sndr.localUrl = msg.fromUrl;
        sndr.remoteUrl = msg.toUrl;
      //  lStat.Text = "attempting to connect";
       // if (sndr.sendMessage(msg))
       //   lStat.Text = "connected";
       // else
      //    lStat.Text = "connect failed";
        postSndMsg(msg.content);
      }
      catch(Exception ex)
      {
                Console.WriteLine(ex.Message);
    //    lStat.Text = ex.Message;
      }
    }

        private void sndmsgs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("The latency will be displayed below");
        }
    }
}
