﻿namespace SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackeUsersProjects;

public class GetTrackerUsersProjectsDto
{
    public string? Name { get; set; }
    public double Hours { get; set; }
    public string? Type { get; set; } // "user" or "project"
}
