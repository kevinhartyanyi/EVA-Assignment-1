using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        }

        void OnBombRemoveEvent(object sender, BombRemoveEvent e)
        {
            Console.WriteLine("Bomb Remove");
            int ind = FindElem(bombs, e.ID);
            elements[bombs[ind].Position._x, bombs[ind].Position._y].BackColor = baseColor;
            bombs.RemoveAt(ind);
        }


        #endregion


        public Form1()
        {
            InitializeComponent();
            Start(10);
            NewGame();
        }

        void Start(int mapSize)
        {
            table = new TableLayoutPanel();            
            model = new GameControlModel(mapSize);
            model.shipMove += new EventHandler<ShipMoveEvent>(OnShipMoveEvent);
            model.bombMove += new EventHandler<BombMoveEvent>(OnBombMoveEvent);
            model.gameOver += new EventHandler<GameOverEvent>(OnGameOverEvent);
            model.bombRemove += new EventHandler<BombRemoveEvent>(OnBombRemoveEvent);
            model.shipCreate += new EventHandler<ShipCreateEvent>(OnShipCreateEvent);
            model.bombCreate += new EventHandler<BombCreateEvent>(OnBombCreateEvent);
            player = new Position();
            ships = new List<Elem>();
            bombs = new List<Elem>();
            _mapSize = mapSize;
            baseColor = Color.AliceBlue;
            shipColor = Color.Maroon;
            playerColor = Color.LimeGreen;
            lightBombColor = Color.Tomato;
            mediumBombColor = Color.Firebrick;
            heavyBombColor = Color.PaleVioletRed;

            elements = new BaseButton[mapSize, mapSize];

            table.RowCount = mapSize;
            table.ColumnCount = mapSize;
            table.Dock = DockStyle.Fill;
            for (int i = 0; i < mapSize; i++)
            {
                var percent = 100f / (float)mapSize;
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percent));
                table.RowStyles.Add(new RowStyle(SizeType.Percent, percent));
            }
            this.Controls.Add(table);
        }

        void NewGame()
        {
            for (int i = 0; i < _mapSize; i++)
            {
                for (int j = 0; j < _mapSize; j++)
                {
                    var baseElem = new BaseButton(baseColor);
                    elements[i, j] = baseElem;
                    table.Controls.Add(baseElem, i, j);
                }
            }

            model.NewGame(_mapSize - 1, _mapSize - 1, 1);
            player.SetPosition(_mapSize - 1, _mapSize - 1);
            elements[player._x, player._y].BackColor = playerColor;
        }
    }
}
