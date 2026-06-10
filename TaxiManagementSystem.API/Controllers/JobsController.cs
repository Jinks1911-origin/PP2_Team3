using Microsoft.AspNetCore.Mvc;
using TaxiManagementSystem.API.Notifier;
using TaxiManagementSystem.API.Repository;

namespace TaxiManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController(IRepository repository, EventNotifier notifier) : ControllerBase
{
    // 実行中JOB一覧取得
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken token)
    {


        return Ok();
    }

    // JOB登録
    [HttpPost]
    public async Task<IActionResult> PostCreateJob(CancellationToken token)
    {
        /* タクシー割当拒否 */
        if (false)
        {
            return BadRequest();
        }

        notifier.Publish(); // 変更イベント発火
        return Created();
    }

    // 実行中JOB個数取得
    [HttpGet("count")]
    public async Task<IActionResult> GetTodayActiveJobs(CancellationToken token)
    {

        return Ok();
    }

    // タクシー再割当
    [HttpPut("{id}/reassign")]
    public async Task<IActionResult> PutReassignTaxi(string id, CancellationToken token)
    {
        /* ID不正値 */
        if (false)
        {
            return NotFound();
        }

        /* 割当拒否 */
        if (false)
        {
            return BadRequest();
        }


        notifier.Publish(); // 変更イベント発火
        return NoContent();
    }

    // JOBキャンセル
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> PutCancelJob(string id, CancellationToken token)
    {
        /* ID不正値 */
        if (false)
        {
            return NotFound();
        }

        /* Cancel拒否 */
        if (false)
        {
            return BadRequest();
        }


        notifier.Publish(); // 変更イベント発火
        return NoContent();
    }

    // JOB中断
    [HttpPut("{id}/abort")]
    public async Task<IActionResult> PutAbortJob(string id, CancellationToken token)
    {
        /* ID不正値 */
        if (false)
        {
            return NotFound();
        }

        /* Abort拒否 */
        if (false)
        {
            return BadRequest();
        }

        notifier.Publish(); // 変更イベント発火
        return NoContent();
    }

    // 運行履歴取得
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(CancellationToken token)
    {
        /* クエリ異常 */
        if (false)
        {
            return BadRequest();
        }

        return NoContent();
    }

    // 本日完了済みJOBの個数取得
    [HttpGet("history/count/today")]
    public async Task<IActionResult> GetTodayHistoryCount(CancellationToken token)
    {

        return NoContent();
    }
}
