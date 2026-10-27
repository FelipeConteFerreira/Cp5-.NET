using FitTrack.Domain.Entities;
using FitTrack.Application.Common;
using Xunit;

namespace FitTrack.Tests;

public sealed class ExerciseTests
{
    [Fact]
    public void Exercise_Rejects_Empty_Name()
    {
        Assert.Throws<ArgumentException>(() => new Exercise(" ", "Peito", 30));
    }

    [Fact]
    public void Exercise_Rejects_NonPositive_Duration()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Exercise("Supino", "Peito", 0));
    }

    [Fact]
    public void PageResult_Calculates_Total_Pages()
    {
        var result = PageResult<int>.Create(new[] { 1, 2 }, 1, 2, 5);
        Assert.Equal(3, result.TotalPages);
        Assert.Equal(5, result.TotalItems);
    }
}
