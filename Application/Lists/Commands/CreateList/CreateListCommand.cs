using Application.Common.Models;

namespace Application.Lists.Commands.CreateList;

public record CreateListCommand(ISet<PointModel> Points);

