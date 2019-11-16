using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class GameEngine
    {
        private Dictionary<string, Func<int[], int>> Categories = new Dictionary<string, Func<int[], int>>
        {
            ["Yahtzee"] = dice => dice.GroupBy(x=>x).Count() == 1 ? 50 : 0,
            ["Large Straight"] = dice => dice.Aggregate((x,y) => (x+1) == y ? y : 0) != 0 ? 40 : 0,
            ["Full House"] = dice => dice.GroupBy(x => x).Select(x => new { Count = x.Count()}).Where(x => x.Count == 2 || x.Count == 3).Count() == 2 ? 25 : 0,
            ["Small Straight"] = dice => (dice.SkipLast(1).Aggregate((x,y) => (x+1) == y ? y : 0) != 0) || (dice.Skip(1).Aggregate((x,y) => (x+1) == y ? y : 0) != 0) ? 30 : 0,
            ["Four of a kind"] = dice => dice.GroupBy(x => x).Select(x => new { Count = x.Count()}).Where(x => x.Count >= 4).Count() == 1 ? dice.Sum() : 0,
            ["Three of a kind"] = dice => dice.GroupBy(x => x).Select(x => new { Count = x.Count()}).Where(x => x.Count >= 3).Count() == 1 ? dice.Sum() : 0,
            ["Chance"] = dice => dice.Sum(),
            ["Two Pair"] = dice => dice.GroupBy(x=>x).Select(x=> new { Count = x.Count() }).Any(x => x.Count == 2) ? 20 : 0,
            ["Pair"] = dice => PairCalculator(dice),
            ["Fours"] =  dice => dice.Where(x=> x == 4).Sum(),
            ["Fives"] =  dice => dice.Where(x=> x == 5).Sum(),
            ["Sixes"] =  dice => dice.Where(x=> x == 6).Sum(),
            ["Threes"] = dice => dice.Where(x=> x == 3).Sum(),
            ["Twos"] =   dice => dice.Where(x=> x == 2).Sum(),
            ["Ones"] =   dice => dice.Where(x=> x == 1).Sum(),
        };

        public int Score(string category, int[] dice)
        {
            if(dice.Length != 5) 
            {
                throw new Exception("Invalid number of dice");
            }

            if(dice.Any(x => x > 6 || x < 1)) 
            {
                throw new Exception("Invalid die");
            }

            if(Categories.TryGetValue(category, out var categoryPointsCalculator))
            {
                return (int)categoryPointsCalculator(dice);
            }

            throw new Exception("Invalid category");
        }

        public string GetCategory(int[] dice)
        {
            var categoryLookup = Categories.Aggregate((x,y) => x.Value.Invoke(dice) >= y.Value.Invoke(dice) ? x : y);
            return categoryLookup.Key;
        }

        private static int PairCalculator(int[] dice)
        {
            var dieCounts = dice.GroupBy(x=>x).Select(x=> new { Count = x.Count() });
            if(dieCounts.Any(x => x.Count == 2))
            {
                return 10 * dieCounts.Where(x => x.Count == 2).Count();
            }

            if(dieCounts.Any(x => x.Count == 4))
            {
                return 20;
            }

            return 0;
        }
    }
}