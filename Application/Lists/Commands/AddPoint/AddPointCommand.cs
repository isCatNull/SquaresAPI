using Application.Common.Models;

namespace Application.Lists.Commands.AddPoint;
public record AddPointCommand(int ListId, PointModel Point);
