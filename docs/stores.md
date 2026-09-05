# Store Integrations

## Principle

販売サイト連携はセーブ管理から独立させる。

DLsite/FANZA/DMMの仕様変更、ログイン失敗、ネットワーク障害があっても、ローカルに登録済みのゲームに対するEngine Detection、Save Backup、Restoreは利用可能であること。

また、DLsite/FANZAのMetadata Providerはゼロから再実装せず、まず既存GPL-3.0実装を現行環境へ移植・近代化する。

Primary upstream:

- `erri120/Playnite.Extensions`
- `src/DLSiteMetadata`
- `src/DLSiteMetadata.Test`
- `src/FanzaMetadata`
- `src/FanzaMetadata.Test`
- `src/Extensions.Common`

## DLsite

### First Goal: Restore Existing Metadata Provider

upstreamの既存機能を維持したまま現行Playnite・現行DLsiteで動作させる。

既存実装が扱う主な項目:

- DLsite URL / RJ等の商品ID
- 検索
- タイトル
- カテゴリ
- ジャンル / タグ
- 開発者関連情報
- 発売日
- Icon
- Cover / Background
- 言語設定
- Metadata field mapping

初回移植ではScraper/API部分以外を理由なく全面書き直さない。

サイト仕様変更で壊れている部分のみ、Playnite mapping層から分離しつつ修正する。

### Later: Library Integration

Metadata Provider復旧後の別機能として検討する。

- 購入済み作品のライブラリ同期
- RJ番号等の作品識別子の安定した保持
- Store account/session handling
- ローカルインストールとの紐付け
- 将来的な再ダウンロード支援

購入履歴同期をMetadata Provider復旧の必須条件にしない。

## FANZA / DMM

### First Goal: Restore Existing Metadata Provider

upstreamの `FanzaMetadata` をベースに近代化する。

既存実装が扱う主な項目:

- FANZA URL
- 検索
- タイトル
- 開発者
- ジャンル / タグ
- Community Score
- Icon
- Cover / Background
- Series
- Release Date

DLsiteと同じく、初回移植では既存構造を保ち、壊れている取得処理を必要最小限修正する。

### Later: Library Integration

- 購入済み作品のライブラリ同期
- 商品ID管理
- account/session handling
- ローカルインストールとの紐付け
- 将来的な再取得支援

認証・購入履歴取得方法は実装着手時点のサイト仕様と利用規約を確認して設計する。

## Metadata vs Library

Metadata ProviderとLibrary synchronizationは別責務とする。

```text
Metadata Provider
  Store page / ID
       -> metadata
       -> existing Playnite game

Library Integration
  Store account / purchase history
       -> owned titles
       -> Playnite library import/update
```

前者はupstream資産を近代化する。
後者は必要に応じてこのプロジェクトで新規実装する。

## Local Games

販売サイト連携を使わないゲームも第一級の対象とする。

Playniteに手動登録されたゲームでもDoujinToolsから以下を利用できること。

- Engine Detection
- Save Backup
- Save Restore
- Archive & Remove

## Identity

セーブバックアップの主キーに販売サイトの商品IDだけを使わない。

同一作品を複数サイトから購入した場合や、販売終了・ID変更・ローカル作品を考慮し、Playnite Game IDを中心に管理する。

Store IDは作品メタデータとして保持する。

## Licensing

upstreamコードを直接移植・改変するため、GPL-3.0条件を保持する。

移植ファイルについては以下を守る。

- 既存著作権表示を保持する
- ライセンス表示を削除しない
- 可能な範囲で元パス・基準commitを記録する
- upstreamコード由来であることを追跡可能にする

詳細は `upstream-migration.md` を参照する。
