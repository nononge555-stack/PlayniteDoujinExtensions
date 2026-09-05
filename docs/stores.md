# Store Integrations

## Principle

販売サイト連携はセーブ管理から独立させる。

DLsite/FANZA/DMMの仕様変更、ログイン失敗、ネットワーク障害があっても、ローカルに登録済みのゲームに対するEngine Detection、Save Backup、Restoreは利用可能であること。

## DLsite

将来の責務候補:

- 購入済み作品のライブラリ同期
- RJ番号等の作品識別子
- タイトル
- サークル/ブランド
- 発売日
- タグ/ジャンル
- カバー画像
- 販売ページ情報
- インストール状態との紐付け
- 将来的な再ダウンロード支援

既存のPlaynite向けDLsite拡張やDLsite管理OSSは、ライセンスと現行仕様を確認した上で参考にする。

外部コードを直接流用する場合は、そのライセンス要件を必ず確認する。

## FANZA / DMM

将来の責務候補:

- 購入済み作品のライブラリ同期
- 商品ID
- タイトル
- サークル/ブランド
- 発売日
- ジャンル
- カバー画像
- 販売ページ情報

認証・購入履歴取得方法は実装着手時点のサイト仕様と利用規約を確認して設計する。

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
