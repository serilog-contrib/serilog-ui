using System;

namespace Serilog.Ui.MongoDbProvider.Tests.Util
{
    public class BaseIntegrationTest<T> : IDisposable where T : BaseServiceBuilder
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal T _builder;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public void Dispose()
        {
            if (_builder != null)
            {
                ((IDisposable)_builder).Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }

}
