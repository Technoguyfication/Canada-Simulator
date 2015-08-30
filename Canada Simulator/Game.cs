/* The MIT License (MIT)

Copyright (c) 2015 Hayden (Technoguyfication) Andreyka

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;

namespace Canada_Simulator
{
    class Program
    {
        #region Menu

            // Our entry point into the applciation.
        public static void Main(string[] args)
        {
            // Make sure we don't have any command-line arguments to be taken care of
            if (args.Length > 0 && args[0] == "load") { SaveSystem.SaveSystem.LoadGame(); }
            if (args.Length > 0 && args[0] == "clearlog") { File.Delete("Data\\Canada Simulator.log"); }

            // Switch over to the Menu method. This will control the rest of the program.
            new Program().Menu();

            // Failsafe
            Tools.FailSafe();
        }

        // Main Menu
        public void Menu()
        {

            Tools.Log("Displaying menu...");
            Tools.Title("Main Menu");
            Tools.Clear(false);
            // Display logo
            try
            {
                using (StreamReader sr = new StreamReader("ASCII\\Logo.txt"))
                {
                    String logo = sr.ReadToEnd();
                    Tools.Print(logo);
                }
            }
            catch (Exception e)
            {
                Tools.Log("The file could not be read: " + e.Message, "Warning");
                Tools.Error("Logo error!");
            }

            // Give the player options now
            // Check we can load a game
            bool loadable = false;
            if (System.IO.File.Exists("Data\\Gamesave.xml")) { loadable = true; }
            // Display menu
            Tools.Print("Please choose from the following options:", true);
            if (loadable) { Tools.Print("(L)oad Game", false); }
            Tools.Print("(N)ew game", false);
            Tools.Print("(E)xit");
            Tools.GetKey();
            int a = 4;
            if (PressedKey == 'l') a = 1; else if (PressedKey == 'n') a = 2; else if (PressedKey == 'e') a = 3;
            switch (a)
            {
                case 1:
                    if (loadable)
                    {
                        Tools.Clear(false); SaveSystem.SaveSystem.LoadGame();
                    }
                    break;
                case 2:
                    while (true)
                        if (loadable)
                        {
                            {
                                Tools.Clear(false);
                                Tools.Print("Are you sure? This will clear any previous saved game.", false, ConsoleColor.Red);
                                Tools.Print("Y/N", false, ConsoleColor.Yellow);
                                Tools.GetKey();
                                if (PressedKey == 'y') { Tools.Clear(false); SaveSystem.NewGame(); } else if (PressedKey == 'n') Menu();
                            }
                        }
                        else { Tools.Clear(false); SaveSystem.NewGame(); }
                case 3:
                    Tools.Exit();
                    break;
                case 4:
                    Menu();
                    break;
            }



        }

            #endregion

        #region Methods for basic program functions

        // Now begin all of the functions and such, these are used to make life easier.
            /**/



        #endregion

        #region Game Areas



        #endregion

    }
}
