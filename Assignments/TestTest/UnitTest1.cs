using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assignment.Data;
using Assignment.Model;
using System.Linq;
namespace TestTest
{
    [TestClass]
    public class UnitTest1
    {
        private GameControlModel model;
        private int shipNumber;
        private int bombNumber;
        private Position player;
        private int mapSize;
        private Mock<IData> data;

        [TestInitialize]
        public void TestInitialize()
        {
            data = new Mock<IData>();
            model = new GameControlModel(data.Object);
            data.Setup(mock => mock.Load(It.IsAny<String>())).Returns(new ModelValues());
            data.Setup(mock => mock.Save(It.IsAny<String>(), model));

            // perzisztencia nélküli modellt hozunk létre
            shipNumber = 3;
            bombNumber = 0;
            mapSize = 10;
            model.bombCreate += new EventHandler<BombCreateEvent>(OnBombCreateEvent);
            model.gameOver += new EventHandler<GameOverEvent>(OnGameOverEvent);
            NewGame();
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
            var x = model.playerX;
            var y = model.playerY;
            var size = model.mapSize;
            var time = model.gameTime;
            var dTime = model.difficultyTime;
            var bID = model.bombID;
            var sCount = model.shipCount;
            model.SaveGame(String.Empty);
            Assert.AreEqual(model.playerX, x);
            Assert.AreEqual(model.playerY, y);
            Assert.AreEqual(model.mapSize, size);
            Assert.AreEqual(model.gameTime, time);
            Assert.AreEqual(model.difficultyTime, dTime);
            Assert.AreEqual(model.bombID, bID);
            Assert.AreEqual(model.shipCount, sCount);
        }
        [TestMethod]
        public void NewGame()
        {
            model.NewGame(mapSize, mapSize - 1, mapSize - 1, shipNumber);
            player = new Position(mapSize - 1, mapSize - 1);
            Assert.AreEqual(model.playerX, mapSize - 1);
            Assert.AreEqual(model.playerY, mapSize - 1);
            Assert.AreEqual(model.gameTime, 0);
            Assert.AreEqual(model.difficultyTime, 1000);
            Assert.AreEqual(model.bombID, 0);
            Assert.AreEqual(model.shipCount, 3);
        }

        void OnGameOverEvent(object sender, GameOverEvent e)
        {
            Assert.IsFalse(model.isPlaying);
            Assert.AreNotEqual(model.gameTime, 0);
        }


        void OnBombCreateEvent(object sender, BombCreateEvent e)
        {
            Assert.AreNotEqual(model.bombs.Count, bombNumber);
            bombNumber++;
        }

        [TestMethod]
        public void PlayerMove()
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

            model.PlayerMove(Move.Down);
            Assert.AreEqual(model.playerY, player._y + 1);
            player._y += 1;

        }
    }
}
