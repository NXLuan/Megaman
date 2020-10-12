using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameMegaman.Effect;

namespace GameMegaman.UserInterface
{
    public partial class GameFrame : Form
    {
        public static int SCREEN_WIDTH = 1000;
        public static int SCREEN_HEIGHT = 600;
        GamePanel gamePanel;

        public GameFrame()
        {
            InitializeComponent();
            this.Size = new Size(SCREEN_WIDTH, SCREEN_HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Game Megaman";

            CacheDataLoader.getInstance().LoadData();

            gamePanel = new GamePanel();
            this.Controls.Add(gamePanel);
            gamePanel.Dock = DockStyle.Fill;

            this.KeyDown += GameFrame_KeyDown;
            this.KeyUp += GameFrame_KeyUp;
        }

        private void GameFrame_KeyUp(object sender, KeyEventArgs e)
        {
            gamePanel.KeyReleased(e.KeyCode);
        }

        private void GameFrame_KeyDown(object sender, KeyEventArgs e)
        {
            gamePanel.KeyPressed(e.KeyCode);
        }

        public void startGame()
        {
            gamePanel.startGame();
        }
        protected override void OnLoad(EventArgs e)
        {
            startGame();
        }
    }
}
