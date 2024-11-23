using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

const int howMany = 51;
const int sheetNumber = 1;
var desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); 
Random random = new ();

Console.WriteLine("Okay, let's create a sheet...");

List<(int left, int right)> tasks = [];

while (tasks.Count <= howMany)
{
    var task = (random.Next(1, 10),random.Next(1, 10));
    if (!tasks.Contains(task))
    {
        tasks.Add(task);
    }
}

Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(1, Unit.Centimetre);
        page.DefaultTextStyle(x => x.FontSize(14));
        page.Header().AlignRight().Text($"{DateTime.Now:D}  |  Sheet #{sheetNumber}").FontSize(10);
                
        page.Content().Column(column =>
        {
            column.Spacing(8);
            column.Item().Height(5);

            column.Item().Row(row =>
            {
                row.RelativeItem().Column(PickTasks(1, 17));
                row.RelativeItem().Column(PickTasks(18, 34));
                row.RelativeItem().Column(PickTasks(35, 51));
            });
        });
    });
}).GeneratePdf(Path.Combine(desktopDirectory, "MultiplicationTasks.pdf"));

Console.WriteLine("Done!");

return;

Action<ColumnDescriptor> PickTasks(int start, int end) => column =>
{
    column.Spacing(13);
    for (var i = start - 1; i < end; i++)
    {
        var taskNumber = $"{i + 1}:".PadRight(5);
        column.Item().Text($"{taskNumber}{tasks[i].left} x {tasks[i].right} = __________");
    }
};