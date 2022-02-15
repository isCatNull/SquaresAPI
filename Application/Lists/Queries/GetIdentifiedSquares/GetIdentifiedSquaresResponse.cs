using Application.Common.Models;

namespace Application.Lists.Queries.GetIdentifiedSquares;
public record GetIdentifiedSquaresResponse(IEnumerable<SquareModel> Squares);
