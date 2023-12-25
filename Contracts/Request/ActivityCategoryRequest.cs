﻿namespace Contracts.Request;

public class ActivityCategoryRequest
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Value { get; init; }
}
