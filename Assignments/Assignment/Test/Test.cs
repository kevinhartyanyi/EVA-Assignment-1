using Assignment.Data;
using Assignment.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assignment.Test
{

    [TestClass]
    class Test
    {
        private GameControlModel model;
        private int shipNumber;
        private int bombNumber;
        private Position player;
        private int mapSize;
        private Mock<IData> data;

        [TestInitialize]
        public void Initialize()
        {
            data = new Mock<IData>();
            data.Setup(mock => mock.Load(It.IsAny<String>())).Returns(new ModelValues());
            data.Setup(mock => mock.Save(It.IsAny<String>(), model));

            model = new GameControlModel(data.Object);
            // perzisztencia nélküli modellt hozunk létre
            shipNumber = 3;
            bombNumber = 0;
            mapSize = 10;
            model.bombCreate += new EventHandler<BombCreateEvent>(OnBombCreateEvent);
            model.gameOver += new EventHandler<GameOverEvent>(OnGameOverEvent);
        }

        [TestMethod]
        public void LoadTest()
        {
            model.LoadGame(String.Empty);
            data.Verify(mock => mock.Load(String.Empty), Times.Once());
        }

        [TestMethod]
        public void SaveTest()
        {
            model.SaveGame(String.Empty);
            data.Verify(mock => mock.Save(String.Empty, model), Times.Once());
        }

        void OnGameOverEvent(object sender, GameOverEvent e)
        {
            Assert.IsFalse(model.isPlaying);
            Assert.AreNotEqual(model.gameTime, 0);
        }

        [TestMethod]
        void NewGame()
        {
            model.NewGame(mapSize, mapSize - 1, mapSize - 1, shipNumber);
            player = new Position(mapSize - 1, mapSize - 1);
            Assert.AreEqual(model.playerX, mapSize - 1);
            Assert.AreEqual(model.playerY,mapSize - 1);
            Assert.AreEqual(model.gameTime, 0);
            Assert.AreEqual(model.difficultyTime, 1000);
            Assert.AreEqual(model.bombID, 0);
            Assert.AreEqual(model.shipCount, 3);
        }

        void OnBombCreateEvent(object sender, BombCreateEvent e)
        {
            Assert.AreNotEqual(model.bombs.Count, bombNumber);
            bombNumber++;
        }

        [TestMethod]
        void PlayerMove()
        {
            model.PlayerMove(Move.Left);
            Assert.AreEqual(model.playerX, player._x - 1);
            player._x -= 1;

            model.PlayerMove(Move.Right);
            Assert.AreEqual(model.playerX, player._x + 1);
            player._x += 1;

            model.PlayerMove(Move.Up);
            Assert.AreEqual(model.playerY, player._y - 1);
            player._y -= 1;

            model.PlayerMove(Move.Left);
            Assert.AreEqual(model.playerY, player._y + 1);
            player._y -= 1;

        }


    }

}
