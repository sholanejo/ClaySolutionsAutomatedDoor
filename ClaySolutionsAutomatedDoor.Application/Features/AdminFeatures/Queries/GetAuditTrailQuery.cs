using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Common.Utility;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Query
{
    public class GetAuditTrailQuery : IRequest<BaseResponse<PaginatedParameter<AuditTrailDto>>>
    {
        [Required]
        public string UserId { get; set; }
        public int? PageSize { get; set; }
        public int? Page { get; set; }
    }

    public class GetAuditTrailQueryHandler(ILogger<GetAuditTrailQueryHandler> _logger,
        IUnitOfWorkRepository _unitOfWorkRepository,
        UserManager<ApplicationUser> _userManager) : IRequestHandler<GetAuditTrailQuery, BaseResponse<PaginatedParameter<AuditTrailDto>>>
    {
        public async Task<BaseResponse<PaginatedParameter<AuditTrailDto>>> Handle(GetAuditTrailQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting a list of audit trails");
            var applicationUser = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (applicationUser == null)
            {
                return BaseResponse<PaginatedParameter<AuditTrailDto>>.FailedResponse(Constants.UserDoesNotExistMessage, StatusCodes.Status400BadRequest);
            }

            var auditTrail = await _unitOfWorkRepository.AuditTrailRepository
                .GetQueryAsync(x => x.PerformedBy == request.UserId);

            var auditTrailDto = auditTrail.Select(x => new AuditTrailDto
            {
                DateCreated = x.DateCreated,
                Notes = x.Notes,
                UserId = Guid.Parse(x.PerformedBy)
            });

            PaginatedParameter<AuditTrailDto> auditTrails = new PaginatedParameter<AuditTrailDto>(auditTrailDto, request.Page, request.PageSize);
            return BaseResponse<PaginatedParameter<AuditTrailDto>>.PassedResponse(Constants.ApiOkMessage, auditTrails);
        }
    }
}
