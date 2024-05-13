using Common.Server;
using Maple.Db;
using Maple.Eps;
using Maple.I18n;

Server.Run<MapleDb>(args, S.Inst, MapleEps.Eps, OrgEps.AddServices, OrgEps.InitApp);
