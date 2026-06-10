using TaxiManagementSystem.API.Model;

namespace TaxiManagementSystem.API.Repository;

public interface IRepository
{
    // 実行中JOBの一覧を取得する
    IAsyncEnumerable<Job> GetActiveJobsAsync();

    // JOB登録
    Task CreateNewJobAsync(RegisterJob registerJob);

    // 実行中JOB個数取得
    Task<int> GetActiveJobsCountAsync();

    // タクシー再割当
    Task ReassignTaxiAsync(string jobId, string taxiId);

    // JOB中断
    Task CancelJobAsync(string jobId);

    // JOB中断
    Task AbortJobAsync(string abortId);

    // 運行履歴取得
    IAsyncEnumerable<Job> GetHistoryAsync(HistoryFilter filter);

    // 本日完了済みJOBの個数取得
    Task<int> GetTodayHistoryCountAsync();

    // タクシー一覧取得
    IAsyncEnumerable<(Taxi Taxi,string JobId)> GetTaxisAsync();

    // タクシーの台数取得
    Task<int> GetTaxisCountAsync();

    // 割当可能なタクシー一覧取得(stringリスト)
    IAsyncEnumerable<string> GetAvilableTaxisAsync();

    // 割当可能なタクシーの台数取得
    Task<int> GetAvilableTaxisCountAsync();

    // タクシーの情報取得
    Task<(Taxi CurrentTaxi,Job CurrentJob)> GetCurrentTaxiInfoAsync(string id);

    // タクシー状態の更新
    Task SetCurrentTaxiStatusAsync(string id, string status);

    // JOB IDの存在確認
    Task<bool> AnyJobAsync(string id);

    // タクシーIDの存在確認
    Task<bool> AnyTaxiAsync(string id);

}
