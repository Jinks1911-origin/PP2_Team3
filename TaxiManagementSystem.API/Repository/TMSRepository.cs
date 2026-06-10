using TaxiManagementSystem.API.Model;

namespace TaxiManagementSystem.API.Repository;

public class TMSRepository(IConfiguration configuration) : IRepository
{
    // ===== フィールド =====

    // DB接続文字列
    private readonly string _connectionString =
        configuration.GetConnectionString("DefaultConnection") ?? "";



    // ===== パブリックメソッド =====

    // 実行中JOBの一覧を取得する
    public async IAsyncEnumerable <Job> GetActiveJobsAsync()
    {

    }

    // JOB登録
    public async Task CreateNewJobAsync(RegisterJob registerJob)
    {

    }

    // 実行中JOB個数取得
    public async Task<int> GetActiveJobsCountAsync()
    {

    }

    // タクシー再割当
    public async Task ReassignTaxiAsync(string jobId, string taxiId)
    {

    }

    // JOB中断
    public async Task CancelJobAsync(string jobId)
    {

    }

    // JOB中断
    public async Task AbortJobAsync(string abortId)
    {

    }

    // 運行履歴取得
    public async IAsyncEnumerable<Job> GetHistoryAsync(HistoryFilter filter)
    {

    }

    // 本日完了済みJOBの個数取得
    public async Task<int> GetTodayHistoryCountAsync()
    {

    }

    // タクシー一覧取得
    public async IAsyncEnumerable<(Taxi Taxi, string JobId)> GetTaxisAsync()
    {

    }

    // タクシーの台数取得
    public async Task<int> GetTaxisCountAsync()
    {

    }

    // 割当可能なタクシー一覧取得(stringリスト)
    public async IAsyncEnumerable<string> GetAvilableTaxisAsync()
    {

    }

    // 割当可能なタクシーの台数取得
    public async Task<int> GetAvilableTaxisCountAsync()
    {

    }

    // タクシーの情報取得
    public async Task<(Taxi CurrentTaxi, Job CurrentJob)> GetCurrentTaxiInfoAsync(string id)
    {

    }

    // タクシー状態の更新
    public async Task SetCurrentTaxiStatusAsync(string id, string status)
    {

    }


    // JOB IDの存在確認
    public async Task<bool> AnyJobAsync(string id)
    {

    }

    // タクシーIDの存在確認
    public async Task<bool> AnyTaxiAsync(string id)
    {

    }
}
