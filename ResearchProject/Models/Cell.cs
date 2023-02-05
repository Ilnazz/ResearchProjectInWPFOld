using System.Linq;
using System.Collections.Generic;

namespace ResearchProject.Models
{
    public class Cell
    {
        public bool IsAlive;

        public bool IsAliveNext;

        private const int CELL_MAX_NEIGHBOURS_NUMBER = 8;

        public readonly List<Cell> Neighbours = new(CELL_MAX_NEIGHBOURS_NUMBER);

        public void DetermineNextLiveState()
        {
            var aliveNeighboursNumber = Neighbours.Count(c => c.IsAlive);

            if (IsAlive)
                IsAliveNext = aliveNeighboursNumber == 2 || aliveNeighboursNumber == 3;
            else
                IsAliveNext = aliveNeighboursNumber == 3;
        }

        public void Advance() { IsAlive = IsAliveNext; }
    }
}
