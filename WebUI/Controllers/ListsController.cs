using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Lists.Commands.AddPoint;
using Application.Lists.Commands.CreateList;
using Application.Lists.Commands.DeletePoint;
using Application.Lists.Queries.GetIdentifiedSquares;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers;

public class ListsController : BaseController
{
    private readonly ISquareCalculationService _squareCalculationService;
    private readonly IAppDbContext _dbContext;

    public ListsController(ISquareCalculationService squareCalculationService, IAppDbContext dbContext)
    {
        _squareCalculationService = squareCalculationService;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Create a list of points (X, Y).
    /// </summary>
    /// <remarks>
    /// Note that created list will contain unique points only. E. g. Adding (1, 1), (1, 1)
    /// will result in a list created containing (1, 1) only. 
    /// </remarks>
    /// <returns>Returns created list's id.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateListResponse>> CreateList([FromBody] CreateListCommand request)
    {
        var list = new List
        {
            Points = request.Points.Select(MapPoint).ToList()
        };

        await _dbContext.Lists.AddAsync(list);
        await _dbContext.SaveChangesAsync();

        return Ok(new CreateListResponse(list.Id));
    }

    /// <summary>
    /// Add a point to existing list.
    /// </summary>
    /// <returns>Returns updated points in list.</returns>
    [HttpPut("add-point")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddPointResponse>> AddPoint([FromQuery] AddPointCommand request)
    {
        var list = await _dbContext.Lists
            .Include(list => list.Points)
            .Include(list => list.Squares)
            .SingleOrDefaultAsync(list => list.Id == request.ListId);

        if (list == null)
        {
            return NotFound(new { Message = "Provided ID of the list does not exist." });
        }

        if (list.Points.Any(point => HasSameCoordinates(request.Point, point)))
        {
            return BadRequest(new { Message = "Point already exists in a given list." });
        }

        list.Points.Add(MapPoint(request.Point));
        list.Squares.Clear();

        await _dbContext.SaveChangesAsync();

        var response = new AddPointResponse(list.Points.Select(MapPointModel));

        return Ok(response);
    }

    /// <summary>
    /// Delete a specified point from a list. 
    /// </summary>
    /// <returns>Returns an updated list.</returns>
    [HttpDelete("delete-point")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeletePointResponse>> DeletePoint([FromQuery] DeletePointCommand request)
    {
        var list = await _dbContext.Lists
            .Include(list => list.Points)
            .Include(list => list.Squares)
            .SingleOrDefaultAsync(list => list.Id == request.ListId);

        if (list == null)
        {
            return NotFound(new { Message = "Provided ID of the list does not exist." });
        }

        var point = list.Points.SingleOrDefault(point => HasSameCoordinates(request.Point, point));

        if (point == null)
        {
            return BadRequest(new { Message = "Such point does not exist." });
        }

        list.Points.Remove(point);
        list.Squares.Clear();

        await _dbContext.SaveChangesAsync();

        var response = new DeletePointResponse(list.Points.Select(MapPointModel));
        return Ok(response);
    }

    /// <summary>
    /// Get identified squares of a list.
    /// </summary>
    /// <returns>Returns a list of squares that were identified from list's points</returns>
    [HttpGet("get-identified-squares")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetIdentifiedSquaresResponse>> GetIdentifiedSquares(
        [FromQuery] GetIdentifiedSquaresQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.Lists
            .Include(list => list.Points)
            .Include(list => list.Squares)
            .SingleOrDefaultAsync(list => list.Id == request.ListId, cancellationToken);

        if (list == null)
        {
            return NotFound(new { Message = "Provided ID of the list does not exist." });
        }

        if (!list.Squares.Any())
        {
            var points = list.Points.Select(MapPointModel).ToList();
            var squares = await _squareCalculationService.CalculateSquaresAsync(points, cancellationToken);

            list.Squares.AddRange(squares.Select(MapSquare));

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var response = new GetIdentifiedSquaresResponse(list.Squares.Select(MapSquareModel));
        return Ok(response);
    }

    private static bool HasSameCoordinates(PointModel model, Point point)
    {
        return model.X == point.X && model.Y == point.Y;
    }

    private static Point MapPoint(PointModel model)
    {
        return new Point { X = model.X, Y = model.Y };
    }

    private static PointModel MapPointModel(Point point)
    {
        return new PointModel(point.X, point.Y);
    }

    private static Square MapSquare(SquareModel model)
    {
        return new Square
        {
            Point1X = model.Point1.X,
            Point1Y = model.Point1.Y,
            Point2X = model.Point2.X,
            Point2Y = model.Point2.Y,
            Point3X = model.Point3.X,
            Point3Y = model.Point3.Y,
            Point4X = model.Point4.X,
            Point4Y = model.Point4.Y
        };
    }

    private static SquareModel MapSquareModel(Square square)
    {
        return new SquareModel(
            new PointModel(square.Point1X, square.Point1Y),
            new PointModel(square.Point2X, square.Point2Y),
            new PointModel(square.Point3X, square.Point3Y),
            new PointModel(square.Point4X, square.Point4Y));
    }
}

