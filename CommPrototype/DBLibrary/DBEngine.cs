////////////////////////////////////////////////////////////////////////
// DBEngine.cs - define noSQL database                                //
// Ver 1.4                                                            //
// Application: Demonstration for CSE687-OOD, Project#4               //
// Language:    C#, ver 6.0, Visual Studio 2015                       //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10               //
// Author : Chiranth Bangalore Sathyaprakash                          //
// SUID: 258380492  , csathyap@syr.edu, 315-751-1129                  //
// Original Author:      Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com                 //
////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements DBEngine<Key, Value> where Value
 * is the DBElement<key, Data> type.
 *
 * This class is a starter for the DBEngine package .
 * It contains add,remove, get value methods.
 * It also contains the dictionary definition.
/*
 *Public Interface - 
 * ------------
 * Dbengine() - Initializes dictionary values
 * insert() -   Inserts  element into dbengine
 * delete() - removes element from dbengine
 * getValue() - gets the value of the key
 * Keys() - returns the key list
 *  
 * Maintenance:
 * ------------
 * Required Files: DBEngine.cs, DBElement.cs, and
 *                 UtilityExtensions.cs only if you enable the test stub
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.4 : 17 Nov 15
 * - changed namespace to Project4Starter
 * ver 1.3 : 07 Oct 15
 - Added remove method .
 * ver 1.2 : 24 Sep 15
 * - removed extensions methods and tests in test stub
 * - testing is now done in DBEngineTest.cs to avoid circular references
 * ver 1.1 : 15 Sep 15
 * - fixed a casting bug in one of the extension methods
 * ver 1.0 : 08 Sep 15
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
    public class DBEngine<Key, Value>
    {
        private Dictionary<Key, Value> dbStore;     // Dictionary to store the key-value pair
        public DBEngine()
        {
            dbStore = new Dictionary<Key, Value>();       // dbstore is the Dictionary
        }

        ////////-------------method to insert a new key and value to the database------/////////////

        public bool insert(Key key, Value val)
        {
            if (dbStore.Keys.Contains(key))
                return false;                    //if the dbstore already has a key then you cannot insert
            dbStore[key] = val;
            return true;
        }


        //------------// Method to remove a keyand thus its value i.e the db element from the database---------////////

        public bool delete(Key key1)
        {
            if (dbStore.Keys.Equals(null)) return false;
            dbStore.Remove(key1);
            return true;
        }

        //----// Method to get the value of the key and return true or false -------////////

        public bool getValue(Key key, out Value val)
        {
            if (dbStore.Keys.Contains(key))
            {
                val = dbStore[key];
                return true;
            }
            val = default(Value);       //If there is no key return default value
            return false;
        }


        ///---/////// This iterates over each element in a list---------//////////////////

        public IEnumerable<Key> Keys()
        {
            return dbStore.Keys;
        }

    }

#if (TEST_DBENGINE)                                     ///----/////test sub for this package----////

    class TestDBEngine
    {
        static void Main(string[] args)
        {
            "Testing DBEngine Package".title('=');
            WriteLine();

            Write("\n  All testing of DBEngine class moved to DBEngineTest package.");
            Write("\n  This allow use of DBExtensions package without circular dependencies.");

            Write("\n\n");
        }
    }
#endif
}
//********///////////////////=======================////////--------////////======================///////----/////////////////////