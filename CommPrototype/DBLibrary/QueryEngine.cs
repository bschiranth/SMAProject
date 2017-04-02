//////////////////////////////////////////////////////////////////
// QueryEngine.cs - Supports queries for noSQL database         //
// Ver 1.1                                                      //
// Application:  Demonstration for CSE681 SMA , Project#4       //
// Language:         C#, ver 6.0, Visual Studio 2015            //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10         //
// Author : Chiranth Bangalore Sathyaprakash                    //
//  SUID: 258380492  , csathyap@syr.edu, 315-751-1129           //
// Original Author:Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com           //
//////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements the queries for the noSQL database.Here, different queries like
 * query for getting  children of a DB element, returning keys that match a specified
 * pattern and others are defined.
/*
 * Public Interface:
 *-----------------------------
 * queryValue()  -   gets the value of the key
 * getChildren() -   gets the children list of the specified key
 * KeyPattern()  -   get the keys of specified pattern
 * StringPattern() - returns keys of specified pattern
 * TimeKeys() - returns keys withing time interval

 * Maintenance:
 * ------------
 * Required Files: DBElement.cs, UtilityExtensions.cs,DBEngine.cs
 *                  DBExtensions.cs, Display.cs,QueryEngine.cs 
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.1 :  17 Nov 15
 * - changed namespace to Project4Starter
 * ver 1.0 :  09 Oct 15
 * - first release
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4Starter
{
    public class QueryEngine
    {

        //<----// This method returns the value of the specified key-------//////
        public Value queryValue<Key, Value, Data>(DBEngine<Key, Value> db, Key key)
        {

            Value getQueryValue;
            bool keyStatus = db.getValue(key, out getQueryValue);

            if (keyStatus) return getQueryValue;
            else
            {
                Console.WriteLine("No Key Present");
                return default(Value);
            }
        }

        ////////////////////--------// This method returns the children of the specified key------////////
        public List<Key> getChildren<Key, Value, Data>(DBEngine<Key, Value> db, Key key)
        {
            try
            {
                Value getQueryValue;
                bool keyStatus = db.getValue(key, out getQueryValue);

                DBElement<Key, Data> DBelem = getQueryValue as DBElement<Key, Data>;

                if (keyStatus) return DBelem.children;
                else
                {
                    Console.WriteLine("No key present");
                    return default(List<Key>);
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(" keys");
                return default(List<Key>);
            }
        }

        //------////<---//  This method returns the list of keys that match the specified pattern-----//

        public List<Key> KeyPattern<Key, Value, Data, Type>(DBEngine<Key, Value> db, Type pattern)
        {
            List<Key> matchedKeys = new List<Key>();          // matched keys conatin the list of keys that match the pattern
            try
            {
                foreach (Key key in db.Keys())
                {
                    Value patternValue;                  // the pattern passed to this method
                    db.getValue(key, out patternValue);
                    DBElement<Key, Data> DBelem = patternValue as DBElement<Key, Data>;
                    string match = pattern.ToString();

                    if (key.ToString().Contains(match)) matchedKeys.Add(key);     // Add keys to list if matched
                    else
                    {
                        Console.WriteLine("No matched type found in {0}", key);

                    }
                }

                return matchedKeys;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("No matched string found");
                return default(List<Key>);
            }
        }

        //----------////<---// Returns all the keys that match the string pattern---//////------//
        //-------------// The metadata elements like name and description are tested for pattern matching----//
        public List<Key> StringPattern<Key, Value, Data>(DBEngine<Key, Value> db, string stringPattern)
        {
            try
            {
                List<Key> matchedKeys = new List<Key>();        //contains list of keys

                foreach (Key key in db.Keys())
                {
                    Value strValue;
                    db.getValue(key, out strValue);
                    DBElement<Key, Data> DBelem = strValue as DBElement<Key, Data>;

                    //---// here metadata elemets are searched for string pattern----/////
                    if (DBelem.name.Contains(stringPattern) || DBelem.descr.Contains(stringPattern)
                        || DBelem.timeStamp.ToString().Contains(stringPattern))
                        matchedKeys.Add(key);
                    else Console.WriteLine("No matched string found in {0}", key);

                    foreach (var v in DBelem.children)
                    {
                        string convertedString = v.ToString();
                        if (convertedString.Contains(stringPattern)) matchedKeys.Add(key);
                    }
                }

                return matchedKeys;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(" pattern found");
                return default(List<Key>);
            }
        }

        //------------------//////////////////////////---------------////////////////////////////////////////
        //--------------///////  This method returns the list of keys that fall within the specified time interval---//

        public List<Key> TimeKeys<Key, Value, Data>(DBEngine<Key, Value> db, string time1, string time2)
        {
            int flag = 0;
            DateTime newTime = new DateTime();
            List<Key> timeKeys = new List<Key>();
            DateTime finalTime1 = Convert.ToDateTime(time1);
            DateTime finalTime2 = Convert.ToDateTime(time2);

            foreach (Key key in db.Keys())
            {
                Value timeValue;
                db.getValue(key, out timeValue);
                DBElement<Key, Data> DBelem = timeValue as DBElement<Key, Data>;

                //---// if one of the time is null assume it is current time-----------//
                if (time1.Equals(null))
                {
                    newTime = DateTime.Now;
                    if ((newTime < DBelem.timeStamp) && (DBelem.timeStamp < finalTime2)) timeKeys.Add(key);
                    flag = 1;
                }
                //-------------// if timestamp falls within the time interval key is added to list--------//
                if ((finalTime1 < DBelem.timeStamp) && (DBelem.timeStamp < finalTime2))
                {
                    timeKeys.Add(key);
                    flag = 1;
                }
            }

            if (flag == 0) Console.WriteLine("Timestamp doesnot exist within the time interval");
            return timeKeys;
        }
    }

    ///////////////////////////////////////////===========================================///////////////////////////


#if (Test_QueryEngine)              // test stub for the query engine

    class Test_QueryEngine
    {
        public static void Main()
        {

            "Testing QueryEngine Package".title('=');
            Console.WriteLine();

            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            DBElement<int, string> HP1 = new DBElement<int, string>();
            DBElement<int, string> HP2 = new DBElement<int, string>();
            DBElement<int, string> HP3 = new DBElement<int, string>();

            HP1.name = "Harry Potter";
            HP1.descr = "The boy who lived";
            HP1.payload = "Defeated Lord voldemort";
            HP1.timeStamp = DateTime.Now;
            HP1.children.AddRange(new List<int> { 10, 101, 1010 });
            db.insert(4, HP1);

            HP2.name = " Ron Weasley";
            HP2.descr = "Potters friend";
            HP2.payload = "the keeper in quiddich";
            HP2.timeStamp = DateTime.Now;
            HP2.children.AddRange(new List<int> { 20, 202, 2020 });
            db.insert(5, HP2);

            HP3.name = "Hermione Granger";
            HP3.descr = "harry's other friend";
            HP3.payload = "Has extensive knowledge in all of magic world \n";
            HP3.timeStamp = DateTime.Now;
            HP3.children.AddRange(new List<int> { 30, 303, 3030 });
            db.insert(6, HP3);

            Console.WriteLine("-----------------------db is shown.------------------------");
            db.showDB();

            Console.WriteLine("==================================================================================");
            ////////////////////////////////////////////////////////////////////
            QueryEngine QE = new QueryEngine();
            Console.WriteLine("-----------------returns value of key--------------");
            DBElement<int, string> showElem = new DBElement<int, string>();
            showElem = QE.queryValue<int, DBElement<int, string>, string>(db, 4);
            if (showElem != null) showElem.showElement();
            ////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("==================================================================================");
            Console.WriteLine("-----------------returns children of the specified key--------------");
            List<int> ChildList = new List<int>();
            ChildList = QE.getChildren<int, DBElement<int, string>, string>(db, 4);
            if (ChildList != null) foreach (int i in ChildList) Console.WriteLine(i);
            Console.WriteLine("==================================================================================");
            //////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("Pattern matching\n");
            List<int> newKeyList = new List<int>();
            newKeyList = QE.KeyPattern<int, DBElement<int, string>, string, int>(db, 4);
            if (newKeyList != null) foreach (int k in newKeyList) Console.WriteLine("Matched in key {0}", k);
            ////////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("==================================================================================");
            Console.WriteLine("string matching");
            List<int> KeyList = new List<int>();
            KeyList = QE.StringPattern<int, DBElement<int, string>, string>(db, "Ron");
            foreach (int j in KeyList) Console.WriteLine("Matched in key {0}", j);
            Console.WriteLine("==================================================================================");
            /////////////////////////////////////////////////////////////////////////////

            Console.WriteLine(" keys within time interval");
            List<int> Time_key_List = new List<int>();
            DateTime t1 = new DateTime();
            DateTime t2 = new DateTime();

            //////////////////////////////////////////////////////////////////////////////////



            string formater = "HH:mm";   //hour minute format.

            string TimeString1 = t1.ToString(formater);
            string TimeString2 = t2.ToString(formater);

            KeyList = QE.TimeKeys<int, DBElement<int, string>, string>(db, "08:10", "22:40");
            foreach (int j in KeyList) Console.WriteLine(j);
            Console.WriteLine("==================================================================================");
            Console.WriteLine("=====================for list of string ============================================");
            //////////////******************////////////////////////////////
            DBEngine<string, DBElement<string, List<string>>> SecondDB = new DBEngine<string, DBElement<string, List<string>>>();
            DBElement<string, List<string>> HP4 = new DBElement<string, List<string>>();
            DBElement<string, List<string>> HP5 = new DBElement<string, List<string>>();
            DBElement<string, List<string>> HP6 = new DBElement<string, List<string>>();

            HP4.name = "Sirius Black";
            HP4.descr = "Harry's Godfather";
            HP4.timeStamp = DateTime.Now;
            HP4.children.AddRange(new List<string> { "dog", "black family", "prisoner" });
            HP4.payload = new List<string> { "Azkaban", "Order of Phoenix", "Maurders Map" };
            SecondDB.insert("first", HP4);


            HP5.name = "Bellatrix Lestrange";
            HP5.descr = "Voldemorts right hand";
            HP5.timeStamp = DateTime.Now;
            HP5.children.AddRange(new List<string> { "imperio", "crucio", "avedakedavra", "Deatheater" });
            HP5.payload = new List<string> { "Malfoy", "dobby", "crazy" };
            SecondDB.insert("second", HP5);


            HP6.name = "Severus Snape";
            HP6.descr = "Dumbeldores spy";
            HP6.timeStamp = DateTime.Now;
            HP6.children.AddRange(new List<string> { "professor", "hogwarts", "potions" });
            HP6.payload = new List<string> { "Loved Lilly", " gifted", "slytherin", "ex-Deatheater" };
            SecondDB.insert("third", HP6);
            SecondDB.showEnumerableDB();
            Console.WriteLine("==================================================================================");


            ///////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("=================Get value===============================");
            DBElement<string, List<string>> elemL = new DBElement<string, List<string>>();
            elemL = QE.queryValue<string, DBElement<string, List<string>>, string>(SecondDB, "first");
            Console.WriteLine("Going to call show element.");
            if (elemL != null) elemL.showEnumerableElement();
            Console.WriteLine("==================================================================================");
            ////////////////////////////////////////////////////////////////////////////

            Console.WriteLine("=======================Children list========================================");
            try
            {
                List<string> ListOfChildren = new List<string>();
                ListOfChildren = QE.getChildren<string, DBElement<string, List<string>>, string>(SecondDB, "first");
                Console.WriteLine(ListOfChildren);
                if (ListOfChildren != null)
                {
                    foreach (string str in ListOfChildren) Console.WriteLine(str);
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Reference error");
            }
            Console.WriteLine("==================================================================================");
            ///////////////////////////////////////////////////////////////////////////////

            Console.WriteLine("=================Pattern matching test========================================");
            Console.WriteLine("\n");

            List<string> testKeys = new List<string>();
            testKeys = QE.KeyPattern<string, DBElement<string, List<string>>, string, string>(SecondDB, "third");
            if (testKeys != null) foreach (string st in testKeys) Console.WriteLine("found in key{0}", st);
            Console.WriteLine("==================================================================================");
            ////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("==============================string matching==============================");
            List<string> KeyStringList = new List<string>();
            KeyStringList = QE.StringPattern<string, DBElement<string, List<string>>, string>(SecondDB, "Snape");
            foreach (int j in KeyList) Console.WriteLine(j);
            Console.WriteLine("==================================================================================");
            /////////////////////////////////////////////////////////////////////////

            Console.WriteLine("test for Time of keys");
            List<string> ListTime = new List<string>();
            DateTime T1 = new DateTime();
            DateTime T2 = new DateTime();


            string FORMAT = "HH:mm";   // Use this format.

            string String_T1 = T1.ToString(FORMAT);
            string String_T2 = T2.ToString(FORMAT);

            KeyList = QE.TimeKeys<int, DBElement<int, string>, string>(db, "00:10", "23:40");
            foreach (string stringTime in ListTime) Console.WriteLine(stringTime);
            Console.WriteLine("==================================================================================");
        }

    }

#endif
}
//********///////////////////=======================////////--------////////======================///////----/////////////////////
