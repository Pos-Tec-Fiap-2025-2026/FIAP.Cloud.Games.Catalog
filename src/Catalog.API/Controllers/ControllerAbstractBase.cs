using Catalog.Application;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    public class ControllerAbstractBase : ControllerBase
    {
        protected bool IsNullOrEmpty<T>(ResultBase<IEnumerable<T>> result) where T : class
        {
            return !result.IsSuccess || (result.Value != null && !result.Value.Any());
        }

        protected bool IsNullOrEmpty<T>(ResultBase<T?> result) where T : class
        {
            return !result.IsSuccess;
        }
    }
}
