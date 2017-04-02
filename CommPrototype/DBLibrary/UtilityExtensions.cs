﻿/////////////////////////////////////////////////////////////////////
// UtilityExtensions.cs - define methods to simplify project code  //
// Ver 1.1                                                         //
// Application: Demonstration for CSE687-OOD, Project#4            //
// Language:    C#, ver 6.0, Visual Studio 2015                    //
//Platform:      Dell Inspiron 15, Core-i5, Windows 10             //
// Author :    Chiranth Bangalore Sathyaprakash                    //
//  SUID: 258380492  , csathyap@syr.edu, 315-751-1129              //
// Original Author: Jim Fawcett, CST 4-187, Syracuse University    //
//              (315) 443-3948, jfawcett@twcny.rr.com              //
/////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements utility extensions that are not specific
 * to a single package.
 */
/*
 * Public Interface
 * ----------------------
 * title() - displays the title in the specific format

 * Maintenance:
 * ------------
 * Required Files: UtilityExtensions.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.1 : 17 Nov 15
 * - changed namespace to Project4Starter
 * ver 1.0 : 13 Sep 15
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Project4Starter
{
    public static class UtilityExtensions
    {
        public static void title(this string aString, char underline = '-')
        {
            Console.Write("\n  {0}", aString);
            Console.Write("\n {0}", new string(underline, aString.Length + 2));
        }
    }
    public class TestUtilityExtensions
    {
        static void Main(string[] args)
        {
            "Testing UtilityExtensions.title".title();
            Write("\n\n");
        }
    }
}
//********///////////////////=======================////////--------////////======================///////----/////////////////////