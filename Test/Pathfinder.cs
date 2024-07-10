
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Test;

internal struct AStarTile
{
    public Point Position { get; set; }
    public int Cost { get; set; }
    public int Distance { get; private set; }
    public int CostDistance { get; private set; }
    public int ParentKey { get; set; }


    public AStarTile(Point position, int cost, int parentKey, Point target)
    {
        Position = position;
        Cost = cost;
        ParentKey = parentKey;
        Distance = (int)Math.Sqrt((target.X - Position.X) * (target.X - Position.X) + (target.Y - Position.Y) * (target.Y - Position.Y));
        CostDistance = (Cost + Distance) * 10000 + 100 * Position.X + Position.Y;
    }
}

internal class Pathfinder
{
    private Grid _grid;

    public Pathfinder(Grid grid)
    {
        _grid = grid;
    }

    private readonly List<Point> mWalkableDirections = new()
    {
        new Point( 0,  1),
        new Point( 0, -1),
        new Point( 1,  0),
        new Point(-1,  0)
    };

    public List<Point> CalculatePath(Point startPos, Point endPos)
    {
        var end = new AStarTile(endPos, 0, 0, endPos);
        var start = new AStarTile(startPos, 0, 0, end.Position);
        var activeTiles = new PriorityQueue<AStarTile, int>(2500);
        activeTiles.Enqueue(start, start.CostDistance);
        var visitedTiles = new Dictionary<int, AStarTile>(5000);

        var closestTileToTarget = start;

        while (activeTiles.Count > 0 && visitedTiles.Count < 5000)
        {
            var checkTile = activeTiles.Dequeue();
            if (visitedTiles.TryGetValue(checkTile.CostDistance, out _))
            {
                continue;
            }
            visitedTiles.Add(checkTile.CostDistance, checkTile);
            if (checkTile.Distance < closestTileToTarget.Distance)
            {
                closestTileToTarget = checkTile;
                if (checkTile.Position == end.Position)
                {
                    var path = new List<Point>() { closestTileToTarget.Position };
                    if (!visitedTiles.TryGetValue(closestTileToTarget.ParentKey, out var currentTile))
                    {
                        return path;
                    }

                    while (currentTile.Position != start.Position && visitedTiles.TryGetValue(currentTile.ParentKey, out currentTile))
                    {
                        path.Add(currentTile.Position);
                    }
                    path.Reverse();
                    return path;
                }
            }
            foreach (var direction in mWalkableDirections)
            {
                if (!IsTileWalkable(checkTile.Position + direction))
                {
                    continue;
                }
                var newTile = new AStarTile(checkTile.Position + direction, checkTile.Cost + 1, checkTile.CostDistance, end.Position);
                activeTiles.Enqueue(newTile, newTile.CostDistance);
            }
        }

        return new List<Point>();
    }

    private bool IsTileWalkable(Point point)
    {
        return _grid.IsPathable(point);
    }
}
