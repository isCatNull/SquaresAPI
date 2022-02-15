using Application.Common.Models;

namespace Application.Lists.Commands.DeletePoint;

public record DeletePointResponse(IEnumerable<PointModel> Points);
