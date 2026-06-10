namespace TaxiManagementSystem.API.Model;

// 実行中のJOB
public record ActiveJob(
    string JobId, string Status, string FromLoc, string ToLoc, string? TaxiId, string? DriverName);

// 実行中のJOB件数
public record ActiveJobsCount(int Count);

// 完了JOB
public record CompletedJob(
    string JobId, string Status, string FromLoc, string ToLoc, string? TaxiId, string? DriverName, DateTime ClosedAt);

// 本日の完了JOB件数
public record TodayCompletedJobsCount(int Count);

// タクシー一覧
public record ActiveTaxi(string TaxiId, string Status, string DriverName, string JobId, string JobStatus);

// タクシーの台数
public record ActiveTaxisCount(int Count);

// 割当可能タクシー
public record AvilableActiveTaxi(string TaxiId);

// 割当可能タクシー台数
public record AvilableActiveTaxisCount(int Count);

// フィルタリング用クエリ
public record HistoryFilter(
    string? Status, string? TaxiId, string? DriverName,DateTime? From, DateTime? To);