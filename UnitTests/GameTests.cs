using System;
using System.Linq;
using Game;
using NUnit.Framework;

namespace UnitTests
{
    public class GameTests
    {
        private GameEngine GameEngine { get; set; }

        [SetUp]
        public void Setup()
        {
            this.GameEngine = new GameEngine();
        }

        [TestCase("Ones", 1, 2, 3, 4, 5, 1)]
        [TestCase("Ones", 1, 2, 3, 4, 1, 2)]
        [TestCase("Twos", 1, 2, 3, 4, 5, 2)]
        [TestCase("Twos", 1, 2, 2, 2, 5, 6)]
        [TestCase("Threes", 1, 2, 3, 4, 5, 3)]
        [TestCase("Threes", 1, 2, 3, 4, 3, 6)]
        [TestCase("Fours", 1, 2, 3, 4, 5, 4)]
        [TestCase("Fours", 4, 4, 3, 4, 5, 12)]
        [TestCase("Fives", 1, 5, 3, 4, 5, 10)]
        [TestCase("Fives", 1, 2, 3, 4, 5, 5)]
        [TestCase("Sixes", 1, 2, 3, 4, 5, 0)]
        [TestCase("Sixes", 6, 6, 6, 4, 5, 18)]
        public void When_scoring_ones(string category, int dice1, int dice2, int dice3, int dice4, int dice5, int result)
        {
            var dices = DiceGenerator(dice1, dice2, dice3, dice4, dice5);
            var score = GameEngine.Score(category, dices);
            Assert.That(score, Is.EqualTo(result));
        }

        [TestCase("Three of a kind", new int[] {1, 1, 1, 4, 6}, 13)]
        [TestCase("Three of a kind", new int[] {1, 1, 2, 4, 6}, 0)]
        [TestCase("Three of a kind", new int[] {2, 2, 2, 2, 6}, 14)]
        [TestCase("Three of a kind", new int[] {3, 3, 5, 5, 5}, 21)]
        [TestCase("Four of a kind", new int[] {1, 1, 1, 4, 1}, 8)]
        [TestCase("Four of a kind", new int[] {1, 1, 2, 4, 6}, 0)]
        [TestCase("Four of a kind", new int[] {2, 2, 2, 2, 6}, 14)]
        [TestCase("Four of a kind", new int[] {3, 3, 5, 5, 5}, 0)]
        [TestCase("Full House", new int[] {3, 3, 5, 5, 5}, 25)]
        [TestCase("Full House", new int[] {3, 3, 5, 5, 3}, 25)]
        [TestCase("Full House", new int[] {3, 3, 5, 3, 3}, 0)]
        [TestCase("Full House", new int[] {3, 3, 3, 3, 3}, 0)]
        [TestCase("Small Straight", new int[] {1, 2, 3, 4, 3}, 30)]
        [TestCase("Small Straight", new int[] {1, 2, 3, 2, 5}, 0)]
        [TestCase("Small Straight", new int[] {6, 2, 3, 4, 5}, 30)]
        [TestCase("Large Straight", new int[] {1, 2, 3, 4, 5}, 40)]
        [TestCase("Large Straight", new int[] {2, 3, 4, 5, 6}, 40)]
        [TestCase("Large Straight", new int[] {6, 2, 3, 4, 5}, 0)]
        [TestCase("Large Straight", new int[] {2, 3, 4, 5, 1}, 0)]
        [TestCase("Yahtzee", new int[] {1, 2, 5, 5, 2}, 0)]
        [TestCase("Yahtzee", new int[] {3, 3, 3, 3, 3}, 50)]
        [TestCase("Chance", new int[] {1, 2, 5, 5, 2}, 15)]
        [TestCase("Chance", new int[] {3, 3, 3, 3, 3}, 15)]
        [TestCase("Pair", new int[] {3, 3, 4, 5, 6}, 10)]
        [TestCase("Pair", new int[] {3, 3, 4, 3, 3}, 20)]
        [TestCase("Two Pair", new int[] {2, 2, 3, 1, 1}, 20)]
        public void When_scoring_with_a_category(string category, int[] dices, int expectedPoints)
        {
            var points = GameEngine.Score(category, dices);
            Assert.That(points, Is.EqualTo(expectedPoints));
        }

        [TestCase("SomeCategory", new int[] {3, 3, 3, 3, 3})]
        public void When_scoring_witn_an_invalid_category(string category, int[] dices)
        {
            var result = Assert.Catch(() => GameEngine.Score(category, dices));

            Assert.That(result, Is.TypeOf(typeof(Exception)));
            Assert.That(result.Message, Is.EqualTo("Invalid category"));
        }

        [Test]
        public void When_invalid_number_of_dice()
        {
            Assert.Catch(() => GameEngine.Score("", new int[] {1, 2,3}), "Invalid number of dice");
        }

        [TestCase(new int[] {1,2,3,4,7})]
        [TestCase(new int[] {1,2,-1,4,2})]
        public void When_scoring_with_an_invalid_die(int[] dice)
        {
            var result = Assert.Catch(() => GameEngine.Score("Ones", dice));

            Assert.That(result, Is.TypeOf(typeof(Exception)));
            Assert.That(result.Message, Is.EqualTo("Invalid die"));
        }

        [TestCase(new int[] {1, 2, 3, 4, 5}, "Large Straight")]
        [TestCase(new int[] {1, 1, 1, 1, 1}, "Yahtzee")]
        [TestCase(new int[] {6, 5, 4, 3, 2}, "Chance")]
        [TestCase(new int[] {6, 6, 6, 6, 2}, "Four of a kind")]
        [TestCase(new int[] {6, 3, 4, 5, 6}, "Small Straight")]
        [TestCase(new int[] {6, 3, 4, 3, 3}, "Three of a kind")]
        public void When_scoring_return_category_with_the_highest_score(int[] dice, string expectedCategory)
        {
            var category = GameEngine.GetCategory(dice);

            Assert.That(category, Is.EqualTo(expectedCategory));
        }



        private int[] DiceGenerator(int dice1, int dice2, int dice3, int dice4, int dice5)
        {
            return new int[] { dice1, dice2, dice3, dice4, dice5 };
        }
    }
}