using Application.Common.Interfaces;
using Application.Common.Models;

namespace Infrastructure.Services;

public class SquareCalculationService : ISquareCalculationService
{
    public async Task<IEnumerable<SquareModel>> CalculateSquaresAsync(
        IList<PointModel> points, CancellationToken cancellationToken = default)
    {
        var squares = await Task.Run(() => CalculateSquares(points), cancellationToken);

        var result = new List<SquareModel>();

        for (var i = 0; i < squares.Count; i += 4)
        {
            var point1 = squares[i];
            var point2 = squares[i + 1];
            var point3 = squares[i + 2];
            var point4 = squares[i + 3];

            result.Add(
                new SquareModel(
                    new PointModel(point1[0], point1[1]),
                    new PointModel(point2[0], point2[1]),
                    new PointModel(point3[0], point3[1]),
                    new PointModel(point4[0], point4[1])));
        }

        return result;
    }

    private static IList<int[]> CalculateSquares(IList<PointModel> points)
    {
        if (points.Count < 4) return new List<int[]>();

        var results = new List<int[]>();

        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i; j < points.Count; j++)
            {
                if (j == i) continue;
                for (int k = j; k < points.Count; k++)
                {
                    if (k == j || k == i) continue;
                    for (int n = k; n < points.Count; n++)
                    {
                        if (n == i || n == j || n == k) continue;

                        var point1 = new int[] { points[i].X, points[i].Y };
                        var point2 = new int[] { points[j].X, points[j].Y };
                        var point3 = new int[] { points[k].X, points[k].Y };
                        var point4 = new int[] { points[n].X, points[n].Y };

                        if (IsValidSquare(point1, point2, point3, point4))
                        {
                            results.Add(point1);
                            results.Add(point2);
                            results.Add(point3);
                            results.Add(point4);
                        }
                    }
                }
            }
        }

        return results;
    }

    private static bool IsValidSquare(int[] p1, int[] p2, int[] p3, int[] p4)
    {
        var distinct = new HashSet<int>
        {
            Dist(p1, p2),
            Dist(p1, p3),
            Dist(p1, p4),
            Dist(p2, p3),
            Dist(p2, p4),
            Dist(p3, p4)
        };

        return distinct.Count == 2 && !distinct.Contains(0);
    }

    private static int Dist(int[] p1, int[] p2) => (p1[0] - p2[0]) * (p1[0] - p2[0]) + (p1[1] - p2[1]) * (p1[1] - p2[1]);
}