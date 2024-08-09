using System.IO;

namespace Serilog.Ui.Web.Endpoints
{
    internal interface IAppStreamLoader
    {
        Stream? GetIndex();
    }
}