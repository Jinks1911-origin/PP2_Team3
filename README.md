# ペアプログラミング2 : チーム4

## 課題テーマ

### 【バッググラウンド】

 　タクシーを４，５台運用する小規模タクシー会社を想定した、タクシーの運行管理システム（TMS:Taxi Management System）を開発する。まず、配車センターのオペレーターは顧客からの予約電話を受けて、それを画面上に登録する（以後、JOB）。
 オペレータは手動でJOBを登録し、Idle状態のタクシーを割り当てることができる。割当可能なタクシーがなければ、JOB作成のみを行い、キューイングする。
 タクシー（スタブ）は、ポーリングで自身のJOBを監視し、JOB割当後は手動で進捗を報告する。今回はタクシーの空車営業を想定しない。

### 【機能と役割】
1. オペレータ画面：フロントエンド（Java Script）
  - 電話から受けた予約情報を登録する
  - タクシーを手配する
  - ジョブの進捗の管理、履歴を確認できる
  - タクシーの現在状態を確認できる
2. APIサーバー：バックエンド（ASP.NET Web API）
  - オペレータ画面からに応答する
  - タクシーアプリに応答する
  - 各種状態をDBで管理する
3. タクシーアプリ：スタブ（C#コンソール）
  - 自身の状態を切り替え、自身の現在状態をAPIサーバーに送信する
  - APIサーバーから受け取ったジョブ情報を表示する

### 【担当者】
- ディネス : フロントエンド、ワイヤーフレーム
- 越智孝 : バックエンド、スタブ、データ・API定義

## シーケンス図

```mermaid
sequenceDiagram
    actor Customer as 顧客
    actor Operator as オペレータ

    participant JobReg as JOB登録画面
    participant JobList as JOB一覧画面(Index.html)
    participant History as 履歴画面
    participant TaxiStatus as タクシー状態確認画面
    participant API as APIサーバー
    participant DB as DB
    participant TaxiApp as タクシーアプリ(スタブ)

    Customer->>Operator: 予約電話

    Operator->>JobReg: 予約登録画面を開く
    JobReg->>API: 割当可能タクシー一覧取得
    API->>DB: 待機中タクシー検索
    DB-->>API: タクシー一覧(空リストあり)
    API-->>JobReg: タクシー一覧

    Operator->>JobReg: 予約情報入力・タクシー選択
    JobReg->>API: ジョブ作成要求

    alt タクシー選択あり
        API->>DB: ジョブ登録(ON・割当済み)
        API->>DB: タクシー状態更新(割当済み)
    else タクシー未選択
        API->>DB: ジョブ登録(ON・未割当)
    end

    DB-->>API: 登録完了
    API-->>JobReg: 作成結果

    Operator->>JobList: ONジョブ確認
    JobList->>API: ONジョブ一覧取得
    API->>DB: ONジョブのみ取得
    DB-->>API: ONジョブ一覧
    API-->>JobList: ONジョブ一覧

    alt JOBキャンセル
        Operator->>JobList: キャンセル操作
        JobList->>API: JOBキャンセル要求
        API->>DB: JOBをOFFへ更新
        API->>DB: タクシー割当解除
        API-->>JobList: キャンセル結果
    else JOBアボート
        Operator->>JobList: アボート操作
        JobList->>API: JOBアボート要求
        API->>DB: JOBをOFFへ更新
        API->>DB: タクシー割当解除
        API-->>JobList: アボート結果
    else 再アサイン
        Operator->>JobList: 再アサイン操作
        JobList->>API: 割当可能タクシー一覧取得
        API->>DB: 待機中タクシー検索
        DB-->>API: タクシー一覧
        API-->>JobList: タクシー一覧

        JobList->>API: 再アサイン要求
        API->>DB: 対象JOBのタクシー状態確認

        alt TaxiStatus = NotAssigned
            API->>DB: JOBにタクシーを再割当
            API->>DB: タクシー状態更新
            API-->>JobList: 再アサイン成功
        else TaxiStatus != NotAssigned
            API-->>JobList: 再アサイン不可
        end
    end

    loop タクシー定期監視
        TaxiApp->>API: タクシー状態送信・ジョブ確認
        API->>DB: タクシー状態更新
        API->>DB: 割当ジョブ取得
        DB-->>API: ジョブ情報
        API-->>TaxiApp: ジョブ情報
    end

    TaxiApp->>API: ジョブ進捗報告
    API->>DB: タクシー状態更新
    API->>DB: JOBステータス更新
    DB-->>API: 更新完了
    API-->>TaxiApp: 更新結果

    Operator->>History: 履歴確認
    History->>API: 全JOB履歴取得
    API->>DB: ON/OFF問わず全JOB取得
    DB-->>API: 全JOB一覧
    API-->>History: 全JOB履歴
```

## 状態遷移図

### JOB状態
``` mermaid
stateDiagram-v2
    [*] --> Queued

    Queued --> Waiting : タクシー割当
    Queued --> Canceled : キャンセル操作

    Waiting --> Active : 乗車
    Waiting --> Canceled : キャンセル操作

    Active --> Completed : 降車
    Active --> Aborting : 途中下車

    Aborting --> Aborted : 途中下車完了

    Completed --> [*]
    Canceled --> [*]
    Aborted --> [*]
```

### タクシー状態
``` mermaid
stateDiagram-v2
    [*] --> OffDuty

    OffDuty --> Idle : 勤務開始

    Idle --> Reserved : 配車
    Idle --> OffDuty : 勤務終了

    Reserved --> Occupied : 乗車
    Reserved --> Idle : JOB取消

    Occupied --> Idle : 降車
```

### テーブル定義

1. jobs：JOB

|物理カラム名|データ型|制約|備考|
|---|---|---|---|
|id|INT|PK,IDENTITY|自動採番|
|job_status_id|INT|NOT NULL, FK(job_status.id)|JOBの現在状態|
|taxi_id|INT|FK(taxis.id)|割当済みのタクシー|
|from_loc|NVARCHAR(20)|NOT NULL|乗車地|
|to_loc|NVARCHAR(20)|NOT NULL|降車地|

2. taxis：タクシー

|物理カラム名|データ型|制約|備考|
|---|---|---|---|
|id|INT|PK,IDENTITY|自動採番|
|taxi_status_id|INT|NOT NULL, FK(taxi_status.id)|タクシーの現在状態|
|driver_name|NVARCHAR(20)|NOT NULL|運転手の名前|

3. job_status：JOBの状態

|物理カラム名|データ型|制約|備考|
|---|---|---|---|
|id|INT|PK,IDENTITY|自動採番|
|status_name|NVARCHAR(10)|NOT NULL, UNIQUE|ステータス名|

4. taxi_status：タクシーの状態

|物理カラム名|データ型|制約|備考|
|---|---|---|---|
|id|INT|PK,IDENTITY|自動採番|
|status_name|NVARCHAR(10)|NOT NULL, UNIQUE|ステータス名|

### ロギング設計

1. システムログ

|ログレベル|出力タイミング|メッセージ例|
|---|---|---|
|INFO|アプリ起動/終了|アプリケーション起動|
|WARN|制約付き実行|localhostで起動|
|ERROR|DB接続エラー、通信確立失敗|通信確立失敗：アクセスを拒否されました|
|FITAL|異常終了|サーバーを起動できませんでした|

2. JOB状態遷移

|ログレベル|出力タイミング|メッセージ例|
|---|---|---|
|INFO|状態遷移時|J001:QUEUED->WAITING:TX001|
|WARN|状態遷移不可|J001:WAITING->COMPLETED:INVALID_TRANSITION|

3. タクシー状態遷移ログ

|ログレベル|出力タイミング|メッセージ例|
|---|---|---|
|INFO|状態遷移時|TX001:IDLE->RESERVED:J001|
|WARN|状態遷移不可|TX001:RESERVED->OFFDUTY:INVALID_TRANSITION|

4. オペレーションログ

|ログレベル|出力タイミング|メッセージ例|
|---|---|---|
|INFO|通常操作|J001 REGISTERED|
|WARN|異常操作|J001 NOT_REGISTERED:INVALID_INPUT|

5. DB操作ログ

|ログレベル|出力タイミング|メッセージ例|
|---|---|---|
|INFO|正常な操作|[成功]JOB一覧取得|
|ERROR|異常操作|[失敗]JOB一覧取得:SQL_ERROR|

6. 通信ログ

|ログレベル|出力タイミング|メッセージ例|
|---|---|---|
|INFO|ステータス200系|[200]192.168.10.24 POST /api/register|
|WARN|400系エラー|[404]192.168.10.24 GET /api/jobs/44:JOB_NOT_FOUND|
|ERROR|500系エラー|[500]192.168.10.24 GET /api/jobs/44:DB_ACCESS_FAILED|

## 使用するパッケージ・ライブラリ

### フロントエンド: HTML/CSS + JavaScript
- Bootstrap：HTMLフレームワーク

### バックエンド: ASP.NET Core Web API (C# / .NET 10)
- NLog, NLog.Web.AspNetCore , NLog.Schema：ロギングアプリ
- Microsoft.Data.SqlClient：SQL Server用プロバイダ
- Dapper：SQL拡張ライブラリ
- Scalar.AspNetCore：APIテスト用画面

### タクシースタブ: C# / .NET 10 コンソールアプリケーション
- 追加パッケージなし

## aaaaa

aaaa
