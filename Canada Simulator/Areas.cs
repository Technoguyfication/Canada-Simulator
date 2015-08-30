using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canada_Simulator
{
    /// <summary>
    /// Game areas that the player can go to.
    /// </summary>
    class Areas
    {

        public static void Enter(string name)
        {
            switch (name.ToLower())
            {
                case "init":
                    new Areas().Init();
                    break;

                case "town":
                    new Areas().Town();
                    break;

                case "town_weps":
                    new Areas().Town_Weps();
                    break;

                case "home":
                    new Areas().Home();
                    break;

                case "dev":
                    new Areas().Dev();
                    break;
            }
        }

        // Initial spawn
        // Area_Init
        private void Init(bool newgame = false)
        {
            if (newgame)
            {
                Tools.Clear(false);
                Mission.Start("intro");
                Tools.Log("Intro complete, sending player home");
            }
            Tools.Clear();
            Areas.Enter("home");
        }

        // Town area
        // Area_Town
        private void Town()
        {
            Variables.plyCanSafelySleep = false;
            while (true)
            {
                Tools.Title("Town");
                Tools.Log("Loading area town.");
                Tools.Print("Welcome to the town!", true, ConsoleColor.Magenta);
                Tools.Print("Please choose where to go:");
                Tools.PrintSameLine("(H)ome ", ConsoleColor.Yellow);
                Tools.Print("(W)eapons Dealer", false, ConsoleColor.Cyan);
                Tools.GetKey();
                if (Variables.PressedKey == 'h')
                {
                    Tools.Clear();
                    GameTools.TakeEnergy(3, "travelling home");
                    Areas.Enter("home");
                }
                else if (Variables.PressedKey == 'w')
                {
                    Tools.Clear();
                    Areas.Enter("town_weps");
                }
                else
                {
                    Tools.Clear();
                }
            }
        }

        // Town Weapons Dealer
        // Area_Town_Weps
        private void Town_Weps()
        {
            Variables.plyCanSafelySleep = false;
            while (true)
            {
                Tools.Title("Weapons Dealer");
                Tools.Log("Loading area town_weps");
                Tools.Print("Welcome to my shop, eh.", false, ConsoleColor.Magenta);
                Tools.Print("You're gonna need to protect yourself out there..\nThis stuff should get the job done..", true, ConsoleColor.Magenta);
                Tools.NewLine();
                Tools.PrintSameLine("What would you like to buy? (Press ", ConsoleColor.Green);
                Tools.PrintSameLine("\'X\'", ConsoleColor.Red);
                Tools.Print(" to cancel)", true, ConsoleColor.Green);

                /*
                 * Wooden Hockey stick: $50
                 * Metal Hockey stick: $150
                 * 
                 * Undecided:
                 * Golden hockey stick: $500
                 * Bladed hockey stick: $1500
                 */

                Tools.Print("(W)ooden Hockey Stick: $50, (M)etal Hockey Stick: $150", false, ConsoleColor.Cyan);
                Tools.GetKey();
                if (Variables.PressedKey == 'x')
                {
                    break;
                }
                else if (Variables.PressedKey == 'w')
                {
                    GameTools.Buy(50, 1, 1);
                }
                else if (Variables.PressedKey == 'm')
                {
                    GameTools.Buy(150, 1, 2);
                }
            }
            Tools.Clear();
        }

        // Home
        // Area_Home
        private void Home()
        {
            Variables.plyCanSafelySleep = true;
            while (true)
            {
                Tools.Title("Home");
                Tools.Log("Loading area home.");
                Tools.Print("Welcome Home, " + Variables.plyName + '!', true, ConsoleColor.Magenta);
                Tools.Print("You have " + Variables.plyExp.ToString() + " Experience Points.", true, ConsoleColor.DarkGreen);
                Tools.Print("Please choose an action:");
                Tools.PrintSameLine("Travel to: ", ConsoleColor.Gray);
                Tools.Print("(T)own", true, ConsoleColor.Yellow);
                Tools.Print("(C)ontinue Story: " + MissionName(plyCurrentMission), true, ConsoleColor.Green);
                Tools.Print("S(l)eep, (S)ave Game, (E)xit Game", true, ConsoleColor.Cyan);
                Tools.GetKey();
                if (PressedKey == 's')
                {
                    // Save game
                    Tools.Clear();
                    SaveGame();
                }
                else if (PressedKey == 't')
                {
                    // Go to town
                    Tools.Clear();
                    TakeEnergy(3, "travelling to the Town");
                    Area_Town();
                }
                else if (PressedKey == 'e')
                    GameClose(); /* exit */
                else if (PressedKey == 'l')
                {
                    // Sleep
                    Tools.Clear();
                    Sleep();
                }
                else if (PressedKey == 'x')
                {
                    // Developer area
                    Tools.Clear();
                    Area_Dev();
                }
                else if (PressedKey == 'c')
                {
                    // Start next mission
                    Tools.Clear();
                    MissionSelect(plyCurrentMission);
                }
                else
                    Tools.Clear(); // Reset menu if invalid choice is entered
            }
        }

        // Developer area
        private void Dev()
        {
            while (true)
            {
                Tools.Title("Dev area");
                Tools.Clear();
                break; /* --disabling this for now--
                    Tools.Print(Variables.plyExp.ToString());
                    Tools.Print("Type the number of exp you want or x to exit.");
                    GetInput();
                    if (EnteredText == "x") { Tools.Clear(); break; }
                    try
                    {
                        Variables.plyExp = int.Parse(EnteredText);
                    }
                    catch (Exception)
                    {
                        Tools.Print("You broke something. Good job.");
                    } */
            }
        }
    }
}
