using Gizo.Application.Cars.Dtos;
using Gizo.Application.Models;
using MediatR;

namespace Gizo.Application.Cars.Queries;

public sealed record GetAllCarsQuery() : IRequest<OperationResult<List<CarsDto>>>;
