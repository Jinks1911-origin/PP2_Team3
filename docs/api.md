# API仕様書

## API一覧

|Method|URI|Query|Description|
|:---|:---|:---:|:---|
|GET|/api/jobs|-|実行中JOB一覧取得|
|POST|/api/jobs|-|JOB登録|
|GET|/api/jobs/count|-|実行中JOB個数取得|
|PUT|/api/jobs/{id}/reassign|-|タクシー再割当|
|PUT|/api/jobs/{id}/cancel|-|JOBキャンセル|
|PUT|/api/jobs/{id}/abort|-|JOB中断|
|GET|/api/jobs/completed/count|-|本日の完了済みJOBの個数取得|
|GET|/api/jobs/history|○|運行履歴取得|
|GET|/api/taxis|-|タクシー一覧取得|
|GET|/api/taxis/count|-|タクシーの台数取得|
|GET|/api/taxis/available|-|割当可能なタクシー一覧|
|GET|/api/taxis/available/count|-|割当可能なタクシーの台数取得|

## 実行中JOB一覧取得

### Request

``` http
GET /api/jobs
```

### Response

#### 200 OK

``` json
[
  {
    "id": 1,
    "status": "Active",
    "taxiName": "TX002",
    "fromLoc": "新居浜駅",
    "toLoc": "イオンモール新居浜"
  },
  {
    "id": 2,
    "status": "Waiting",
    "taxiName": "TX003",
    "fromLoc": "フレッシュバリュー喜光地",
    "toLoc": "喜光地自治会館"
  },
  {
    "id": 3,
    "status": "Queued",
    "taxiName": "",
    "fromLoc": "新須賀自治会館",
    "toLoc": "フジ新居浜"
  }
]
```

#### 500 INTERNAL SERVER ERROR

``` json
{
  "error": "JOBS_FETCH_FAILED"
}
```

## JOB登録

### Request

``` http
POST /api/jobs
```

#### Request Body

``` json
{
  "fromLoc": "新居浜駅",
  "toLoc": "イオンモール新居浜",
  "taxiId": 3
}
```

### Response

#### 201 Created

なし

#### 400 Bad Request

異常なリクエスト
``` json
{
  "error": "INVALID_REQUEST"
}
```

タクシー割当失敗
``` json
{
  "error": "CANNOT_ASSIGN"
}
```

## 実行中JOB個数取得

### Request

``` http
GET /api/jobs/count
```

### Response

#### 200 OK

``` json
{
  "jobsCount": 12
}
```

#### 500 INTERNAL SERVER ERROR

``` json
{
  "error": "JOBS_COUNT_FETCH_FAILED"
}
```

## タクシー再割当

### Request

``` http
PUT /api/jobs/{id}/assign
```

#### Request Body

``` json
{
  "taxiId": 2
}
```

### Response

#### 200 OK

なし

#### 400 Bad Request

タクシー割当失敗
``` json
{
  "error": "CANNOT_ASSIGN"
}
```

#### 404 Not Found

JobIDが存在しない
``` json
{
  "error": "JOB_NOT_FOUND"
}
```

## JOBキャンセル

### Request

``` http
PUT /api/jobs/{id}/cancel
```

#### Request Body

なし

### Response

#### 200 OK

なし

#### 400 Bad Request

キャンセルできない状態のためキャンセルに失敗
``` json
{
  "error": "JOB_CANCEL_FAILED"
}
```

#### 404 Not Found

JobIDが存在しない
``` json
{
  "error": "JOB_NOT_FOUND"
}
```

## JOB中断

### Request

``` http
PUT /api/jobs/{id}/abort
```

#### Request Body

なし

### Response

#### 200 OK

なし

#### 400 Bad Request

中断できない状態のため中断に失敗
``` json
{
  "error": "JOB_ABORT_FAILED"
}
```

#### 404 Not Found

JobIDが存在しない
``` json
{
  "error": "JOB_NOT_FOUND"
}
```
## 運行履歴取得

### Request

``` http
GET /api/jobs/history
```

#### Query Parameter

なし

### Response

#### 200 OK

``` json
[
  {
    "id": 1,
    "status": "Completed",
    "taxiName": "TX002",
    "fromLoc": "新居浜駅",
    "toLoc": "イオンモール新居浜"
  },
  {
    "id": 2,
    "status": "Canceled",
    "taxiName": "",
    "fromLoc": "フレッシュバリュー喜光地",
    "toLoc": "喜光地自治会館"
  },
  {
    "id": 3,
    "status": "Active",
    "taxiName": "TX001",
    "fromLoc": "新須賀自治会館",
    "toLoc": "フジ新居浜"
  }
]
```

#### 500 INTERNAL SERVER ERROR

``` json
{
  "error": "JOB_HISTORY_FETCH_FAILED"
}
``` 



## タクシー一覧取得

### Request

``` http
GET /api/taxis
```

#### Query Parameter

なし

### Response





## 割当可能なタクシー一覧

### Request

``` http
GET /api/taxis/available
```

#### Query Parameter

なし

### Response

#### 200 OK

割当可能なタクシーが存在するとき
``` json
[
  {
    "taxiName": "TX003"
  },
  {
    "taxiName": "TX005"
  }
]
```

割当可能なタクシーが存在しないとき
``` json
[]
```

#### 500 INTERNAL SERVER ERROR

``` json
{
  "error": "AVAILABLE_TAXIS_FETCH_FAILED"
}
```
