using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TicTacToeAssignment.Authorization;

namespace TicTacToeAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GameController : ControllerBase
    {
        /// <summary>
        /// Valid Board IDs: 00, 01, 02; 10, 11, 12; 20, 21, 22
        /// </summary>
    
        static List<int> TrackPlayers = new List<int>(); //tracks users playing 
        static bool player1Playing = false;
        static List<string> BlockedBoxByPlayer1 = new List<string>(); //TrackPlayer[0] contains player1 
        static int winnerId;
        static List<string> BlockedBoxByPlayer2 = new List<string>(); //TrackPlayer[1] contains player2

        [HttpGet]
        [Exception]
        [Authorize]
        [Log]
        public string MakeAMove([FromHeader] string boxId, [FromHeader] int tokenId)
        {
            if (boxId != "00" && boxId != "01" && boxId != "02" && boxId != "10" && boxId != "11" && boxId != "12" && boxId != "20" && boxId != "21" && boxId != "22")
                throw new Exception("Invalid board box Id");

            if (!TrackPlayers.Contains(tokenId))
            {
                TrackPlayers.Add(tokenId); //player track 
            }
            if (TrackPlayers.Count == 3)
            {
                TrackPlayers.Remove(TrackPlayers[TrackPlayers.Count - 1]);
                throw new Exception("3 users cant play!");
            }
            int row = int.Parse(boxId[0].ToString());
            int column = int.Parse(boxId[1].ToString());

            if (player1Playing == true && tokenId == TrackPlayers[0])
                throw new Exception("Same user cant play again!");

            if (tokenId == TrackPlayers[0])//player1, access BlockedBoxByPlayer1 list
                player1Playing = true;
            else
                player1Playing = false;

            if (BlockedBoxByPlayer1.Contains(boxId) || BlockedBoxByPlayer2.Contains(boxId))
                throw new Exception("Block of board blocked!");


            if (player1Playing)
                BlockedBoxByPlayer1.Add(boxId);
            else
                BlockedBoxByPlayer2.Add(boxId);

            if (BlockedBoxByPlayer1.Count + BlockedBoxByPlayer2.Count == 9)
            {
                TrackPlayers.Clear();
                BlockedBoxByPlayer1.Clear();
                BlockedBoxByPlayer2.Clear();
                return "Draw...You can start new game!";
            }//draw 

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == row && j == column)
                    { 
                        if (player1Playing)
                        {
                            winnerId = CheckWinner(ref BlockedBoxByPlayer1, 1);
                            if (winnerId != 999)
                                return "Player 1 Wins...You can start new game!";
                        }
                        else
                        {
                            winnerId = CheckWinner(ref BlockedBoxByPlayer2, 2);
                            if (winnerId != 999)
                                return "Player 2 Wins...You can start new game!";
                        }
                        break;
                    }
                }
            }
            return "In Progress";
        }


        public static int CheckWinner(ref List<string> BlockedBox, int PlayerId)
        {
            int currentPlayer = TrackPlayers[TrackPlayers.Count - 1];
            if (BlockedBox.Contains("00") && BlockedBox.Contains("01") && BlockedBox.Contains("02")) //horizontal row 1 
            {
                TrackPlayers.Clear();
                BlockedBoxByPlayer1.Clear();
                BlockedBoxByPlayer2.Clear();
                return PlayerId;
            }
            else if (BlockedBox.Contains("10") && BlockedBox.Contains("11") && BlockedBox.Contains("12")) //horizontal row 2 
            {
                TrackPlayers.Clear();
                BlockedBoxByPlayer1.Clear();
                BlockedBoxByPlayer2.Clear();
                return PlayerId;
            }
            else if (BlockedBox.Contains("20") && BlockedBox.Contains("21") && BlockedBox.Contains("22")) //horizontal row 3 
            {
                TrackPlayers.Clear();
                BlockedBoxByPlayer1.Clear();
                BlockedBoxByPlayer2.Clear();
                return PlayerId;
            }
            else if (BlockedBox.Contains("00") && BlockedBox.Contains("10") && BlockedBox.Contains("20")) //vertical row 1 
            {
                TrackPlayers.Clear();
                BlockedBoxByPlayer1.Clear();
                BlockedBoxByPlayer2.Clear();
                return PlayerId;
            }
            else if (BlockedBox.Contains("01") && BlockedBox.Contains("11") && BlockedBox.Contains("21")) //vertical row 2 
            {
                TrackPlayers.Clear();
                BlockedBoxByPlayer1.Clear();
                BlockedBoxByPlayer2.Clear();
                return PlayerId;
            }
            else if (BlockedBox.Contains("02") && BlockedBox.Contains("12") && BlockedBox.Contains("22")) //vertical row 3 
            {
                TrackPlayers.Clear();
                BlockedBoxByPlayer1.Clear();
                BlockedBoxByPlayer2.Clear();
                return PlayerId;
            }
            else if (BlockedBox.Contains("00") && BlockedBox.Contains("11") && BlockedBox.Contains("22")) //diagonal 1 
            {
                TrackPlayers.Clear();
                BlockedBoxByPlayer1.Clear();
                BlockedBoxByPlayer2.Clear();
                return PlayerId;
            }
            else if (BlockedBox.Contains("02") && BlockedBox.Contains("11") && BlockedBox.Contains("20")) //diagonal 1 
            {
                TrackPlayers.Clear();
                BlockedBoxByPlayer1.Clear();
                BlockedBoxByPlayer2.Clear();
                return PlayerId;
            }
            return 999;
        }



        [HttpGet]
        [Route("CheckStatus")]
        public string CheckStatus()
        {
            int winner = 0;
            winner = CheckWinner(ref BlockedBoxByPlayer1, 1);
            if (winner == 1)
            {
                TrackPlayers.Clear();
                BlockedBoxByPlayer1.Clear();
                BlockedBoxByPlayer2.Clear();
                return "Winner - 1... You can start new game!"; //player1 - Winner
            }
            else
            {
                winner = CheckWinner(ref BlockedBoxByPlayer2, 2); //player2 - Winner
                if (winner == 2)
                {
                    TrackPlayers.Clear();
                    BlockedBoxByPlayer1.Clear();
                    BlockedBoxByPlayer2.Clear();
                    return "Winner - 2...You can start new game!";
                }
                if (BlockedBoxByPlayer1.Count + BlockedBoxByPlayer2.Count == 9)
                {
                    TrackPlayers.Clear();
                    BlockedBoxByPlayer1.Clear();
                    BlockedBoxByPlayer2.Clear();
                    return "Draw... You can start new game!";
                }//draw 
                else
                    return "Progress";
            }
        }
    }
}
