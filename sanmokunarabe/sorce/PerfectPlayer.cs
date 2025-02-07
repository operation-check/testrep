﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToo
{
    public class PerfectPlayer : IPlayer
    {

        public Stone Stone { get; private set; }

        // コンストラクタ
        public PerfectPlayer(Stone myStone)
        {
            Stone = myStone;
        }

        // 一手を打つ
        public int NextMove(Board board)
        {
            if (board.GetVacantIndexes().Count() == 9)
            {
                var (place, winner) = FirstMove(Stone, board);
                board[place] = Stone;
                return place;
            }
            else
            {
                var (place, winner) = BestMove(board, Stone);
                board[place] = Stone;
                return place;
            }
        }

        // 次のプレイヤー
        private Stone NextPlayer(Stone now)
        {
            return now == Stone.Black ? Stone.White : Stone.Black;
        }

        // 王手をみつける
        private int FindCheckmate(Board board, List<int> line, Stone myStone)
        {
            if (line.Count(x => board[x] == myStone) == 2)
            {
                var vacants = line.Where(x => board[x] == Stone.Empty);
                if (vacants.Count() == 1)
                    return vacants.First();
            }
            return -1;
        }

        // 最善手を打つ
        private (int, Stone) BestMove(Board board, Stone player, int level = 0)
        {
            var opponent = NextPlayer(player);
            if (board.IsFin())
                // ここで終わったということは、引き分けしかない。Stone.Emptyは引分けを示す
                return (-1, Stone.Empty);

            var drawix = -1;
            var loseix = -1;
            // 空いている手を一つずつ確かめる
            foreach (var ix in board.GetVacantIndexes())
            {
                // ixに石を置いてみる
                board[ix] = player;
                try
                {
                    var win = board.Judge();
                    if (win == player)
                        // 勝利したのでこれを最善手として戻る
                        return (ix, player);
                    if (win == Stone.Empty)
                    {
                        // まだ終わっていないので、さらに探索 相手も最善手を指すと仮定する
                        var (nextix, winner) = BestMove(board, opponent, level + 1);
                        if (winner == player)
                            // 相手がどこにおいても自分の勝ちなので、これを最善手として戻る
                            return (ix, winner);
                        if (winner == Stone.Empty)
                            // 相手が最善手を指して引分けだった
                            drawix = ix;
                        else
                            // 自分がixに打つと、相手の勝ち
                            loseix = ix;

                    }
                }
                finally
                {
                    // 置いた石をixから取り除く
                    board[ix] = Stone.Empty;
                }
            }
            // ここまで来たということは勝ちは無かった。
            if (drawix != -1)
                // 引分けがあったので、引分けの手を最善手として戻る。Stone.Emptyは引分けを示す
                return (drawix, Stone.Empty);
            // 負けてしまった。仕方がないので、負ける手を自分の指す手として戻る
            return (loseix, opponent);
        }

        Random _rnd = new Random();

        // 最初の一手を決める
        private (int, Stone) FirstMove(Stone player, Board board)
        {

            var indexes = new[] { board.ToIndex(1,1),
            board.ToIndex(1,3),
            board.ToIndex(3,1),
            board.ToIndex(3,3),
            board.ToIndex(2,2) };
            return (indexes[_rnd.Next(0, indexes.Count())], Stone.Empty);
        }
    }
}