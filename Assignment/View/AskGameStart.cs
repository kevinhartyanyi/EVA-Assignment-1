using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment.View
{
    public partial class AskGameStart : Form
    {
        private int _shipNumber;
        private int _mapSize;
        private string _loadGame = "";
        private bool _closeGame = true;

        public int ShipNumber { get { return _shipNumber; } }
        public string LoadGame { get { return _loadGame; } }

        public int MapSize { get { return _mapSize; } }

        public bool CloseGame { get { return _closeGame; } }

        private NumericUpDown shipNumber;
        private NumericUpDown mapSize;

        void OnMapSizeChanged(object sender, EventArgs e)
        {
            _mapSize = (int)mapSize.Value;
        }
        void OnShipNumberChanged(object sender, EventArgs e)
        {
            _shipNumber = (int)shipNumber.Value;
        }

        void OnOKClickedEvent(object sender, EventArgs e)
        {
            _closeGame = false;
            this.Close();
        }

        void OnFormClosingEvent(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine(_closeGame);

        }


        void OnGameLoadgEvent(object sender, EventArgs e)
        {
            Console.WriteLine("Load Game");
            string fileName = "";
            var t = new Thread((ThreadStart)(() => {
                OpenFileDialog sFile = new OpenFileDialog();
                sFile.Filter = "Save files (*.saveGame)|*.saveGame";
                sFile.ShowDialog();
                fileName = sFile.FileName;
                _loadGame = fileName;
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            if(fileName != "")
                this.Close();
        }

        public AskGameStart(int gameTime)
        {
            InitializeComponent();
            _shipNumber = 1;
            _mapSize = 10;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            shipNumber = new NumericUpDown();
            shipNumber.Minimum = 1;
            shipNumber.Dock = DockStyle.Bottom;
            shipNumber.ValueChanged += new EventHandler(OnShipNumberChanged);
            Label shipLabel = new Label();
            shipLabel.Dock = DockStyle.Top;
            shipLabel.AutoSize = true;
            shipLabel.Text = "Ship Number";
            shipLabel.Font = new Font("Arial", 24, FontStyle.Bold);

            mapSize = new NumericUpDown();
            mapSize.Minimum = 5;
            mapSize.Value = 10;
            mapSize.Dock = DockStyle.Bottom;
            mapSize.ValueChanged += new EventHandler(OnMapSizeChanged);
            Label mapLabel = new Label();
            mapLabel.Dock = DockStyle.Top;
            mapLabel.AutoSize = true;
            mapLabel.Text = "Map Size";
            mapLabel.Font = new Font("Arial", 24, FontStyle.Bold);

            Panel mapInfo = new Panel();
            mapInfo.Dock = DockStyle.Top;
            SplitContainer shipSplit = new SplitContainer();
            shipSplit.Panel1.Controls.Add(mapLabel);
            shipSplit.Panel2.Controls.Add(mapSize);
            mapInfo.Controls.Add(mapLabel);
            mapInfo.Controls.Add(mapSize);
            mapInfo.Padding = new Padding(0, 40, 0, 0);

            Panel shipInfo = new Panel();
            shipInfo.Dock = DockStyle.Top;
            shipInfo.Controls.Add(shipLabel);
            shipInfo.Controls.Add(shipNumber);
            shipInfo.Padding = new Padding(0, 40, 0, 0);

            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Fill;
            Button ok = new Button();
            ok.Text = "New Game";
            ok.Font = new Font("Arial", 24, FontStyle.Bold);
            ok.Dock = DockStyle.Fill;
            ok.Click += new EventHandler(OnOKClickedEvent);
            buttonPanel.Controls.Add(ok);

            Panel gameStatPanel = new Panel();
            gameStatPanel.Dock = DockStyle.Top;
            Label gameStat = new Label();
            gameStat.Font = new Font("Arial", 24, FontStyle.Bold);
            if (gameTime == -1)
                gameStat.Text = "Start a new game";
            else
                gameStat.Text = "You survived for: " + gameTime + " seconds";
            gameStat.Dock = DockStyle.Fill;
            gameStat.TextAlign = ContentAlignment.MiddleCenter;
            gameStatPanel.Controls.Add(gameStat);

            Panel gameLoadPanel = new Panel();
            gameLoadPanel.Dock = DockStyle.Right;
            Button gameLoad = new Button();
            gameLoad.Font = new Font("Arial", 14, FontStyle.Bold);
            gameLoad.Text = "Load Game";
            gameLoad.Dock = DockStyle.Fill;
            gameLoad.TextAlign = ContentAlignment.MiddleCenter;
            gameLoad.Click += new EventHandler(OnGameLoadgEvent);
            gameLoadPanel.Controls.Add(gameLoad);

            this.Controls.Add(buttonPanel);
            this.Controls.Add(shipInfo);
            this.Controls.Add(mapInfo);
            this.Controls.Add(gameStatPanel);
            this.Controls.Add(gameLoadPanel);

            this.FormClosed += new FormClosedEventHandler(OnFormClosingEvent);

        }

    }
}
