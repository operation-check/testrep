﻿using System;
namespace TicTacToo
{
    public class HumanPlayer : IPlayer
    {
        public Stone Stone { get; private set; }

        public HumanPlayer(Stone mark)
        {
            Stone = mark;
        }

        // 次の一手を打つ
        public int NextMove(Board board)
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line.Length != 2)
                    continue;
                var x = line[1] - '0';
                var y = line[0] - '0';
                if (1 <= x && x <= 3 && 1 <= y && y <= 3)
                {
                    var index = board.ToIndex(x, y);
                    if (board.CanPut(index))
                    {
                        board[index] = Stone;
                        return index;
                    }
                }
            }
        }
    }
}