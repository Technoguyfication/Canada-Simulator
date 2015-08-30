
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canada_Simulator
{
    class GameTools
    {
        // Sleeping
        // Sleep
            public static void Sleep()
            {
                Tools.Log("Sleep attempt");
                if (Variables.plyCanSafelySleep)
                {
                    Variables.plyEnergy = 100;
                    Tools.Clear();
                    Tools.Log("Player succesfully slept");
                    Tools.Print("You have rested and restored all your energy.");
                }
                else
                {
                    Tools.Log("Player cannot safely sleep");
                    Tools.Print("You are not currently in a safe location to sleep.", false);
                    Tools.Print("You can still sleep, but you may may not wake up in the same condition you were.");
                    Tools.Print("Sleep anyways? (Y/N)", false);
                    Tools.GetKey();
                    while (true)
                    {
                        if (Variables.PressedKey == 'y')
                        {
                            Random random = new Random();
                            if (random.Next(100) < 40)
                            {
                                int EnergyTaken = random.Next(25);
                                TakeHealth(EnergyTaken, "because you were attacked in the night");
                                Variables.plyEnergy = 100;
                                Tools.Clear();
                                Tools.Log("Player slept, lost " + EnergyTaken.ToString() + " health.");
                                break;
                            }
                        }
                        else if (Variables.PressedKey == 'n')
                        {
                            Tools.Clear();
                            Tools.Log("Sleep aborted");
                            break;
                        }
                        else break;
                    }
                }
            }

        // Losing energy
        // TakeEnergy
            public static void TakeEnergy(int taken, string reason)
            {
                Variables.plyEnergy = Variables.plyEnergy - taken;
                Tools.Clear();
                Tools.Log("Taking " + taken + " energy from player. Reason: " + reason);
                Tools.Print("You lost " + taken.ToString() + " energy " + reason + '.');
            }

        // Losing Health
        // TakeHealth
            public static void TakeHealth(int taken, string reason)
            {
                Variables.plyHealth = Variables.plyHealth - taken;
                Tools.Clear();
                Tools.Log("Taking " + taken + " health from player. Reason: " + reason);
                Tools.Print("You lost " + taken.ToString() + " health " + reason);
            }

        // Death
        // Dead
            public static void Dead(bool energy = false)
            {
                if (!energy)
                {
                    Tools.Print("You have fallen and shall never again know the succulent flavor of maple syrup.");
                }
                else
                {
                    Tools.Print("You ran out of energy and died. Great job.");
                }
                    Tools.Pause(false, "Press any key to reload from your last save");
                SaveSystem.LoadGame();
            }

        // Update check. This runs every time something happens, usually when running  Stats();
        // Check
            public static void Check()
            {
                // Health check
                if (!(Variables.plyHealth > 0)) { Tools.Clear(false); Dead(); }

                // Energy check
                if (!(Variables.plyEnergy > 0)) { Tools.Clear(false); Dead(true); }
            }

        // Buying Items
        // Buy
            public static void Buy(int price, int itemid, int data, bool special = false)
            {
                if (CanBuy(price))
                {
                    Tools.Log("Buying a " + ItemIDName(itemid) + ". ID:" + itemid);
                    object item = ItemID(itemid);
                    if (!special)
                    {
                        TakeMoney(price);
                    }
                    else
                    {
                        TakeSyrup(price);
                    }
                    item = data;
                    Tools.Clear();
                    Tools.Print("Bought " + ItemIDName(itemid) + " for $" + price + '.');
                }
                else
                {
                    Tools.Clear();
                    Tools.Print("You do not have enough money to buy that!", true, ConsoleColor.Red);
                }
            }


        // A huge freaking mission selector
        // MissionSelect
            public static void MissionSelect(int id)
            {
                if (id == 1)
                {}  
                else
                    Tools.Error("Undefined");
            }

        // Money giving
        // GiveMoney
            public static void GiveMoney(int amount)
            {
                Tools.Log("Giving the player " + amount.ToString() + " dollars.");
                Variables.plyDollars = Variables.plyDollars + amount;
            }

        // Exp giving
        // GiveExp
            public static void GiveExp(int amount)
            {
                Tools.Log("Giving the player " + amount.ToString() + " exp.");
                Variables.plyExp = Variables.plyExp + amount;
            }

        // Syrup giving
        // GiveSyrup
            public static void GiveSyrup(int amount)
            {
                Tools.Log("Giving the player " + amount.ToString() + " syrup.");
                plySyrup = plySyrup + amount;
            }

        // Syrup taking
        // TakeSyrup
            public static void TakeSyrup(int amount)
            {
                Tools.Log("Taking " + amount.ToString() + " syrup from the player");
                plySyrup = plySyrup - amount;
            }

        // Money taking
        // TakeMoney
            public static void TakeMoney(int amount) // TAKE MY MONEY
            {
                Tools.Log("Taking " + amount.ToString() + " money from player");
                Variables.plyDollars = Variables.plyDollars - amount;
            }

        #endregion

        #region Method Variables

            // Now for the method variables
            /**/
        // New line string
        // NewLineStrng

            public static string NewLineStrng()
            {
                return Environment.NewLine;
            }
        
        // Item ID
        // ItemID
            public static object ItemID(int itemid)
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
            public static string MissionName(int id)
            {
                if (id == 1)
                {
                    return "Introduction";
                }
                else
                    return "Undefined";
            }

        // Item ID String
        // ItemIDName
            public static string ItemIDName(int itemid)
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
            public static bool CanBuy(int price)
            {
                // If the player has equal or more money return true, otherwise return false
                if (Variables.plyDollars >= price) return true; else return false;
            }


            // Level calculator
            // LvlCalc
            public static int LvlCalc()
            {
                int lvl;
                // Increment levels at 50, I.E. 50, 100, 150, 200, etc. exp.
                lvl = ((25 + ((int) Math.Sqrt(25 * 25 - 4 * 25 * (-Variables.plyExp)))) / (2 * 25));
                // Return the level
                return lvl;
            }
    }
}
