namespace DataAccessTests;
public class DataAccessTests
{
    IConfiguration Configuration { get; set; }

    public DataAccessTests()
    {
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<DataAccessTests>();

        Configuration = builder.Build();
    }

    [Fact]
    public async Task GetTodoItems_ShouldReturn()
    {
        // Arrange
        string? _connectionString = Configuration["ConnectionStrings:Default"];
        if (string.IsNullOrWhiteSpace(_connectionString)) { false.Should().BeTrue("Fail tests because there is nothing we can do here"); return; }

        Mock<ILogger<DapperDataAccess>> mock = new();
        ILogger<DapperDataAccess> logger = mock.Object;
        DapperDataAccess access = new(_connectionString, logger);

        //Act
        IEnumerable<MyTodoItem> result = await access.GetTodoItems(CancellationToken.None);
        List<MyTodoItem> expected = result.ToList<MyTodoItem>();

        // Assert
        expected.Should().NotBeNull();
        expected.Count.Should().BePositive(); // we know this because we wrote the script in create database
        expected.Where(x => x.Id == 1).Count().Should().Be(1);
        expected.Where(x => x.Id == 1).First().ExternalId.Should().NotBeEmpty();
        expected.Where(x => x.Id == 1).First().Title.Should().Be("Brush teeth");
        expected.Where(x => x.Id == 1).First().Description.Should().Be("I should brush my teeth every day");
        expected.Where(x => x.Id == 1).First().IsDone.Should().Be(true);
        expected.Where(x => x.Id == 1).First().CreatedBy.Should().Be(0);
        expected.Where(x => x.Id == 1).First().CreatedDate.Should().BeAfter(DateTime.MinValue);
        expected.Where(x => x.Id == 1).First().CreatedDate.Should().BeBefore(DateTime.UtcNow);
        expected.Where(x => x.Id == 1).First().ModifiedBy.Should().Be(0);
        expected.Where(x => x.Id == 1).First().ModifiedDate.Should().BeAfter(DateTime.MinValue);
        expected.Where(x => x.Id == 1).First().ModifiedDate.Should().BeBefore(DateTime.UtcNow);
    }
}