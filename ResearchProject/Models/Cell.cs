using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchProject.Models
{
    public class Cell
    {
        public bool IsAlive;

        private bool _isAliveNext;

        private const int CELL_MAX_NEIGHBOURS_NUMBER = 8;

        public readonly List<Cell> Neighbours = new(8);

        public void DetermineNextLiveState()
        {
            var aliveNeighboursNumber = Neighbours.Count(c => c.IsAlive);

            if (IsAlive)
                _isAliveNext = aliveNeighboursNumber == 2 || aliveNeighboursNumber == 3;
            else
                _isAliveNext = aliveNeighboursNumber == 3;
        }

        public void Advance() { IsAlive = _isAliveNext; }

    }
}
