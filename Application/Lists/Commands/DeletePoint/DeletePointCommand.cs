﻿using Application.Common.Models;

namespace Application.Lists.Commands.DeletePoint;

public record DeletePointCommand(int ListId, PointModel Point);
