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
        static int[,] board = new int[3, 3]; //maintains 0,1: 0 = not used block, 1 = used block
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
                TrackPlayers.Add(tokenId); //player track 

            if (TrackPlayers.Count == 3)
                throw new Exception("3 users cant play!");


            int row = int.Parse(boxId[0].ToString());
            int column = int.Parse(boxId[1].ToString());

            if (player1Playing == true && tokenId == TrackPlayers[0])
                throw new Exception("Same user cant play again!");

            if (tokenId == TrackPlayers[0])//player1, access BlockedBoxByPlayer1 list
                player1Playing = true;
            else
                player1Playing = false;

            if (player1Playing)
            {
                if (BlockedBoxByPlayer1.Contains(boxId))
                    throw new Exception("Block of board blocked!");
                else
                    BlockedBoxByPlayer1.Add(boxId);
            }
            else
            {
                if (BlockedBoxByPlayer2.Contains(boxId))
                    throw new Exception("Block of board blocked!");
                else
                    BlockedBoxByPlayer2.Add(boxId);
            }

            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board.Length; j++)
                {
                    if (i == row && j == column)
                    {
                        board[i, j] = 1;
                        if (player1Playing)
                        {
                            winnerId = CheckWinner(ref BlockedBoxByPlayer1, 1);
                            if (winnerId != 999)
                                return "Player 1 Wins";
                        }
                        else
                        {
                            winnerId = CheckWinner(ref BlockedBoxByPlayer2, 2);
                            if (winnerId != 999)
                                return "Player 2 Wins";
                        }
                        break;
                    }
                }
            }
            return "";
        }


        public static int CheckWinner(ref List<string> BlockedBox, int PlayerId)
        {
            int currentPlayer = TrackPlayers[TrackPlayers.Count - 1];
            if (BlockedBox.Contains("00") && BlockedBox.Contains("01") && BlockedBox.Contains("02")) //horizontal row 1 
                return PlayerId;
            else if (BlockedBox.Contains("10") && BlockedBox.Contains("11") && BlockedBox.Contains("12")) //horizontal row 2 
                return PlayerId;
            else if (BlockedBox.Contains("20") && BlockedBox.Contains("21") && BlockedBox.Contains("22")) //horizontal row 3 
                return PlayerId;
            else if (BlockedBox.Contains("00") && BlockedBox.Contains("10") && BlockedBox.Contains("20")) //vertical row 1 
                return PlayerId;
            else if (BlockedBox.Contains("01") && BlockedBox.Contains("11") && BlockedBox.Contains("21")) //vertical row 2 
                return PlayerId;
            else if (BlockedBox.Contains("02") && BlockedBox.Contains("12") && BlockedBox.Contains("22")) //vertical row 3 
                return PlayerId;
            else if (BlockedBox.Contains("00") && BlockedBox.Contains("11") && BlockedBox.Contains("22")) //diagonal 1 
                return PlayerId;
            else if (BlockedBox.Contains("02") && BlockedBox.Contains("11") && BlockedBox.Contains("20")) //diagonal 1 
                return PlayerId;

            return 999;
        }



        [HttpGet]
        [Route("CheckStatus")]
        public string CheckStatus()
        {
            int winner = 0;
            winner = CheckWinner(ref BlockedBoxByPlayer1, 1);
            if (winner == 1)
                return "Winner - 1"; //player1 - Winner
            else
            {
                winner = CheckWinner(ref BlockedBoxByPlayer2, 2); //player2 - Winner
                if (winner == 2)
                    return "Winner - 2";
                if (BlockedBoxByPlayer1.Count + BlockedBoxByPlayer2.Count == 9)
                    return "Draw";
                //draw 
                else
                    return "Progress";
            }
        }
    }
}
