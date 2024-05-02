using Common.Server;
using Common.Shared;
using Common.Shared.Auth;
using Maple.Api.Counter;
using Maple.Db;
using Microsoft.EntityFrameworkCore;
using Counter = Maple.Api.Counter.Counter;

namespace Maple.Eps;

internal static class CounterEps
{
    private static async Task<Db.Counter> GetCounter(
        IRpcCtx ctx,
        MapleDb db,
        ISession ses,
        Get? req = null
    )
    {
        if (req != null && ses.Id != req.User)
        {
            // getting arbitrary users counter
            var c = await db.Counters.SingleOrDefaultAsync(x => x.User == req.User, ctx.Ctkn);
            ctx.NotFoundIf(c == null, model: new { Name = "Counter" });
            return c.NotNull();
        }
        // getting my counter
        var counter = await db.Counters.SingleOrDefaultAsync(x => x.User == ses.Id, ctx.Ctkn);
        if (counter == null)
        {
            counter = new() { User = ses.Id, Value = 0 };
            await db.AddAsync(counter, ctx.Ctkn);
        }

        return counter;
    }

    public static IReadOnlyList<IEp> Eps { get; } =
        new List<IEp>()
        {
            Ep<Get, Counter>.DbTx<MapleDb>(
                CounterRpcs.Get,
                async (ctx, db, ses, req) =>
                {
                    var counter = await GetCounter(ctx, db, ses, req);
                    return counter.ToApi();
                },
                false
            ),
            Ep<Nothing, Counter>.DbTx<MapleDb>(
                CounterRpcs.Increment,
                async (ctx, db, ses, _) =>
                {
                    var counter = await GetCounter(ctx, db, ses);
                    if (counter.Value < uint.MaxValue)
                    {
                        counter.Value++;
                    }
                    var fcm = ctx.Get<IFcmClient>();
                    var res = counter.ToApi();
                    await fcm.SendTopic(ctx, db, ses, new List<string>() { ses.Id }, res);
                    return res;
                }
            ),
            Ep<Nothing, Counter>.DbTx<MapleDb>(
                CounterRpcs.Decrement,
                async (ctx, db, ses, req) =>
                {
                    var counter = await GetCounter(ctx, db, ses);
                    if (counter.Value > uint.MinValue)
                    {
                        counter.Value--;
                    }
                    var fcm = ctx.Get<IFcmClient>();
                    var res = counter.ToApi();
                    await fcm.SendTopic(ctx, db, ses, new List<string>() { ses.Id }, res);
                    return res;
                }
            )
        };

    public static Task OnAuthActivation(IRpcCtx ctx, MapleDb db, string id, string email) =>
        Task.CompletedTask;

    public static Task OnAuthDelete(IRpcCtx ctx, MapleDb db, ISession ses) =>
        db.Counters.Where(x => x.User == ses.Id).ExecuteDeleteAsync(ctx.Ctkn);

    public static Task AuthValidateFcmTopic(
        IRpcCtx ctx,
        MapleDb db,
        ISession ses,
        IReadOnlyList<string> topic
    )
    {
        ctx.BadRequestIf(topic.Count != 1);
        return Task.CompletedTask;
    }
}
