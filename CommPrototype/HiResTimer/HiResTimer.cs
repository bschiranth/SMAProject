//////////////////////////////////////////////////////////////////
///  HiResTimer.cs - High Resolution Timer - Uses Win32         //
///  ver 1.0         Performance Counters and .Net Interop      //
// Application:  Demonstration for CSE681 SMA , Project#4       //
// Language:         C#, ver 6.0, Visual Studio 2015            //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10         //
// Author : Chiranth Bangalore Sathyaprakash                    //
// SUID: 258380492  , csathyap@syr.edu, 315-751-1129            //
// Original Author:Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com           //
//////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////
/// Based on:                                                       ///
/// Windows Developer Magazine Column: Tech Tips, August 2002       ///
/// Author: Shawn Van Ness, shawnv@arithex.com                      ///
///////////////////////////////////////////////////////////////////////
/* Public Interface - 
 * -------------------------
 * HiResTimer() -calls performance functions
 * Start() - starts timer
 * Stop() - stops timer
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
 */
using System;
using System.Runtime.InteropServices; // for DllImport attribute
using System.ComponentModel; // for Win32Exception class
using System.Threading; // for Thread.Sleep method

namespace Project4Starter
{
   public class HiResTimer
    {
        protected ulong a, b, f;

        public HiResTimer()
        {
            a = b = 0UL;
            if (QueryPerformanceFrequency(out f) == 0)
                throw new Win32Exception();
        }

        public ulong ElapsedTicks
        {
            get
            { return (b - a); }
        }

        public ulong ElapsedMicroseconds
        {
            get
            {
                ulong d = (b - a);
                if (d < 0x10c6f7a0b5edUL) // 2^64 / 1e6
                    return (d * 1000000UL) / f;
                else
                    return (d / f) * 1000000UL;
            }
        }

        public TimeSpan ElapsedTimeSpan
        {
            get
            {
                ulong t = 10UL * ElapsedMicroseconds;
                if ((t & 0x8000000000000000UL) == 0UL)
                    return new TimeSpan((long)t);
                else
                    return TimeSpan.MaxValue;
            }
        }

        public ulong Frequency
        {
            get
            { return f; }
        }

        public void Start()
        {
            Thread.Sleep(0);
            QueryPerformanceCounter(out a);
        }

        public ulong Stop()
        {
            QueryPerformanceCounter(out b);
            return ElapsedTicks;
        }

        // Here, C# makes calls into C language functions in Win32 API
        // through the magic of .Net Interop

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern
           int QueryPerformanceFrequency(out ulong x);

        [DllImport("kernel32.dll")]
        protected static extern
           int QueryPerformanceCounter(out ulong x);
    }
}