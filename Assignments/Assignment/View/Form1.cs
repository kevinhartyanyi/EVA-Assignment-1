﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assignment.Model;
using Assignment.View;

namespace Assignment
{
    public partial class Form1 : Form
    {
        TableLayoutPanel table;
        GameControlModel model;
        int _mapSize;
        int shipNumber;

        BaseButton[,] elements;

        Color baseColor;
        Color shipColor;
        Color playerColor;
        Color lightBombColor;
        Color mediumBombColor;
        Color heavyBombColor;

        Position player;
        List<Elem> ships;
        List<Elem> bombs;

        #region

        int FindElem(List<Elem> cont, int id)
        {
            int re = -1;
            for (int i = 0; i < cont.Count; i++)
            {
                if (cont[i].ID == id)
                    re = i;
            }
            return re;
        }

        void OnKeyDownEvent(object sender, KeyPressEventArgs e)
        {
            if (!model.isPlaying)
                return;
            if (e.KeyChar == 'a' || e.KeyChar == 'A')
            {
                model.PlayerMove(Model.Move.Left);
            }
            else if (e.KeyChar == 'd' || e.KeyChar == 'D')
            {
                model.PlayerMove(Model.Move.Right);
            }
            else if (e.KeyChar == 'w' || e.KeyChar == 'W')
            {
                model.PlayerMove(Model.Move.Up);
            }
            else if (e.KeyChar == 's' || e.KeyChar == 'S')
            {
                model.PlayerMove(Model.Move.Down);
            }
        }

        void OnPlayerMoveEvent(object sender, PlayerMoveEvent e)
        {

            elements[player._x, player._y].BackColor = baseColor;
            player._x = e.Position._x;
            player._y = e.Position._y;
            elements[player._x, player._y].BackColor = playerColor;
        }

        void OnShipMoveEvent(object sender, ShipMoveEvent e)
        {
            int find = FindElem(ships, e.ID);
            var s = ships[find];
            elements[s.Position._x, s.Position._y].BackColor = baseColor;
            s.Position = e.Position;
            elements[s.Position._x, s.Position._y].BackColor = shipColor;
            
        }

        void OnShipCreateEvent(object sender, ShipCreateEvent e)
        {
            Elem elem = new Elem(e.Position, e.ID, Model.Type.Ship);
            ships.Add(elem);
            elements[elem.Position._x, elem.Position._y].BackColor = shipColor;
        }

        void OnBombCreateEvent(object sender, BombCreateEvent e)
        {
            Model.Type bombType = Model.Type.Nothing;
            Color bombColor = Color.Black;
            switch (e.BombType)
            {
                case Bomb_Type.Light:
                    bombType = Model.Type.LightBomb;
                    bombColor = lightBombColor;
                    break;
                case Bomb_Type.Medium:
                    bombType = Model.Type.MediumBomb;
                    bombColor = mediumBombColor;
                    break;
                case Bomb_Type.Heavy:
                    bombType = Model.Type.HeavyBomb;
                    bombColor = heavyBombColor;
                    break;
            }
            Elem elem = new Elem(e.Position, e.ID, bombType);
            bombs.Add(elem);
            elements[elem.Position._x, elem.Position._y].BackColor = bombColor;
        }

        void OnBombMoveEvent(object sender, BombMoveEvent e)
        {
            if (elements == null)
                return;
            int find = FindElem(bombs, e.ID);
            var b = bombs[find];
            elements[b.Position._x, b.Position._y].BackColor = baseColor;
            b.Position = e.Position;
            Color bombColor = Color.White;
            switch (b.Type)
            {
                case Model.Type.LightBomb:
                    bombColor = lightBombColor;
                    break;
                case Model.Type.MediumBomb:
                    bombColor = mediumBombColor;
                    break;
                case Model.Type.HeavyBomb:
                    bombColor = heavyBombColor;
                    break;

            }
            elements[b.Position._x, b.Position._y].BackColor = bombColor;
            
        }

        void OnGameOverEvent(object sender, GameOverEvent e)
        {
            Console.WriteLine("GameOver");
            AskGameStart();
        }

        void OnBombRemoveEvent(object sender, BombRemoveEvent e)
        {
            Console.WriteLine("Bomb Remove");
            int ind = FindElem(bombs, e.ID);
            elements[bombs[ind].Position._x, bombs[ind].Position._y].BackColor = baseColor;
            bombs.RemoveAt(ind);
        }


        #endregion
        delegate void NewGameCall();

        void CheckCall()
        {
            if (this.InvokeRequired)
            {
                NewGameCall d = new NewGameCall(NewGame);
                this.Invoke(d);
            }
            else
            {
                NewGame();
            }
        }
        void AskGameStart()
        {
            using (AskGameStart dial = new View.AskGameStart())
            {
                dial.ShowDialog();
                Console.WriteLine(dial.ShipNumber);
                Console.WriteLine(dial.MapSize);
                _mapSize = dial.MapSize;
                shipNumber = dial.ShipNumber;
            }
            CheckCall();
        }


        public Form1()
        {
            if (Debugger.IsAttached)
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            InitializeComponent();
            Start();
            AskGameStart();
        }

        void Start()
        {           
            model = new GameControlModel();
            model.shipMove += new EventHandler<ShipMoveEvent>(OnShipMoveEvent);
            model.bombMove += new EventHandler<BombMoveEvent>(OnBombMoveEvent);
            model.gameOver += new EventHandler<GameOverEvent>(OnGameOverEvent);
            model.bombRemove += new EventHandler<BombRemoveEvent>(OnBombRemoveEvent);
            model.shipCreate += new EventHandler<ShipCreateEvent>(OnShipCreateEvent);
            model.bombCreate += new EventHandler<BombCreateEvent>(OnBombCreateEvent);
            model.playerMove += new EventHandler<PlayerMoveEvent>(OnPlayerMoveEvent);
            player = new Position();
            ships = new List<Elem>();
            bombs = new List<Elem>();
            baseColor = Color.AliceBlue;
            shipColor = Color.Orange;
            playerColor = Color.LimeGreen;
            lightBombColor = Color.Tomato;
            mediumBombColor = Color.Firebrick;
            heavyBombColor = Color.PaleVioletRed;
            _mapSize = 3;
            shipNumber = 1;

            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(OnKeyDownEvent);
        }

        void NewGame()
        {
            elements = new BaseButton[_mapSize, _mapSize];
            this.Controls.Clear();
            table = new TableLayoutPanel();
            table.RowCount = _mapSize;
            table.ColumnCount = _mapSize;
            table.Dock = DockStyle.Fill;
            for (int i = 0; i < _mapSize; i++)
            {
                var percent = 100f / (float)_mapSize;
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percent));
                table.RowStyles.Add(new RowStyle(SizeType.Percent, percent));
            }
            this.Controls.Add(table);

            for (int i = 0; i < _mapSize; i++)
            {
                for (int j = 0; j < _mapSize; j++)
                {
                    var baseElem = new BaseButton(baseColor);
                    elements[i, j] = baseElem;
                    table.Controls.Add(baseElem, i, j);
                }
            }

            model.NewGame(_mapSize, _mapSize - 1, _mapSize - 1, shipNumber);
            player.SetPosition(_mapSize - 1, _mapSize - 1);
            elements[player._x, player._y].BackColor = playerColor;
        }
    }
}
