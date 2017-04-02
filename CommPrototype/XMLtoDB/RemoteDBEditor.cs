/////////////////////////////////////////////////////////////////////
// RemoteDBEditor.cs - Write CLient for  Project #4                //
// Ver 1.1                                                        //
// Application: Demonstration for CSE681-SMA, Project#4            //
// Language:    C#, ver 6.0, Visual Studio 2015                    //
// Platform:      Dell Inspiron 15, Core-i5, Windows 10            //
// Author : Chiranth Bangalore Sathyaprakash                       //
//  SUID: 258380492  , csathyap@syr.edu, 315-751-1129              //
// Original Author: Jim Fawcett, CST 4-187, Syracuse University    //
//                   (315) 443-3948, jfawcett@twcny.rr.com         //
/////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package does all the write client operations in the package
 */
/*
/* Public Interface - 
 * -------------------------
 * InsertDBElement() - Inserts a dbelement into database
 *  DeleteKey() - deletes the db element of that key
 *  EditTextMetadata() - edits the metadata of db element
 *  getValue() - gets the value of that key
 *  getChildren() - gets the children list of that key
 *  KeyPatternMatching() - returns the keys that match the pattern
 *  persist() - saves the XML 
 *  load() - loads the XML from saved location
/*
 *
 * Maintenance History:
 * -------------------

 *  ver 1.1-Nov 22
 *  -added write client function
 * ver 1.0- Nov 18
 * - added read client functions 
 * Required Files - 
 * ------------------------------
 * ICommService.cs, Utilities.cs,Receiver.cs
 * PersistDB.cs, P2lib.cs
 */


using Project4Starter.Project4Starter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace Project4Starter
{
    //----------------------------------------------------------------------------------------------------//
    public class RemoteDBEditor
    {
        //----< Inserts a dbelement into database---------------------------------------->
        public XElement InsertDBElement(XElement xe,DBEngine<int,DBElement<int,string>> db)
        {  
            try {
                Console.WriteLine("================================");
                Console.WriteLine("<-------Showing Insert--------->");
                    DBElement<int, string> dbelem = new DBElement<int, string>();
                    dbelem.name = xe.Element("name").Value;              //Insert DB elements with recieved message values 
                    dbelem.descr = xe.Element("descr").Value;
                    dbelem.payload = xe.Element("payload").Value;
                    
                    List<int> childrenList = new List<int>();
                    XElement cl = xe.Element("children");
                    foreach (var c in cl.Elements("cKey")) childrenList.Add(Int32.Parse(c.Value));
                    dbelem.children = childrenList;
                     bool result =  db.insert(Int32.Parse((xe.Element("Key").Value)), dbelem);
                     db.showDB();
                Console.Write("\n===========================================================\n");
           
                //<----- True Xelement------------>
                if (result == true)
                {
                    XElement Success = new XElement("Result", "Success!, It was inserted");
                    return Success;
                }
                else
                {
                    XElement Failure = new XElement("Result", "Failure!,No Insertion");
                    return Failure;
                }
               
            }
            catch(NullReferenceException nre)
            {
                XElement Failure = new XElement("Failure!",nre.ToString());    
                return Failure;
            }
          }
        //----------------------------------------------------------------------------------------------------//
        //----< deletes the db element of that key------------------------------->
        public XElement DeleteKey(XElement xe, DBEngine<int, DBElement<int, string>> db)
        {
            Console.WriteLine("================================");
            Console.WriteLine("<------Deleting Key--------->");
            bool result = db.delete(Int32.Parse((xe.Element("Key").Value)));

         Console.WriteLine("====Bay should be deleted============");
         db.showDB();
            Console.Write("\n===========================================================\n");

            if (result == true)
            {
                XElement Success = new XElement("Result", "Success!, Key was Deleted");
                return Success;
            }
            else
            {
                XElement Failure = new XElement("Result", "Failed to delete");
                return Failure;
            }
          
        }
        //----------------------------------------------------------------------------------------------------//
        //----< edits the metadata of db element------------------------------>
        public XElement EditTextMetadata(XElement xe, DBEngine<int, DBElement<int, string>> db)
        {

            Console.WriteLine("<========================================================>");
            Console.WriteLine("<------Editing data,james(key 5) will be edited--------->");
   
            bool result =  db.EditTextMetaData<int, DBElement<int, string>, string>(Int32.Parse((xe.Element("Key").Value)),
                "Cameron","Director 10", "Aliens,Avatar");
            db.showDB();
            Console.Write("\n===========================================================\n");

            if (result == true)
            {
                XElement Success = new XElement("Result", "Success!, Element was edited");
                return Success;
            }
            else
            {
                XElement Failure = new XElement("Result", "Failed to edit");
                return Failure;
            }
        }
        //----------------------------------------------------------------------------------------------------//
        //----< gets the value of that key------------------------------->
        public XElement getValue(XElement xe, DBEngine<int, DBElement<int, string>> db,QueryEngine QE)
        {
            Console.WriteLine("<===========================================================>");
            Console.WriteLine("<-----------------------Return the value of the Key--------->");

             DBElement<int, string> dbElemkey = new DBElement<int, string>();
              dbElemkey =  QE.queryValue<int, DBElement<int, string>, string>(db, 1);
              dbElemkey.showElement();
            Console.Write("\n===========================================================\n");
            if (QE.Equals(null))
            {
                XElement Failure = new XElement("Result", "Failed to get Key");
                return Failure;
            }
            else
            {
                XElement Success = new XElement("Result", "Success!,Value of Key was obtained");
                return Success;
            }
        }
        //----< gets the children list of that key------------------------------>
        public XElement getChildren(XElement xe, DBEngine<int, DBElement<int, string>> db, QueryEngine QE)
        {
            Console.WriteLine("<==========================================>");
            Console.WriteLine("<------Return the children of the Key--------->");
           List<int> ChildList = new List<int>();
             ChildList =  QE.getChildren<int, DBElement<int, string>, string>(db, 2);
            Console.WriteLine("<------List of Keys are-------------------------->");
              foreach (int i in ChildList) Console.Write("Key {0}\t", i);
            Console.Write("\n===========================================================\n");
            if (QE.Equals(null))
            {
                XElement Failure = new XElement("Result", "Failed to get List");
                return Failure;
             }
            else
            {
                XElement Success = new XElement("Result", "Success!,Children list was obtained");
                return Success;
            }
        }
        //----< returns the keys that match the pattern------------------------------>
        public XElement KeyPatternMatching(XElement xe, DBEngine<int, DBElement<int, string>> db, QueryEngine QE)
        {
            Console.WriteLine("<==========================================>");
            Console.WriteLine("<------Return the matched Keys--------->");

           List<int> newKeyList = new List<int>();
             QE.KeyPattern<int, DBElement<int, string>, string, int>(db, 1);
            foreach (int p in newKeyList) Console.WriteLine("Matched pattern found in Key {0}", p);
            Console.Write("\n===========================================================\n");

            if (QE.Equals(null))
            {
                XElement Failure = new XElement("Result", "Failed to get List");
                return Failure;
            }
            else
            {
                XElement Success = new XElement("Result", "Success!,Matching Keys were found");
                return Success;
            }
         }
        //<--------------------------------------------------------------------------------------->
        //----<  saves the XML ---------------------------->
        public XElement persist(DBEngine<int, DBElement<int, string>> db, PersistEngine pe)
        {
            pe.persistDB<int,DBElement<int,string>,string>(db);
            Console.Write("\n===========================================================\n");
            XElement Success = new XElement("Result", "Success!,FIle was saved!");
            return Success;
          
        }
        //<------------------- loads the XML from saved location------------------------------------->
        public XElement load(DBEngine<int, DBElement<int, string>> db, PersistEngine pe)
        {
            Console.WriteLine("<--------------File will be loaded here--------------------------------->");
            pe.loadDB<int, DBElement<int, string>, string>(db,"./../../../../SavedXMLFiles/new_file.xml");
            db.showDB();
            Console.Write("\n===========================================================\n");
            XElement Success = new XElement("Result", "Success!,File was loaded");
            return Success;

        }
//<------------------------------------------------------------------------------------------------------->
#if (TEST_REMOTEDBEDITOR)            //Test stub 


        static void Main(string[] args)
        {       
            "Testing DBEngine Package".title('=');
            Console.WriteLine();
            XElement xe = new XElement("test");
            DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
            Console.WriteLine("================================");
            Console.WriteLine("<-------Showing Insert--------->");
            DBElement<int, string> dbelem = new DBElement<int, string>();
            dbelem.name = xe.Element("name").Value;              //Insert DB elements with recieved message values 
            dbelem.descr = xe.Element("descr").Value;
            dbelem.payload = xe.Element("payload").Value;

            List<int> childrenList = new List<int>();
            XElement cl = xe.Element("children");
            foreach (var c in cl.Elements("cKey")) childrenList.Add(Int32.Parse(c.Value));
            dbelem.children = childrenList;
            bool result = db.insert(Int32.Parse((xe.Element("Key").Value)), dbelem);
            db.showDB();
            Console.WriteLine("============================================================");
           
        }

#endif
    }

}
