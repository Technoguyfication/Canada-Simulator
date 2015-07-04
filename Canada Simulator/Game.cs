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
        #region Class Variables

            // Initialize class variables.

            // Player stats
            string plyName;
            int plyExp;
            int plyHealth;
            int plyEnergy;
            int plyDollars;
            int plySyrup;
            int plyArmor;
            int plyMelee;
            int plyCurrentMission;

            bool plyCanSafelySleep;

            // Essential Variables
            char PressedKey;
            string EnteredText;
            string guid;

        #endregion

        #region Menu

        // Our entry point into the applciation.
        public static void Main(string[] args)
        {
            // Make sure we don't have any command-line arguments to be taken care of
            if (args.Length > 0 && args[0] == "load") { new Program().LoadGame(); }
            if (args.Length > 0 && args[0] == "clearlog") { File.Delete("Data\\Canada Simulator.log"); }

            // Switch over to the Menu method. This will control the rest of the program.
            new Program().Menu();

            // Failsafe
            new Program().FailSafe();
        }

        // Main Menu
        public void Menu()
        {

            Log("Displaying menu...");
            Title("Main Menu");
            Clear(false);
            // Display logo
            try
            {
                using (StreamReader sr = new StreamReader("ASCII\\Logo.txt"))
                {
                    String logo = sr.ReadToEnd();
                    Print(logo);
                }
            }
            catch (Exception e)
            {
                Log("The file could not be read: " + e.Message, "Warning");
                Error("Logo error!");
            }

            // Give the player options now
            // Check we can load a game
            bool loadable = false;
            if (System.IO.File.Exists("Data\\Gamesave.xml")) { loadable = true; }
            // Display menu
            Print("Please choose from the following options:", true);
            if (loadable) { Print("(L)oad Game", false); }
            Print("(N)ew game", false);
            Print("(E)xit");
            GetKey();
            int a = 4;
            if (PressedKey == 'l') a = 1; else if (PressedKey == 'n') a = 2; else if (PressedKey == 'e') a = 3;
            switch (a)
            {
                case 1:
                    if (loadable)
                    {
                        Clear(false); LoadGame();
                    }
                    break;
                case 2:
                    while (true)
                        if (loadable)
                        {
                            {
                                Clear(false);
                                Print("Are you sure? This will clear any previous saved game.", false, ConsoleColor.Red);
                                Print("Y/N", false, ConsoleColor.Yellow);
                                GetKey();
                                if (PressedKey == 'y') { Clear(false); NewGame(); } else if (PressedKey == 'n') Menu();
                            }
                        }
                        else { Clear(false); NewGame(); }
                case 3:
                    Exit();
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

            // An easier system for console.writeline that can be better manipulated if need-be
            // Print
            public void Print(string text, bool newline = true, ConsoleColor color = ConsoleColor.White, ConsoleColor bgcolor = ConsoleColor.Black)
            {
                Console.ForegroundColor = color;
                Console.BackgroundColor = bgcolor;
                if (!newline) { Console.WriteLine(text); } else { Console.WriteLine(text + Environment.NewLine); }
                Console.ResetColor();
            }

            // An easier system for console.write
            // PrintSameLine
            public void PrintSameLine(string text, ConsoleColor color = ConsoleColor.White, ConsoleColor bgcolor = ConsoleColor.Black)
            {
                Console.ForegroundColor = color;
                Console.BackgroundColor = bgcolor;
                Console.Write(text);
                Console.ResetColor();
            }

            // A newline inserter
            // NewLine
            public void NewLine()
            {
                Console.WriteLine();
            }

            // A "pause" function for when we need the program to pause and allow some form of input to proceed
            // Pause
            public void Pause(bool linebuffer = true, string message = "Press any key to continue.", bool log = true)
            {
                if (log) Log("Pausing Application");
                if (linebuffer) { Print(Environment.NewLine + message); } else { Print(message); }
                Console.ReadKey();
            }

            // A "title" function for simple title changing.
            // Title
            public void Title(string a)
            {
                Console.Title = ("Canada Simulator V" + Assembly.GetExecutingAssembly().GetName().Version + " - " + a);
            }

            // A logging system
            // Log
            public void Log(string text, string type = "Info")
            {
                try
                {
                    using (StreamWriter log = File.AppendText("Data\\Canada Simulator.log"))
                    {
                        log.WriteLine(DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString() + " [" + type.ToUpper() + "] " + text);
                    }
                }
                catch (Exception e)
                {
                    Error("We ran into an error with the logging system." + Environment.NewLine + "Did you move the EXE out of it's folder?");
                    Error("Error Details:", true);
                    Error(e.Message, true);
                }
            }

            // Saving system. We use XML files as gamesaves.
            // SaveGame
            public void SaveGame(bool emergency = false)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                string saveFile;
                // Start an XML writer and put all our character's data into it.
                Log("Saving game...");
                if (!emergency) { saveFile = "Data\\Gamesave.xml"; } else { saveFile = "Data\\Gamesave_RECOVERY.xml"; }
                XmlWriter writer = XmlWriter.Create(saveFile, settings);
                writer.WriteStartDocument();
                writer.WriteComment("This file was generated by Canada Simulator. It is a gamesave file." + Environment.NewLine + "Edit at your own risk, it is your fault if you screw up your gamesave.");
                writer.WriteStartElement("SaveDetails");
                writer.WriteAttributeString("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                writer.WriteStartElement("Character");
                writer.WriteAttributeString("Name", plyName);
                writer.WriteAttributeString("Exp", plyExp.ToString());
                writer.WriteAttributeString("Health", plyHealth.ToString());
                writer.WriteAttributeString("Energy", plyEnergy.ToString());
                writer.WriteAttributeString("Armor", plyArmor.ToString());
                writer.WriteAttributeString("CurrentMission", plyCurrentMission.ToString());
                writer.WriteStartElement("Currency");
                writer.WriteAttributeString("Dollars", plyDollars.ToString());
                writer.WriteAttributeString("Syrup", plySyrup.ToString());
                writer.WriteStartElement("Inventory");
                writer.WriteAttributeString("Melee", plyMelee.ToString());
                writer.WriteEndElement();
                writer.WriteEndDocument();
                // Flush and close the writer when saving is complete.
                Log("Save complete!");
                writer.Flush();
                writer.Close();
                if (!emergency) Print("Game saved!", true, ConsoleColor.Green);
            }

            // Save file reading system
            // LoadGame
            public void LoadGame()
            {
                Log("Loading game...");
                try
                {
                    using (XmlReader reader = XmlReader.Create("Data\\Gamesave.xml"))
                    {
                        // Player Stats
                        reader.ReadToFollowing("Character");
                        reader.MoveToFirstAttribute();
                        plyName = reader.Value;
                        reader.MoveToNextAttribute();
                        plyExp = int.Parse(reader.Value);
                        reader.MoveToNextAttribute();
                        plyHealth = int.Parse(reader.Value);
                        reader.MoveToNextAttribute();
                        plyEnergy = int.Parse(reader.Value);
                        reader.MoveToNextAttribute();
                        plyArmor = int.Parse(reader.Value);
                        reader.MoveToNextAttribute();
                        plyCurrentMission = int.Parse(reader.Value);
                        // Currency
                        reader.ReadToFollowing("Currency");
                        reader.MoveToFirstAttribute();
                        plyDollars = int.Parse(reader.Value);
                        reader.MoveToNextAttribute();
                        plySyrup = int.Parse(reader.Value);
                        // Inventory
                        reader.ReadToFollowing("Inventory");
                        reader.MoveToFirstAttribute();
                        plyMelee = int.Parse(reader.Value);
                        Log("Loaded game \"" + plyName + "\"");
                        Log("Save file version is " + /*SaveVersion*/ "[UNKNOWN]");
                    }
                }
                catch (Exception e)
                {
                    Log("Could not load save file: " + e.Message, "Fatal");
                    Clear(false);
                    Print("Could not load save file:" + Environment.NewLine + e.Message);
                    Pause();
                    FailSafe();
                }
                Area_Init();
            }

            // A new game generator.
            // NewGame
            public void NewGame()
            {
                Log("Generating new save...");
                Clear(false, false);
                // Get what the player's name will be
                while (true)
                {
                    Print("What would you like your character's name to be?", true);
                    GetInput();
                    // Make sure it isn't too long
                    if (EnteredText.Length > 15)
                    {
                        Clear(false);
                        Print("Your character's name may not be longer than 15 characters!", false);
                        Pause();
                        Clear(false);
                    }
                    else
                        break;
                }
                Log("Generating new save with character name \"" + EnteredText + "\"");
                // Set name
                plyName = EnteredText;
                // Set default attribs
                plyExp = 0;
                plyHealth = 100;
                plyEnergy = 100;
                plyArmor = 0;
                // Set default currency
                plyDollars = 500;
                plySyrup = 15;
                // Set default inventory
                plyMelee = 0;
                // Set missions
                plyCurrentMission = 1;
                // Save the game.
                SaveGame();
                // Send us into the game.
                Area_Init();
            }

            // A screen clearer
            // Clear
            public void Clear(bool stats = true, bool check = true)
            {
                // Clear Screen
                Console.Clear();
                // Stats
                // If display stats, check whether or not we do framecheck, if not, display stats without framecheck, otherwise do both
                if (stats) if (check) Stats(true); else if (!check) Stats(false);
                // Log the screen clearing.
                Log("Cleared screen. Stats: " + stats.ToString());
            }

            // Stats displayer - ONLY FOR USE WITH Clear(); !!
            // Stats
            public void Stats(bool check)
            {
                // Check all the player stats ad run a frame check
                if (check) Check();
                // Name Label
                PrintSameLine(plyName, ConsoleColor.Green);
                // Health System and label
                PrintSameLine("  Health: ");
                if (plyHealth >= 75)
                {
                    PrintSameLine(plyHealth.ToString(), ConsoleColor.Green);
                }
                else if (plyHealth >= 50 && plyHealth < 75)
                {
                    PrintSameLine(plyHealth.ToString(), ConsoleColor.Yellow);
                }
                else if (plyHealth >= 25 && plyHealth < 50)
                {
                    PrintSameLine(plyHealth.ToString(), ConsoleColor.Red);
                }
                else if (plyHealth >= 5 && plyHealth < 25)
                {
                    PrintSameLine(plyHealth.ToString(), ConsoleColor.DarkRed);
                }
                else PrintSameLine(plyHealth.ToString(), ConsoleColor.Gray);
                // Energy system and label
                PrintSameLine("  Energy: ");
                if (plyEnergy >= 75)
                {
                    PrintSameLine(plyEnergy.ToString(), ConsoleColor.Green);
                }
                else if (plyEnergy >= 50 && plyEnergy < 75)
                {
                    PrintSameLine(plyEnergy.ToString(), ConsoleColor.Yellow);
                }
                else if (plyEnergy >= 25 && plyEnergy < 50)
                {
                    PrintSameLine(plyEnergy.ToString(), ConsoleColor.Red);
                }
                else if (plyEnergy >= 5 && plyEnergy < 25)
                {
                    PrintSameLine(plyEnergy.ToString(), ConsoleColor.DarkRed);
                }
                else PrintSameLine(plyEnergy.ToString(), ConsoleColor.Gray);
                // Armor system and label
                PrintSameLine("  Armor: ");
                if (plyArmor >= 75)
                {
                    PrintSameLine(plyArmor.ToString(), ConsoleColor.Green);
                }
                else if (plyArmor >= 50 && plyArmor < 75)
                {
                    PrintSameLine(plyArmor.ToString(), ConsoleColor.Yellow);
                }
                else if (plyArmor >= 25 && plyArmor < 50)
                {
                    PrintSameLine(plyArmor.ToString(), ConsoleColor.Red);
                }
                else if (plyArmor >= 5 && plyArmor < 25)
                {
                    PrintSameLine(plyArmor.ToString(), ConsoleColor.DarkRed);
                }
                else PrintSameLine(plyArmor.ToString(), ConsoleColor.Gray);
                // Money
                PrintSameLine("  Money: ");
                PrintSameLine(plyDollars.ToString(), ConsoleColor.Gray);
                // Level
                PrintSameLine("  Level: ");
                PrintSameLine(LvlCalc().ToString(), ConsoleColor.Gray);
                NewLine();
                NewLine();
            }

            // Exits the progam
            // Exit
            public void Exit()
            {
                Print("Exiting...", true, ConsoleColor.Red);
                Log("Closing Program...");
                Environment.Exit(0);
            }

            // A red error generator
            // Error
            public void Error(string error, bool keepguid = false)
            {
                if (!keepguid) NewGuid();
                Print("ERROR ID " + guid + ": " + error, true, ConsoleColor.Red);
                if (!keepguid) Pause(true, "Press any key to dismiss.", false);
            }

            // Refresh the string guid
            // NewGuid
            public void NewGuid()
            {
                guid = (Guid.NewGuid().ToString());
            }

            // A simplified and as usual more maniputable method for Console.ReadKey()
            // GetKey
            public void GetKey()
            {
                PressedKey = Console.ReadKey(true).KeyChar.ToString().ToLower()[0];
            }

            // A way to capture text from the user
            // GetInput
            public void GetInput()
            {
                EnteredText = Console.ReadLine().ToString();
            }

            // A game closer
            // GameClose
            public void GameClose()
            {
                Clear();
                Print("Would you like to (S)ave and Exit, (E)xit W/O saving, or Go (B)ack?");
                GetKey();
                if (PressedKey == 's') { SaveGame(); Exit(); } else if (PressedKey == 'e') { Exit(); } else if (PressedKey == 'b') { } else GameClose();
                Clear();
            }

            // A failsafe in case the program somehow terminates all open methods
            // FailSafe
            public void FailSafe()
            {
                Clear();
                Log("The program went back to the static Main() method, closing program.", "Fatal");
                SaveGame(true);
                Print("The program has encountered a fatal error and needs to close." + Environment.NewLine + "Please refer to the log for information on why this happened.");
                Print("We have saved a copy of your savegame to \"Data\\Gamesave_RECOVERY.xml\"" + Environment.NewLine + "You should not lose any progress." + Environment.NewLine + "Please use caution when restoring from a crashed save.");
                Pause(true, "Press any key to exit.");
            }

        #endregion

        #region Methods for gameplay-related stuff and such

        // Sleeping
        // Sleep
            public void Sleep()
            {
                if (plyCanSafelySleep)
                {
                    plyEnergy = 100;
                    Clear();
                    Print("You have rested and restored all your energy.");
                }
                else
                {
                    Print("You are not currently in a safe location to sleep.", false);
                    Print("You can still sleep, but you may may not wake up in the same condition you were.");
                    Print("Sleep anyways? (Y/N)", false);
                    GetKey();
                    while (true)
                    {
                        if (PressedKey == 'y')
                        {
                            Random random = new Random();
                            if (random.Next(100) < 40)
                            {
                                TakeHealth(random.Next(25), "because you were attacked in the night");
                                plyEnergy = 100;
                                Clear();
                                break;
                            }
                        }
                        else if (PressedKey == 'n')
                        {
                            Clear();
                            break;
                        }
                        else break;
                    }
                }
            }

        // Losing energy
        // TakeEnergy
            public void TakeEnergy(int taken, string reason)
            {
                plyEnergy = plyEnergy - taken;
                Clear();
                Print("You lost " + taken.ToString() + " energy " + reason + '.');
            }

        // Losing Health
        // TakeHealth
            public void TakeHealth(int taken, string reason)
            {
                plyHealth = plyHealth - taken;
                Clear();
                Print("You lost " + taken.ToString() + " health " + reason);
            }

        // Death
        // Dead
            public void Dead(bool energy = false)
            {
                if (!energy)
                {
                    Print("You have perished and shall forever rest in maple pepperoni.");
                }
                else
                {
                    Print("You ran out of energy and died. Good job.");
                }
                    Pause(false, "Press any key to reload from your last save");
                LoadGame();
            }

        // Update check. This runs every time something happens, usually when running  Stats();
        // Check
            public void Check()
            {
                // Health check
                if (!(plyHealth > 0)) { Clear(false); Dead(); }

                // Energy check
                if (!(plyEnergy > 0)) { Clear(false); Dead(true); }
            }

        // Buying Items
        // Buy
            public void Buy(int price, int itemid, int data, bool blackmarket = false)
            {
                if (CanBuy(price))
                {
                    Log("Buying a " + ItemIDName(itemid) + ". ID:" + itemid);
                    object item = ItemID(itemid);
                    if (!blackmarket)
                    {
                        plyDollars = (plyDollars - price);
                    }
                    else
                    {
                        plySyrup = (plySyrup - price);
                    }
                    item = data;
                    Clear();
                    Print("Bought " + ItemIDName(itemid) + " for $" + price + '.');
                }
                else
                {
                    Clear();
                    Print("You do not have enough money to buy that!", true, ConsoleColor.Red);
                }
            }


        // A huge freaking mission selector
        // MissionSelect
            public void MissionSelect(int id)
            {
                if (id == 1)
                    Mission_GettingStarted();
                else
                    Error("Undefined");
            }

        #endregion

        #region Method Variables

            // Now for the method variables
            /**/
        // New line string
        // NewLineStrng

            public string NewLineStrng()
            {
                return Environment.NewLine;
            }
        
        // Item ID
        // ItemID
            public object ItemID(int itemid)
            {
                // I'm too lazy to make an array so we use a really stupid if statement for an item id list.

                if (itemid == 1)
                {
                    return plyMelee;
                }
                else if (itemid == 2)
                {
                    return plyMelee;
                }

                return null;
            }

        // Mission name generator
        // MissionName
            public string MissionName(int id)
            {
                if (id == 1)
                {
                    return "Getting Started";
                }
                else
                    return "Undefined";
            }

        // Item ID String
        // ItemIDName
            public string ItemIDName(int itemid)
            {
                if (itemid == 1)
                {
                    return "Wooden Hockey Stick";
                }
                else if (itemid == 2)
                {
                    return "Metal Hockey Stick";
                }
                return null;
            }

        // Check if the player has enough cash
        // CanBuy
            public bool CanBuy(int price)
            {
                // If the player has equal or more money return true, otherwise return false
                if (plyDollars >= price) return true; else return false;
            }


            // Level calculator
            // LvlCalc
            public int LvlCalc()
            {
                int lvl;
                // Increment levels at 50, I.E. 50, 100, 150, 200, etc. exp.
                lvl = ((25 + ((int) Math.Sqrt(25 * 25 - 4 * 25 * (-plyExp)))) / (2 * 25));
                // Return the level
                return lvl;
            }

        #endregion

        #region Missions

        // Mission data

            public void missioninfo(Array type)
            {
                
            }

        // Area for the missions. Each is contained in it's own method

            public void Mission_GettingStarted()
            {
                // Mission setup
                Title("Mission: Getting Started");
                plyCanSafelySleep = false;
                string NarratorName = "Bob";
                Clear();

                // Mission area
                {
                    Print(NarratorName + ": Welcome to Canada Simulator!");
                    Print(NarratorName + ": Allow me to show you around, eh!");
                    Pause();
                }

                // Mission cleanup
                {
                    // Give the player 25 exp
                    plyExp = plyExp + 25;

                    Clear();

                    // Tell the player mission is over
                    Print("Mission Completed!", true, ConsoleColor.DarkYellow);
                    Print("25 Exp Earned");
                    Pause(false, "Press any key to end mission.");
                    Clear();
                }
            }

        #endregion

        #region Game Areas

            // Initial spawn
            // Area_Init
            public void Area_Init()
            {
                Clear();
                Area_Home();
            }

            #region Town

            // Town area
            // Area_Town
                public void Area_Town()
                {
                    plyCanSafelySleep = false;
                    while (true)
                    {
                        Title("Town");
                        Log("Loading area town.");
                        Print("Welcome to the town!", true, ConsoleColor.Magenta);
                        Print("Please choose where to go:");
                        PrintSameLine("(H)ome ", ConsoleColor.Yellow);
                        Print("(W)eapons Dealer", false, ConsoleColor.Cyan);
                        GetKey();
                        if (PressedKey == 'h') 
                        { 
                            Clear();
                            TakeEnergy(3, "travelling home");
                            Area_Home(); 
                        }
                        else if (PressedKey == 'w') 
                        {
                            Clear();
                            Area_Town_Weps(); 
                        } 
                        else 
                        {
                            Clear();
                        }
                    }
                }

            // Town Weapons Dealer
            // Area_Town_Weps
                public void Area_Town_Weps()
                {
                    plyCanSafelySleep = false;
                    while (true)
                    {
                        Title("Weapons Dealer");
                        Log("Loading area town_weps");
                        Print("Welcome to my shop, eh.", false, ConsoleColor.Magenta);
                        Print("You're gonna need to protect yourself out there.." + NewLineStrng() + "This stuff should get the job done..", true, ConsoleColor.Magenta);
                        NewLine();
                        PrintSameLine("What would you like to buy? (Press ", ConsoleColor.Green);
                        PrintSameLine("\'X\'", ConsoleColor.Red);
                        Print(" to cancel)", true, ConsoleColor.Green);

                        /*
                         * Wooden Hockey stick: $50
                         * Metal Hockey stick: $150
                         * 
                         * Undecided:
                         * Golden hockey stick: $500
                         * Bladed hockey stick: $1500
                         */

                        Print("(W)ooden Hockey Stick: $50, (M)etal Hockey Stick: $150", false, ConsoleColor.Cyan);
                        GetKey();
                        if (PressedKey == 'x')
                        {
                            break;
                        }
                        else if (PressedKey == 'w')
                        {
                            Buy(50, 1, 1);
                        }
                        else if (PressedKey == 'm')
                        {
                            Buy(150, 1, 2);
                        }
                    }
                    Clear();
                }

            #endregion

        // Home
        // Area_Home
            public void Area_Home()
            {
                plyCanSafelySleep = true;
                while (true)
                {
                    Title("Home");
                    Log("Loading area home.");
                    Print("Welcome Home, " + plyName + '!', true, ConsoleColor.Magenta);
                    Print("You have " + plyExp.ToString() + " Experience Points.", true, ConsoleColor.DarkGreen);
                    Print("Please choose an action:");
                    PrintSameLine("Travel to: ", ConsoleColor.Gray);
                    Print("(T)own", true, ConsoleColor.Yellow);
                    Print("Start Next (M)ission: " + MissionName(plyCurrentMission), true, ConsoleColor.Green);
                    Print("S(l)eep, (S)ave Game, (E)xit Game", true, ConsoleColor.Cyan);
                    GetKey();
                    if (PressedKey == 's')
                    { 
                        // Save game
                        Clear(); 
                        SaveGame();
                    } 
                    else if (PressedKey == 't')
                    {
                        // Go to town
                        Clear();
                        TakeEnergy(3, "travelling to the Town");
                        Area_Town(); 
                    }
                    else if (PressedKey == 'e')
                        GameClose(); /* exit */
                    else if (PressedKey == 'l') 
                    {
                        // Sleep
                        Clear();
                        Sleep();
                    }
                    else if (PressedKey == 'x')
                    {
                        // Developer area
                        Clear();
                        Area_Dev();
                    }
                    else if (PressedKey == 'm')
                    {
                        // Start next mission
                        Clear();
                        MissionSelect(plyCurrentMission);
                    }
                    else
                        Clear(); /* Reset menu if invalid choice is entered */
                }
            }

        // Developer area
            public void Area_Dev()
            {
                while (true)
                {
                    Title("Dev area");
                    Clear();
                    break; /* disabling this for now
                    Print(plyExp.ToString());
                    Print("Type the number of exp you want or x to exit.");
                    GetInput();
                    if (EnteredText == "x") { Clear(); break; }
                    try
                    {
                        plyExp = int.Parse(EnteredText);
                    }
                    catch (Exception)
                    {
                        Print("You broke something. Good job.");
                    } */
                }
            }

        #endregion

    }
}