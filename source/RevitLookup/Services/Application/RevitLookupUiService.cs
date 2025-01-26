﻿using System.Collections;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Autodesk.Revit.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.Models.Decomposition;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services.Application;
using RevitLookup.Abstractions.Services.Decomposition;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.UI.Framework.Views.Windows;
using Wpf.Ui;

namespace RevitLookup.Services.Application;

public sealed class RevitLookupUiService : IRevitLookupUiService, ILookupServiceParentStage, ILookupServiceRunStage
{
    private static readonly Dispatcher Dispatcher;
    private UiServiceImpl _uiService = null!; //Late init in the constructor

    static RevitLookupUiService()
    {
        var uiThread = new Thread(Dispatcher.Run);
        uiThread.SetApartmentState(ApartmentState.STA);
        uiThread.Start();

        Dispatcher = EnsureDispatcherStart(uiThread);
    }

    public RevitLookupUiService(IServiceScopeFactory scopeFactory)
    {
        if (Dispatcher.CheckAccess())
        {
            _uiService = new UiServiceImpl(scopeFactory);
        }
        else
        {
            Dispatcher.Invoke(() => _uiService = new UiServiceImpl(scopeFactory));
        }
    }

    public ILookupServiceShowStage Decompose(KnownDecompositionObject decompositionObject)
    {
        if (Dispatcher.CheckAccess())
        {
            _uiService.Decompose(decompositionObject);
        }
        else
        {
            Dispatcher.Invoke(() => _uiService.Decompose(decompositionObject));
        }

        return this;
    }

    public ILookupServiceShowStage Decompose(object? obj)
    {
        if (Dispatcher.CheckAccess())
        {
            _uiService.Decompose(obj);
        }
        else
        {
            Dispatcher.Invoke(() => _uiService.Decompose(obj));
        }

        return this;
    }

    public ILookupServiceShowStage Decompose(IEnumerable objects)
    {
        if (Dispatcher.CheckAccess())
        {
            _uiService.Decompose(objects);
        }
        else
        {
            Dispatcher.Invoke(() => _uiService.Decompose(objects));
        }

        return this;
    }

    public ILookupServiceShowStage Decompose(ObservableDecomposedObject decomposedObject)
    {
        if (Dispatcher.CheckAccess())
        {
            _uiService.Decompose(decomposedObject);
        }
        else
        {
            Dispatcher.Invoke(() => _uiService.Decompose(decomposedObject));
        }

        return this;
    }

    public ILookupServiceShowStage Decompose(List<ObservableDecomposedObject> decomposedObjects)
    {
        if (Dispatcher.CheckAccess())
        {
            _uiService.Decompose(decomposedObjects);
        }
        else
        {
            Dispatcher.Invoke(() => _uiService.Decompose(decomposedObjects));
        }

        return this;
    }

    public ILookupServiceParentStage AddParent(IServiceProvider parentProvider)
    {
        if (Dispatcher.CheckAccess())
        {
            _uiService.AddParent(parentProvider);
        }
        else
        {
            Dispatcher.Invoke(() => _uiService.AddParent(parentProvider));
        }

        return this;
    }

    public ILookupServiceDecomposeStage AddStackHistory(ObservableDecomposedObject item)
    {
        if (Dispatcher.CheckAccess())
        {
            _uiService.AddStackHistory(item);
        }
        else
        {
            Dispatcher.Invoke(() => _uiService.AddStackHistory(item));
        }

        return this;
    }

    public ILookupServiceRunStage Show<T>() where T : Page
    {
        if (Dispatcher.CheckAccess())
        {
            _uiService.Show<T>();
        }
        else
        {
            Dispatcher.Invoke(() => _uiService.Show<T>());
        }

        return this;
    }

    public void RunService<T>(Action<T> handler) where T : class
    {
        if (Dispatcher.CheckAccess())
        {
            _uiService.RunService(handler);
        }
        else
        {
            Dispatcher.Invoke(() => _uiService.RunService(handler));
        }
    }

    private static Dispatcher EnsureDispatcherStart(Thread thread)
    {
        Dispatcher? dispatcher = null;
        SpinWait spinWait = new();
        while (dispatcher is null)
        {
            spinWait.SpinOnce();
            dispatcher = Dispatcher.FromThread(thread);
        }

        // We must yield
        // Sometimes the Dispatcher is unavailable for current thread
        Thread.Sleep(1);

        return dispatcher;
    }

    private sealed class UiServiceImpl
    {
        private IServiceProvider? _parentProvider;
        private readonly List<Task> _activeTasks = [];
        private readonly IServiceScope _scope;
        private readonly IDecompositionService _decompositionService;
        private readonly IVisualDecompositionService _visualDecompositionService;
        private readonly INavigationService _navigationService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<RevitLookupUiService> _logger;
        private readonly Window _host;

        public UiServiceImpl(IServiceScopeFactory scopeFactory)
        {
            _scope = scopeFactory.CreateScope();

            _host = _scope.ServiceProvider.GetRequiredService<RevitLookupView>();
            _decompositionService = _scope.ServiceProvider.GetRequiredService<IDecompositionService>();
            _visualDecompositionService = _scope.ServiceProvider.GetRequiredService<IVisualDecompositionService>();
            _navigationService = _scope.ServiceProvider.GetRequiredService<INavigationService>();
            _notificationService = _scope.ServiceProvider.GetRequiredService<INotificationService>();
            _logger = _scope.ServiceProvider.GetRequiredService<ILogger<RevitLookupUiService>>();

            _host.Closed += (_, _) => _scope.Dispose();
        }

        public async void Decompose(KnownDecompositionObject decompositionObject)
        {
            try
            {
                await Task.WhenAll(_activeTasks);
            }
            catch
            {
                // ignored
            }
            finally
            {
                _activeTasks.Add(_visualDecompositionService.VisualizeDecompositionAsync(decompositionObject));
            }
        }

        public async void Decompose(object? obj)
        {
            try
            {
                await Task.WhenAll(_activeTasks);
            }
            catch
            {
                // ignored
            }
            finally
            {
                _activeTasks.Add(_visualDecompositionService.VisualizeDecompositionAsync(obj));
            }
        }

        public async void Decompose(IEnumerable objects)
        {
            try
            {
                await Task.WhenAll(_activeTasks);
            }
            catch
            {
                // ignored
            }
            finally
            {
                _activeTasks.Add(_visualDecompositionService.VisualizeDecompositionAsync(objects));
            }
        }

        public async void Decompose(ObservableDecomposedObject decomposedObject)
        {
            try
            {
                await Task.WhenAll(_activeTasks);
            }
            catch
            {
                // ignored
            }
            finally
            {
                _activeTasks.Add(_visualDecompositionService.VisualizeDecompositionAsync(decomposedObject));
            }
        }

        public async void Decompose(List<ObservableDecomposedObject> decomposedObjects)
        {
            try
            {
                await Task.WhenAll(_activeTasks);
            }
            catch
            {
                // ignored
            }
            finally
            {
                _activeTasks.Add(_visualDecompositionService.VisualizeDecompositionAsync(decomposedObjects));
            }
        }

        public async void AddParent(IServiceProvider parentProvider)
        {
            try
            {
                await Task.WhenAll(_activeTasks);
            }
            catch
            {
                // ignored
            }
            finally
            {
                var decompositionService = parentProvider.GetRequiredService<IDecompositionService>();
                _decompositionService.DecompositionStackHistory.AddRange(decompositionService.DecompositionStackHistory);
                _parentProvider = parentProvider;
            }
        }

        public async void AddStackHistory(ObservableDecomposedObject item)
        {
            try
            {
                await Task.WhenAll(_activeTasks);
            }
            catch
            {
                // ignored
            }
            finally
            {
                _decompositionService.DecompositionStackHistory.Add(item);
            }
        }

        public async void Show<T>() where T : Page
        {
            try
            {
                await Task.WhenAll(_activeTasks);
            }
            catch (InvalidObjectException exception)
            {
                _notificationService.ShowError("Invalid object", exception);
            }
            catch (InternalException)
            {
                _notificationService.ShowError(
                    "Invalid object",
                    "A problem in the Revit code. Usually occurs when a managed API object is no longer valid and is unloaded from memory");
            }
            catch (SEHException)
            {
                _notificationService.ShowError(
                    "Revit API internal error",
                    "A problem in the Revit code. Usually occurs when a managed API object is no longer valid and is unloaded from memory");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "RevitLookup new instance startup error");
                _notificationService.ShowError("Lookup engine error", exception);
            }
            finally
            {
                ShowHost(false);
                _navigationService.Navigate(typeof(T));
            }
        }

        public async void RunService<T>(Action<T> handler) where T : class
        {
            try
            {
                await Task.WhenAll(_activeTasks);
            }
            catch
            {
                // ignored
            }
            finally
            {
                var service = _scope.ServiceProvider.GetRequiredService<T>();
                handler.Invoke(service);
            }
        }

        private void ShowHost(bool modal)
        {
            if (_parentProvider is null)
            {
                _host.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                var parentHost = _parentProvider.GetRequiredService<IWindowIntercomService>().GetHost();

                _host.WindowStartupLocation = WindowStartupLocation.Manual;
                _host.Left = parentHost.Left + 47;
                _host.Top = parentHost.Top + 49;
            }

            if (modal)
            {
                _host.ShowDialog();
            }
            else
            {
                _host.Show(Context.UiApplication.MainWindowHandle);
            }
        }
    }
}