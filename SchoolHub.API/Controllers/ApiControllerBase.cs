using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RT.Comb;
using SchoolHub.Common.Models.Entities.Users;

namespace SchoolHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : Controller
    {
        protected readonly ICombProvider _comb;
        protected readonly IMapper _mapper;

        public Guid TennantIdUserLoggedIn { get; private set; }

        public ApiControllerBase(ICombProvider comb, IMapper mapper)
        {
            _comb = comb;
            _mapper = mapper;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.Identity.IsAuthenticated)
            {
                var tennantIdClaim = User.FindFirst("TennantId")?.Value;

                if (Guid.TryParse(tennantIdClaim, out var tennantId))
                {
                    TennantIdUserLoggedIn = tennantId;
                }
                else
                {
                    TennantIdUserLoggedIn = Guid.Empty; 
                }
            }
        }
    }
}
