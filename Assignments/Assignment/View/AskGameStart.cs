using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment.View
{
    public partial class AskGameStart : Form
    {
        private int _shipNumber;
        private int _mapSize;

        public int ShipNumber { get { return _shipNumber; } }
        public int MapSize { get { return _mapSize; } }

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
            this.Close();
        }

        public AskGameStart()
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

            this.Controls.Add(buttonPanel);
            this.Controls.Add(shipInfo);
            this.Controls.Add(mapInfo);

        }
    }
}
