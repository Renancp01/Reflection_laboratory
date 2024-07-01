using Contracts;
using Contracts.Extensions;

namespace Contacts.Test;

public class LinkExtensionsTests
{
    [Fact]
    public void FillParamsFromSource_ShouldFillParamsWhenNullOrEmpty()
    {
        // Arrange
        var link = new Link();
        link.Params["TestProperty"] = "";
        var source = new { TestProperty = "TestValue" };
        var items = new List<TestItem> { new TestItem { Link = link } };

        // Act
        LinkExtensions.FillParamsFromSourceList(items, source);

        // Assert
        Assert.Equal("TestValue", link.Params["TestProperty"]);
    }

    [Fact]
    public void FillParamsFromSource_ShouldNotOverwriteNonEmptyValue()
    {
        // Arrange
        var link = new Link();
        link.Params["TestProperty"] = "ExistingValue";
        var source = new { TestProperty = "NewValue" };
        var items = new List<TestItem> { new TestItem { Link = link } };

        // Act
        LinkExtensions.FillParamsFromSourceList(items, source);

        // Assert
        Assert.Equal("ExistingValue", link.Params["TestProperty"]);
    }

    [Fact]
    public void FillParamsFromSource_ShouldNotModifyWhenLinkIsNull()
    {
        // Arrange
        TestItem item = null;
        var source = new { TestProperty = "TestValue" };
        var items = new List<TestItem> { item };

        // Act
        LinkExtensions.FillParamsFromSourceList(items, source);

        // Assert
        // Nothing to assert as item is null, just ensuring no exceptions are thrown
    }

    [Fact]
    public void FillParamsFromSource_ShouldHandleKeyNotPresentInParams()
    {
        // Arrange
        var link = new Link();
        var source = new { TestProperty = "TestValue" };
        var items = new List<TestItem> { new TestItem { Link = link } };

        // Act
        LinkExtensions.FillParamsFromSourceList(items, source);

        // Assert
        Assert.False(link.Params.ContainsKey("TestProperty"));
    }

    [Fact]
    public void FillParamsFromSourceList_ShouldFillParamsForAllItemsWhenNullOrEmpty()
    {
        // Arrange
        var item1 = new TestItem { Link = new Link { Params = { ["TestProperty"] = "" } } };
        var item2 = new TestItem { Link = new Link { Params = { ["TestProperty"] = "" } } };
        var items = new List<TestItem> { item1, item2 };
        var source = new { TestProperty = "TestValue" };

        // Act
        LinkExtensions.FillParamsFromSourceList(items, source);

        // Assert
        Assert.Equal("TestValue", item1.Link.Params["TestProperty"]);
        Assert.Equal("TestValue", item2.Link.Params["TestProperty"]);
    }
}
public class TestItem : ILinkableItem
{
    public Link Link { get; set; } = new();
}
