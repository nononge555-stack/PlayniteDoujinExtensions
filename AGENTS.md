# AGENTS.md

## Project Goal

Playnite向けの同人ゲーム管理拡張群を開発する。

中心思想は「ゲーム本体を削除しても、ユーザー固有のセーブデータとライブラリ情報は安全に保持する」こと。

## Upstream-First Rule

DLsite/FANZA Metadata Providerはゼロから再実装しない。

Primary upstreamはGPL-3.0の `erri120/Playnite.Extensions`。

優先して調査・移植する対象:

- `src/Extensions.Common`
- `src/DLSiteMetadata`
- `src/DLSiteMetadata.Test`
- `src/FanzaMetadata`
- `src/FanzaMetadata.Test`
- `src/GameManagement` はArchive削除処理の参考対象

既存コードで解決済みのPlaynite integration、Metadata mapping、Settings、XAML、検索処理等を理由なく作り直さない。

初回移植では不要な大規模リネーム・全面リファクタリングを避け、upstreamとの差分を追跡しやすくする。

サイト仕様変更で壊れているScraper等は必要最小限の修正から始める。

## Licensing / Attribution

upstreamコードを移植・改変する場合はGPL-3.0条件を守る。

- 既存の著作権表示を削除しない。
- ライセンス表示を削除しない。
- upstream由来のファイルを追跡可能にする。
- 可能な範囲で元パスと基準commitを記録する。
- ライセンス上不明点がある場合、コードを無断で再ライセンスしない。

## Architecture Rules

- Playnite本体はforkしない。Playnite Extensionsとして実装する。
- `Doujin.Core` は可能な限りPlaynite SDKに依存させない。
- 販売サイト固有処理とゲームエンジン固有処理を分離する。
- DLsite/FANZA等の障害・仕様変更でセーブ管理機能が壊れない構造にする。
- Metadata Providerと購入済みLibrary synchronizationを別責務として扱う。
- Engine DetectorとSave Locatorは追加可能な小さなコンポーネントとして実装する。
- UIからファイル操作ロジックを分離する。

## Save Data Safety

セーブデータの損失は最重要の失敗として扱う。

`Archive & Remove` では必ず以下の順序を守る。

1. セーブデータを検出する。
2. バックアップ先へコピーする。
3. コピー先の存在とサイズを確認する。
4. 必要に応じてハッシュを計算し検証する。
5. manifestを正常に書き込む。
6. 上記すべてに成功した場合のみ削除操作を許可する。

バックアップ失敗、検証失敗、例外、対象セーブ位置が不明な場合はゲーム本体を自動削除しない。

削除処理とバックアップ処理を単一の危険な不可逆操作にしない。

upstream `GameManagement` のUninstall処理を流用・参考にする場合も、この安全条件を必ず上位に置く。

## Current Development Priority

### Phase 1: Existing Extension Modernization

最優先は以下。

- upstream基準commitの記録
- `Extensions.Common` の必要部分移植
- `DLSiteMetadata` / tests移植
- `FanzaMetadata` / tests移植
- 現行Playnite SDK / manifestへ更新
- build/test復旧
- Playnite上でロード確認
- DLsite/FANZAの実サイトMetadata取得確認
- 壊れたScraperの必要最小限修正

Phase 1完了前に購入履歴同期やダウンロード自動化へ先走らない。

### Phase 2: DoujinTools MVP

Phase 1後に以下を優先する。

- RPG Maker 2000/2003/XP/VX/VX Ace/MV/MZの判定
- セーブ位置・ファイルパターンの判定
- 手動バックアップ
- 手動復元
- Playnite Generic Pluginからの操作
- Coreロジックのユニットテスト

以下は初期対象外。

- セーブファイル内部の解析・編集
- 非ツクールゲームの汎用セーブ自動検出
- DLsite/FANZAからの自動ダウンロード
- ゲーム本体のバイナリ解析を必要とする高度な判定

## Coding Guidelines

- C#を使用する。
- Playniteプラグイン部分は実装時点の公式テンプレート/SDK要件に合わせる。
- SDKバージョンを推測で固定せず、移植着手時に現行公式要件を確認する。
- CoreはPlaynite固有型を公開APIに漏らさない。
- ファイルシステム操作はテスト可能な境界を持たせる。
- 1クラス1責務を優先する。
- 巨大なswitchにエンジン固有処理を集約せず、Detector/Locator単位に分ける。
- 不要な抽象化は避ける。将来対応予定だけを理由に複雑な仕組みを先行実装しない。
- upstream移植時は「動作復旧」と「設計刷新」を同一PRで混ぜすぎない。

## Testing

Phase 1では既存DLsite/FANZAテストを可能な限り復旧・再利用する。

DoujinToolsでは特に以下をテストする。

- 各RPG Makerバージョンの正常判定
- 複数候補が存在するときの判定優先順位
- セーブが存在しないゲーム
- セーブが複数存在するゲーム
- 保存先がユーザー指定で上書きされたゲーム
- バックアップ途中失敗
- コピー後の検証失敗
- 復元先に既存セーブが存在する場合
- Archive処理がバックアップ失敗時に削除へ進まないこと

実ゲームデータや購入コンテンツをテストリポジトリへコミットしない。テスト用の最小ダミーファイルを使用する。

## Git Workflow

- 安定ブランチ: `main`
- 日常統合ブランチ: `develop`
- 作業ブランチ: `feature/<name>`
- featureブランチは最新developから作成する。
- 実装完了後developへPRする。
- developへ統合済みのfeatureブランチは削除する。
- リリース可能なまとまりでdevelopからmainへ統合する。

## Documentation

設計判断を変更した場合は対応する `docs/` ファイルも更新する。

upstream移植方針は `docs/upstream-migration.md` を参照する。

セーブデータ削除・復元に関わる仕様変更は、コードだけでなく `docs/save-management.md` に安全条件を明記する。
