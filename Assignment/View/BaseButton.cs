using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment.View
{
    class BaseButton : Button
    {
        public BaseButton(Color baseColor) : base()
        {
            this.BackColor = baseColor;
            this.Dock = DockStyle.Fill;
            //this.FlatStyle = FlatStyle.Flat;
            this.Text = "";
            this.Enabled = false;
        }
    }
}
