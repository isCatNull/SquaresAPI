using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface ISquareCalculationService
{
    Task<IEnumerable<SquareModel>> CalculateSquaresAsync(IList<PointModel> points, CancellationToken cancellationToken = default);
}
