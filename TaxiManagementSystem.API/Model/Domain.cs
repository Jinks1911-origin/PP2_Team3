namespace TaxiManagementSystem.API.Model;

// JOBデータ
public record Job(string Id, int StatusId, string TaxiId, string FromLoc, string ToLoc, DateTime ClosedAt);

// タクシーデータ
public record Taxi(string Id, int StatusId, string DriverName);

// JOB状態マスター
public record JobStatus(string Id, string Name);

// タクシー状態マスター
public record TaxiStatus(string Id, string Name);
