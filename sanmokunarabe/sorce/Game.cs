﻿using System;
using System.Collections.Generic;

namespace TicTacToo
{
    public class Game : IObservable<Board>
    {
        private Board _board;
        private IPlayer _player1;
        private IPlayer _player2;

        public Game(IPlayer player1, IPlayer player2, Board board)
        {
            _player1 = player1;
            _player2 = player2;
            _board = board;
        }

        // Game開始
        public Stone Start()
        {
            IPlayer player = _player1;
            Publish(_board);
            while (!_board.IsFin())
            {
                player.NextMove(_board);
                Publish(_board);
                player = Turn(player);
            }
            Complete();
            return _board.Judge();

        }

        // 次のプレイヤー
        private IPlayer Turn(IPlayer player)
        {
            return player == _player1 ? _player2 : _player1;
        }

        private List<IObserver<Board>> _observers = new List<IObserver<Board>>();

        // 終了を通知する
        private void Complete()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        // 状況変化を知らせるために購読者に通知する
        private void Publish(Board board)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(board);
            }
        }

        // 購読を申し込む -  observerが通知を受け取れるようになる
        public IDisposable Subscribe(IObserver<Board> observer)
        {
            _observers.Add(observer);
            return observer as IDisposable;
        }
    }
}