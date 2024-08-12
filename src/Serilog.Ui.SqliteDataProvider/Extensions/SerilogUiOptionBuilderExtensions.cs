﻿using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Core.Models.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.SqliteDataProvider.Extensions
{
    /// <summary>
    /// SQLite data provider specific extension methods for <see cref="ISerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary> Configures the SerilogUi to connect to a SQLite database.</summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="setupOptions">The SQLite options action.</param>
        public static ISerilogUiOptionsBuilder UseSqliteServer(
            this ISerilogUiOptionsBuilder optionsBuilder,
            Action<RelationalDbOptions> setupOptions)
        {
            var dbOptions = new RelationalDbOptions(string.Empty);
            setupOptions(dbOptions);
            dbOptions.Validate();

            optionsBuilder.Services.AddScoped<IDataProvider, SqliteDataProvider>(p => new SqliteDataProvider(dbOptions));

            return optionsBuilder;
        }
    }
}