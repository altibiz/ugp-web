using CsvHelper.Configuration;
using CsvHelper;
using Members.Core;
using OrchardCore.BackgroundTasks;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YesSql;
using Members.Models;
using System.Collections.Concurrent;

namespace Members.Base
{

    [BackgroundTask(Schedule = "*/1 * * * *", Description = "Members export background task.")]
    public class MembersExport : IBackgroundTask
    {
        private readonly MemberService _memberService;
        private readonly ISession _session;

        public static readonly ConcurrentQueue<ExportFilterData> PendingExports = new();

        public MembersExport(MemberService memberService, ISession session)
        {
            _memberService = memberService;
            _session = session;
        }

        public Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            if (PendingExports.TryDequeue(out var toExport))
            {
                // Access the required services using the serviceProvider
                var memberService = (MemberService)serviceProvider.GetService(typeof(MemberService));
                var session = (ISession)serviceProvider.GetService(typeof(ISession));

                // Implement the logic from your existing methods

                DateTime date = new DateTime(1801, 1, 1); // use a format that works for your use case

                var exportCounty = toExport.County ?? ""; // Set the desired value for exportCounty
                var exportActivity = toExport.Activity ?? new string[0]; // Set the desired value for exportActivity

                var csvConfig = new CsvConfiguration(new CultureInfo("hr-HR"))
                {
                    ShouldQuote = args => true
                };


                var exportFolder = "ExportMembers"; // Specify the folder name
                var fileName = "Members.csv"; // Specify the file name

                var exportPath = Path.Combine(Directory.GetCurrentDirectory(), exportFolder);


                // Create the export folder if it does not exist
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }

                var filePath = Path.Combine(exportPath, fileName);


                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter, csvConfig))
                {
                    var pageSize = 100; // Adjust the batch size as needed

                    var task1 = _memberService.WriteMembersToCsv(date, exportCounty, csvWriter, pageSize);
                    var task2 = _memberService.WriteCompaniesToCsv(date, exportCounty, exportActivity, csvWriter, pageSize);

                    Task.WhenAll(task1, task2).GetAwaiter().GetResult();

                    csvWriter.Flush();
                    memoryStream.Position = 0;

                    var bytInStream = memoryStream.ToArray();

                    var info = session.Query<ExportInfo>().FirstOrDefaultAsync().GetAwaiter().GetResult() ?? new ExportInfo();
                    info.LastSave = DateTime.Now;
                    session.Save(info);

                    // Perform any desired action with the generated file here
                    // For example, save it to disk or upload it to a storage service

                    File.WriteAllBytes(filePath, bytInStream);

                }
            }
            return Task.CompletedTask;

        }
    }
}
