# Tests

## Phase 1

最初はupstreamから移植するMetadata Providerの既存テストを可能な限り復旧・再利用する。

対象:

- `DLSiteMetadata.Test`
- `FanzaMetadata.Test`
- 必要に応じてupstream `TestUtils`

テストは以下を分離する。

### Unit Tests

CIで安定して実行可能なもの。

- URL / Product ID parsing
- Metadata mapping
- Scrapper result parsing（fixture利用）
- Settings behavior
- error handling

### Live Verification

実サイトへのアクセスが必要な検証。

- DLsite検索/作品Metadata取得
- FANZA検索/作品Metadata取得
- site HTML/API変更の検出

Live verificationを通常のunit testへ混ぜず、サイト障害やネットワーク障害でCI全体が不安定にならないようにする。

購入済み作品、cookie、ログイン情報、個人アカウント情報をテストリポジトリやCIへ含めない。

## Doujin.Core / DoujinTools

Phase 2以降は実際に購入したゲームデータをリポジトリへ含めず、テスト用ディレクトリ構成と最小ダミーファイルで以下を再現する。

- RPG Maker各世代のエンジン判定
- セーブファイル検出
- セーブなし
- 複数セーブ
- Engine誤判定を避けるケース
- ユーザー指定Save Path
- バックアップ成功
- バックアップ途中失敗
- ハッシュ/サイズ検証失敗
- 復元先に既存セーブがある場合
- Archive処理がバックアップ失敗時に削除へ進まないこと

特にファイル削除を伴うテストでは必ずテスト専用一時ディレクトリを利用し、実ユーザーデータへアクセスしない。
