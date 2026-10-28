using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using HorseClub.Services.KhoiND;
using HorseClub.Services.KhoiND.Dtos;
using HorseClub.Services.KhoiND.Realtime;

namespace HorseClub.WorkerService.KhoiND
{
    // On every tick, reports horses created or updated since the previous tick, and
    // pushes a summary through the hub when "Worker:HubUrl" is configured.
    public class Worker(IServiceScopeFactory scopes, IOptions<WorkerOptions> opt, ILogger<Worker> log)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            HubConnection? hub = null;
            if (!string.IsNullOrWhiteSpace(opt.Value.HubUrl))
            {
                hub = new HubConnectionBuilder().WithUrl(opt.Value.HubUrl).WithAutomaticReconnect().Build();
                await ConnectWithRetryAsync(hub, ct);    // hub host may start after the worker
            }
            else
            {
                log.LogInformation("Worker:HubUrl is empty, running standalone");
            }

            // CreatedAt (SYSUTCDATETIME) and UpdatedAt (DateTime.UtcNow) are stored in UTC.
            var lastCheck = DateTime.UtcNow;
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(Math.Max(1, opt.Value.IntervalSeconds)));
            try
            {
                while (await timer.WaitForNextTickAsync(ct))
                {
                    try
                    {
                        // Taken before the query so rows written during it are picked up next tick.
                        var tickStart = DateTime.UtcNow;

                        using var scope = scopes.CreateScope();          // fresh service + DbContext per tick
                        var svc = scope.ServiceProvider.GetRequiredService<IHorseServiceKhoiND>();
                        var horses = await svc.GetAllAsync();

                        var created = horses.Where(h => h.CreatedAt > lastCheck).ToList();
                        var updated = horses.Where(h => h.CreatedAt <= lastCheck && h.UpdatedAt > lastCheck).ToList();
                        lastCheck = tickStart;

                        log.LogInformation("{Created} new and {Updated} updated horse(s) since last check ({Total} active)",
                            created.Count, updated.Count, horses.Count);
                        foreach (var h in created) LogHorse("New", h.ToDto());
                        foreach (var h in updated) LogHorse("Updated", h.ToDto());

                        if ((created.Count > 0 || updated.Count > 0) && hub is not null
                            && hub.State == HubConnectionState.Connected)
                        {
                            await hub.InvokeAsync(HubContract.Broadcast,
                                $"Worker: {created.Count} new, {updated.Count} updated horse(s)", ct);
                        }
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        log.LogError(ex, "Worker tick failed");          // never kill the loop
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // host is stopping
            }
            finally
            {
                if (hub is not null) await hub.DisposeAsync();
            }
        }

        private void LogHorse(string action, HorseDto dto)
            => log.LogInformation("  {Action}: {Name} (breed {Breed}, owner {Owner}) [{Id}]",
                action, dto.Name, dto.Breed ?? "-", dto.OwnerName ?? "-", dto.Id);

        private async Task ConnectWithRetryAsync(HubConnection hub, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await hub.StartAsync(ct);
                    log.LogInformation("Connected to hub {HubUrl}", opt.Value.HubUrl);
                    return;
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception ex)
                {
                    log.LogWarning(ex, "Hub not reachable, retrying in 5s");
                }

                try { await Task.Delay(TimeSpan.FromSeconds(5), ct); }
                catch (OperationCanceledException) { return; }
            }
        }
    }
}
