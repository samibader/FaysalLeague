using Microsoft.AspNetCore.Mvc.Filters;

namespace FaisalLeague.Helpers
{
    public interface IActionTransactionHelper
    {
        void BeginTransaction();
        void EndTransaction(ActionExecutedContext filterContext);
        void CloseSession();
    }
}