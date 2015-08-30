using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canada_Simulator
{
    class Mission
    {
        public static void Start(string Mission)
        {
            switch (Mission)
            {
                case "Intro":
                    break;// stuff
            }
        }

        private void Mission_Intro()
        {
            // Mission setup
            Tools.Title("Introduction");
            Variables.plyCanSafelySleep = false;
            Tools.Clear(false);

            // Mission area
            {
                Tools.Log("Displaying introduction text");
                Tools.Print("It's 2056 and the syrup industry has fallen..");
                Tools.Wait(1);
                Tools.Print("There are no more maple trees, and society has been overthrown.");
                Tools.Wait(5);
                Tools.Clear(false);
                Tools.Print("This is the story of your survival.");
                Tools.Wait(2);
                Tools.Pause(false, "Press any key to enter tutorial.");
                Tools.Clear(false);
                // Begin tutorial

                Tools.Print("You are a lone survivor in the middle of an apocalyptic country formerly called Canada..");
                Tools.Wait(3);
                Tools.Print("You will need to fight off enemies, and trade what you have for what you need.");
                Tools.Wait(4);
                Tools.Print("People are here will still accept normal Canadian Dollars, but there is one\ncurrency more valuble than anything else...");
                Tools.Wait(3);
                Tools.PrintSameLine("Maple ", ConsoleColor.Red);
                Tools.Wait(1);
                Tools.Print("Syrup...", false, ConsoleColor.Red);
                Tools.Pause(false, "Press any key to continue tutorial.");
                // Part 2
                Tools.Print("You will need to buy some items from the stores at the Town and complete\nmissions to earn money and Exp.");
                Tools.Pause();
            }

            // Mission cleanup
            {
                // Give the player 25 exp
                GameTools.GiveExp(25);
                // Give the player 1 maple syrup
                GameTools.GiveSyrup(1);
                Tools.Clear();

                // Tell the player mission is over
                Tools.Log("Mission " + MissionName(plyCurrentMission) + "  ID: " + plyCurrentMission.ToString() + " complete!");
                Tools.Print("Mission Completed!", true, ConsoleColor.DarkYellow);
                Tools.Print("25 Exp, $200, and 1 Maple Syrup Earned");

                Tools.Pause(false, "Press any key to end mission.");
                Tools.Clear();
            }
        }
    }
}
