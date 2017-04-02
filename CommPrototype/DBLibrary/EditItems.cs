//////////////////////////////////////////////////////////////////
// EDitItem.cs - Define element for noSQL database             //
// Ver 1.2                                                      //
// Application:  Demonstration for CSE681 SMA , Project#4      //
// Language:         C#, ver 6.0, Visual Studio 2015            //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10         //
// Author : Chiranth Bangalore Sathyaprakash                    //
//  SUID: 258380492  , csathyap@syr.edu, 315-751-1129           //
// Original Author:Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com           //
///////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This packagae implemets the insert and remove methods ..
 * It also implements editing text metadata,
 * adding and removing the child relationships 
 */
/*
 * Public Interface:
 * *--------------------
 * AddRelation<Key, Value, Data>() -  Adds relationship to element
 * RemoveRelation<Key, Value, Data>() - Removes relationship from an element
 * EditTextMetaData() - edits the metadata elements     
 * 
 * Maintenance:
 * ------------
 * Required Files: EditItem.cs, DBElement.cs, UtilityExtensions.cs, 
 *                 DBEngine.cs, Display.cs, DBExtensions
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.2 : 17 Nov 15
 * - changed namespace to Project4Starter
 * ver 1.1 : 08 Oct 15
 *          - Added AddRelationship, and
 *            Remove Relationship methods    
 * ver 1.0 : 07 Oct 15
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
    public static class EditItems
    {
        // ---//-//This method is used to add a key to the list of children of another element //--//---//
        //  It  is like adding relationship ---///
        public static void AddRelation<Key, Value, Data>(this DBEngine<Key, Value> db, Key key1, Key key2)
        {
            Value Val1;
            try
            {
                // <---/// If the db engine does not contain the key then it cannot be added-----//
                if (!db.getValue(key1, out Val1)) Console.WriteLine("No key present\n");
                else
                {
                    DBElement<Key, Data> DBelem = Val1 as DBElement<Key, Data>;

                    if (DBelem.children.Contains(key2))
                        Console.WriteLine("\n key already exists in the children list\n");
                    else
                        DBelem.children.Add(key2);          // the key is appended to the childrens list--//
                }
            }
            catch (NullReferenceException)          // this is to catch the NullReferenceException
            {
                Console.WriteLine("Give the correct key");
            }
        }

        //----//--// This method is used to remove an existing relationship  ----///---////


        public static void RemoveRelation<Key, Value, Data>(this DBEngine<Key, Value> db, Key key1, Key key2)
        {
            Value Val2;

            if (!db.getValue(key1, out Val2))
                Console.WriteLine("No key present");
            else

            {
                //----/ If the elemets key has the key we are looking for, it can be deleted from childrens list--///

                DBElement<Key, Data> DBelem = Val2 as DBElement<Key, Data>;

                if (DBelem.children.Contains(key2))
                {
                    DBelem.children.Remove(key2);
                }
                else Console.WriteLine("key not present in the children list");
            }
        }

        //-- // This method is used to edit the metadate elements like name, description, timestamp and payload---///

        public static bool EditTextMetaData<Key, Value, Data>(this DBEngine<Key, Value> db, Key key,
            string newName, string newdescription, Data newData)
        {
            Value Val3;
            db.getValue(key, out Val3);
            DBElement<Key, Data> DBelem = Val3 as DBElement<Key, Data>;

            DBelem.name = newName;
            DBelem.descr = newdescription;
            DBelem.timeStamp = DateTime.Now;
            DBelem.payload = newData;

            return true;
        }
    }

#if (Test_editItem)             // <--------// Test stub for the edit item file  -----////////

    class Test_editItem
    {
        public static void Main()
        {

            "Testing EditItems Package".title('=');
            Console.WriteLine();

            // ----/// Initialize db elements -----//////////////////-------------////
            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            DBElement<int, string> hp1 = new DBElement<int, string>();
            DBElement<int, string> hp2 = new DBElement<int, string>();
            DBElement<int, string> hp3 = new DBElement<int, string>();

            hp1.name = "Harry Potter";
            hp1.descr = "The boy who lived";
            hp1.payload = "Defeated Lord voldemort";
            hp1.timeStamp = DateTime.Now;
            hp1.children.AddRange(new List<int> { 10, 11, 12 });
            db.insert(1, hp1);

            hp2.name = " Ron Weasley";
            hp2.descr = "Potters friend";
            hp2.payload = "the keeper in quiddich";
            hp2.timeStamp = DateTime.Now;
            hp2.children.AddRange(new List<int> { 13, 14, 15 });
            db.insert(2, hp2);

            hp3.name = "Hermione Granger";
            hp3.descr = "harry's other friend";
            hp3.payload = "Has extensive knowledge in all of magic world \n";
            hp3.timeStamp = DateTime.Now;
            hp3.children.AddRange(new List<int> { 16, 17, 18 });
            db.showDB();
            Console.WriteLine("===================================");

            db.insert(3, hp3);
            Console.WriteLine("-----------------------Element is added---------------------");
            db.showDB();

            Console.WriteLine("-----------------------Element is deleted---------------------");
            db.delete(2);
            db.showDB();

            Console.WriteLine("-----------------------Relation is added---------------------");
            db.AddRelation<int, DBElement<int, string>, string>(1, 112);
            db.showDB();

            Console.WriteLine("-----------------------Relation is removed--------------------");
            db.RemoveRelation<int, DBElement<int, string>, string>(1, 2);
            db.showDB();
            Console.WriteLine("-----------------------metadata is edited--------------------");
            db.EditTextMetaData<int, DBElement<int, string>, string>(3, "Her-my-nee", "Super intellignet", "Mudblood");
            db.showDB();
            Console.WriteLine("\n");
            Console.WriteLine("==================================================================================");
            /////////////////////////////////-------------------------------/////////////////////
            ////////--------Test for another type of database--------///////////////

            DBEngine<string, DBElement<string, List<string>>> SecondDB = new DBEngine<string, DBElement<string, List<string>>>();
            DBElement<string, List<string>> hp4 = new DBElement<string, List<string>>();
            DBElement<string, List<string>> hp5 = new DBElement<string, List<string>>();

            Console.WriteLine("==================================================================================");
            Console.WriteLine("-----------------this is for list of strings------------------------");
            hp4.name = "Albus Dumbledore";
            hp4.descr = "Headmaster of hogwarts";
            hp4.timeStamp = DateTime.Now;
            hp4.children.AddRange(new List<string> { "elder wand", "legend", "Voldemorts professor" });
            hp4.payload = new List<string> { "pointyhat", "tall", "moonglasses" };
            SecondDB.showEnumerableDB();
            SecondDB.insert("four", hp4);
            Console.WriteLine("==================================================================================");

            hp5.name = "Lord Voldemort";
            hp5.descr = "the Villian";
            hp5.timeStamp = DateTime.Now;
            hp5.children.AddRange(new List<string> { "seeks elder wand", "immortal", "Dumbeldore's student" });
            hp5.payload = new List<string> { "no noset", "horcrux", "nagini" };
            SecondDB.insert("five", hp5);

            Console.WriteLine("-----------------------element is added---------------------");
            SecondDB.showEnumerableDB();

            Console.WriteLine("-----------------------element is deleted--------------------");
            SecondDB.delete("five");
            SecondDB.showEnumerableDB();

            Console.WriteLine("-----------------------element is added again---------------------");

            hp5.name = "Lord Voldemort";
            hp5.descr = "the Villian";
            hp5.timeStamp = DateTime.Now;
            hp5.children.AddRange(new List<string> { "seeks elder wand", "immortal", "Dumbeldore's student" });
            hp5.payload = new List<string> { "no noset", "horcrux", "nagini" };
            SecondDB.insert("five", hp5);
            SecondDB.showEnumerableDB();

            Console.WriteLine("==================================================================================");

            ////////--------------------------/////////////////////-----------------------//////////////////////
            Console.WriteLine("-----------------------Relation is added---------------------");
            SecondDB.AddRelation<string, DBElement<string, List<string>>, List<string>>("four", "five");
            SecondDB.showEnumerableDB();


            ////////////////////------------------------------////////////////////////////
            Console.WriteLine("-----------------------Relation is removed---------------------");
            SecondDB.RemoveRelation<string, DBElement<string, List<string>>, List<string>>("five", "immortal");
            SecondDB.showEnumerableDB();


            /////////-------------------////////////////////////////////
            Console.WriteLine("----------------------medtadata is edited---------------------");
            SecondDB.EditTextMetaData<string, DBElement<string, List<string>>, List<string>>
                ("four", "Albus Severus dore", " had a brother and sister",
                new List<string> { "light", "can deflect avedakedavra" });

            SecondDB.showEnumerableDB();
        }
    }

#endif
}
//********///////////////////=======================////////--------////////======================///////----/////////////////////
