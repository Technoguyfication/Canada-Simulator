using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canada_Simulator
{
    class Tools
    {
        // An easier system for console.writeline that can be better manipulated if need-be
        // Print
        public static void Print(string text, bool newline = true, ConsoleColor color = ConsoleColor.White, ConsoleColor bgcolor = ConsoleColor.Black)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = bgcolor;
            if (!newline) { Console.WriteLine(text); } else { Console.WriteLine(text + Environment.NewLine); }
            Console.ResetColor();
        }

        // An easier system for console.write
        // PrintSameLine
        public static void PrintSameLine(string text, ConsoleColor color = ConsoleColor.White, ConsoleColor bgcolor = ConsoleColor.Black)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = bgcolor;
            Console.Write(text);
            Console.ResetColor();
        }

        // A newline inserter
        // NewLine
        public static void NewLine()
        {
            Console.WriteLine();
        }

        // A "pause" function for when we need the program to pause and allow some form of input to proceed
        // Pause
        public static void Pause(bool linebuffer = true, string message = "Press any key to continue.", bool log = true)
        {
            if (log) Log("Pausing Application");
            if (linebuffer) { Print(Environment.NewLine + message); } else { Print(message); }
            Console.ReadKey();
        }

        // A "title" function for simple title changing.
        // Title
        public static void Title(string a)
        {
            Console.Title = ("Canada Simulator V" + Assembly.GetExecutingAssembly().GetName().Version + " - " + a);
            Log("Changed title to " + Console.Title);
        }

        // A logging system
        // Log
        public static void Log(string text, string type = "Info")
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
                Error("We ran into an error with the logging system.\nDid you move the EXE out of it's folder?");
                Error("Error Details:", true);
                Error(e.Message, true);
            }
        }

        // A screen clearer
        // Clear
        public static void Clear(bool stats = true, bool check = true)
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
        public static void Stats(bool check)
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
        public static void Exit()
        {
            Print("Exiting...", true, ConsoleColor.Red);
            Log("Closing Program...");
            Environment.Exit(0);
        }

        // A red error generator
        // Error
        public static void Error(string error, bool keepguid = false)
        {
            if (!keepguid) NewGuid();
            Print("ERROR ID " + guid + ": " + error, true, ConsoleColor.Red);
            if (!keepguid) Pause(true, "Press any key to dismiss.", false);
        }

        // Refresh the string guid
        // NewGuid
        public static void NewGuid()
        {
            guid = (Guid.NewGuid().ToString());
            Log("Generating new GUID: " + guid);
        }

        // A simplified and as usual more maniputable method for Console.ReadKey()
        // GetKey
        public static void GetKey()
        {
            PressedKey = Console.ReadKey(true).KeyChar.ToString().ToLower()[0];
            Log("Pressed key: " + PressedKey.ToString());
        }

        // A way to capture text from the user
        // GetInput
        public static void GetInput()
        {
            EnteredText = Console.ReadLine().ToString();
            Log("Player input: " + EnteredText);
        }

        // A game closer
        // GameClose
        public static void GameClose()
        {
            Clear();
            Print("Would you like to (S)ave and Exit, (E)xit W/O saving, or Go (B)ack?");
            GetKey();
            if (PressedKey == 's') { SaveGame(); Exit(); } else if (PressedKey == 'e') { Exit(); } else if (PressedKey == 'b') { } else GameClose();
            Clear();
        }

        // Wait method
        // Wait
        public static void Wait(int seconds)
        {
            Log("Waiting for " + seconds + " seconds.");
            System.Threading.Thread.Sleep(seconds * 1000);
        }

        // A failsafe in case the program somehow terminates all open methods
        // FailSafe
        public static void FailSafe()
        {
            Clear();
            Log("The program went back to the static Main() method, closing program.", "Fatal");
            SaveSystem.SaveGame(true);
            Print("The program has encountered a fatal error and needs to close.\nPlease refer to the log for information on why this happened.");
            Print("We have saved a copy of your savegame to \"Data\\Gamesave_RECOVERY.xml\"\nYou should not lose any progress.\nPlease use caution when restoring from a crashed save.");
            Pause(true, "Press any key to exit.");
        }
    }
}
