namespace TaxiManagementSystem.API.Model;

// JOB登録
public record RegisterJob(string FromLoc, string ToLoc, string? TaxiId);

// タクシー再割当
public record ReassignTaxi(string TaxiId);