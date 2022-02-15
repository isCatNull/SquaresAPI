using Application.Common.Models;

namespace Application.Lists.Commands.AddPoint;

public record AddPointResponse(IEnumerable<PointModel> Points);
