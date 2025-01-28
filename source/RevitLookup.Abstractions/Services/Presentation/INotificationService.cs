﻿namespace RevitLookup.Abstractions.Services.Presentation;

public interface INotificationService
{
    void ShowSuccess(string title, string message);
    void ShowWarning(string title, string message);
    void ShowError(string title, string message);
    void ShowError(string title, Exception exception);
}