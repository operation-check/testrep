﻿using System;
namespace TicTacToo
{
    public interface IPlayer
    {
        Stone Stone { get; }
        int NextMove(Board board);
    }
}
