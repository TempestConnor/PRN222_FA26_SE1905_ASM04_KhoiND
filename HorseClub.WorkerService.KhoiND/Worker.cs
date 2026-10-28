using Microsoft.Extensions.Options;
using HorseClub.Services.KhoiND;
using HorseClub.Services.KhoiND.Dtos;

namespace HorseClub.WorkerService.KhoiND
{
    // On every tick, reports horses created or updated since the previous tick.
    public class Worker(IServiceScopeFactory scopes, IOptions<WorkerOptions> opt, ILogger<Worker> log)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
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
        }

        private void LogHorse(string action, HorseDto dto)
            => log.LogInformation("  {Action}: {Name} (breed {Breed}, owner {Owner}) [{Id}]",
                action, dto.Name, dto.Breed ?? "-", dto.OwnerName ?? "-", dto.Id);
    }
}
