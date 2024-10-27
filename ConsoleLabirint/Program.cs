int[,] labyrinth1 = new int[,]
{
    { 1, 1, 1, 1, 1, 1, 1 },
    { 1, 0, 0, 0, 0, 0, 1 },
    { 1, 0, 1, 1, 1, 0, 1 },
    { 0, 0, 0, 0, 1, 0, 0 },
    { 1, 1, 0, 0, 1, 1, 1 },
    { 1, 1, 1, 0, 1, 1, 1 },
    { 1, 1, 1, 0, 1, 1, 1 }
};

int[,] labyrinth2 = new int[,]
{
    { 1, 1, 1, 1, 1, 1, 1 },
    { 1, 0, 0, 0, 0, 0, 1 },
    { 1, 0, 1, 1, 1, 0, 1 },
    { 1, 0, 0, 0, 1, 0, 1 },
    { 1, 1, 0, 0, 1, 1, 1 },
    { 1, 1, 1, 0, 1, 1, 1 },
    { 1, 1, 1, 0, 1, 1, 1 }
};

Labyrinth.HasLabyrinthExit(1, 2, labyrinth1);
Labyrinth.HasLabyrinthExit(1, 2, labyrinth2);


public static class Labyrinth
{
    private static readonly List<(int, int)> NeighborsCoordinatesDirections =
        [(0, -1), (-1, 0), (0, 1), (1, 0)];

    private static void FillNeighbors(
        int pointRowCoordinate,
        int pointColumnCoordinate,
        int[,] labyrinth,
        HashSet<(int, int)> neighbors
    )
    {
        foreach (var neighborsCoordinate in NeighborsCoordinatesDirections)
        {
            var neighborRowCoordinate = pointRowCoordinate + neighborsCoordinate.Item1;
            var neighborColumnCoordinate = pointColumnCoordinate + neighborsCoordinate.Item2;
            var isRowCoordinateValid = neighborRowCoordinate >= 0 && neighborRowCoordinate < labyrinth.GetLength(0);
            var isColumnCoordinateValid = neighborColumnCoordinate >= 0 && neighborColumnCoordinate < labyrinth.GetLength(1);
            var isAnyCoordinateValid = isRowCoordinateValid && isColumnCoordinateValid;
            var isNeighborsCoordinateExist = neighbors.Contains((neighborRowCoordinate, neighborColumnCoordinate));

            if (isAnyCoordinateValid &&
                labyrinth[neighborRowCoordinate, neighborColumnCoordinate] == 0 &&
                !isNeighborsCoordinateExist)
            {
                neighbors.Add((neighborRowCoordinate, neighborColumnCoordinate));
                FillNeighbors(neighborRowCoordinate, neighborColumnCoordinate, labyrinth, neighbors);
            }
        }
    }

    private static bool IsLabyrinthExit(int pointRowCoordinate, int pointColumnCoordinate, int[,] labyrinth)
    {
        var isEdgeRowCoordinate = pointRowCoordinate == 0 || pointRowCoordinate == labyrinth.GetLength(0) - 1;
        var isEdgeColumnCoordinate = pointColumnCoordinate == 0 || pointColumnCoordinate == labyrinth.GetLength(1) - 1;

        if (isEdgeRowCoordinate || isEdgeColumnCoordinate)
        {
            return labyrinth[pointRowCoordinate, pointColumnCoordinate] == 0;
        }

        return false;
    }

    public static void HasLabyrinthExit(int startRow, int startColumn, int[,] labyrinth)
    {
        if (labyrinth[startRow, startColumn] == 1)
        {
            throw new Exception("Задайте координату, свободную от перегородок лабиринта");
        }

        var neighbors = new HashSet<(int, int)>();

        FillNeighbors(startRow, startColumn, labyrinth, neighbors);

        if (neighbors.Count > 0)
        {
            var exitCount = 0;

            foreach (var neighbor in neighbors)
            {
                if (IsLabyrinthExit(neighbor.Item1, neighbor.Item2, labyrinth))
                {
                    exitCount++;
                }
            }

            if (exitCount > 0)
            {
                Console.WriteLine($"Лабиринт имеет {exitCount} выход(а)");

                return;
            }
        }

        Console.WriteLine("Лабиринт не имеет выхода");
    }
}