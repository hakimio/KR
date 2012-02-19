using System.Collections.Generic;

public interface IHasNeighbours<N>
{
    IEnumerable<N> Neighbours { get; }
}