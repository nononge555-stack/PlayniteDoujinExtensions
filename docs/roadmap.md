# Roadmap

## Phase 0 - Repository / Design

- [x] プロジェクトコンセプト整理
- [x] Core / Playnite / Storeの責務分離
- [x] セーブ安全要件の明文化
- [x] RPG Maker初期対応範囲の決定
- [x] upstream候補の特定: `erri120/Playnite.Extensions`
- [x] upstreamライセンス確認: GPL-3.0
- [x] DLsite/FANZA既存構成調査
- [x] upstream-first方針の決定
- [ ] リポジトリへGPL-3.0ライセンス本文・attributionを追加
- [ ] CI作成

## Phase 1 - Existing Extension Modernization

目標: 既存のDLsite/FANZA Playnite Metadata Providerを、現行環境でビルド・利用できる状態へ戻す。

### 1.1 Import / Build Baseline

- [ ] upstream基準commitを決めて記録
- [ ] `Extensions.Common` の必要部分を移植
- [ ] `DLSiteMetadata` を移植
- [ ] `DLSiteMetadata.Test` を移植
- [ ] `FanzaMetadata` を移植
- [ ] `FanzaMetadata.Test` を移植
- [ ] 既存著作権表示・GPL情報を保持
- [ ] Solution / Project referencesを再構成
- [ ] 現行Playnite SDKへ合わせてビルド修正
- [ ] `extension.yaml` を現行要件へ合わせる
- [ ] CIでbuild/test可能にする

### 1.2 DLsite Runtime Verification

- [ ] Playniteへ拡張をロード
- [ ] DLsite URL認識
- [ ] RJ等の商品ID認識
- [ ] 検索
- [ ] タイトル取得
- [ ] 開発者/サークル関連情報取得
- [ ] タグ/ジャンル/カテゴリ取得
- [ ] 発売日取得
- [ ] Icon / Cover / Background取得
- [ ] 言語設定確認
- [ ] 既存テスト修正/追加
- [ ] サイト仕様変更によるScraper破損を必要最小限修正

### 1.3 FANZA Runtime Verification

- [ ] Playniteへ拡張をロード
- [ ] FANZA URL認識
- [ ] 検索
- [ ] タイトル取得
- [ ] 開発者情報取得
- [ ] ジャンル/タグ取得
- [ ] Community Score取得
- [ ] Icon / Cover / Background取得
- [ ] Series取得
- [ ] Release Date取得
- [ ] 既存テスト修正/追加
- [ ] サイト仕様変更によるScraper破損を必要最小限修正

### Phase 1 Exit Criteria

- DLsite/FANZA両拡張が現行Playniteでロードできる
- 最低限のMetadata取得が実サイトで成功する
- build/testがCIで再現可能
- upstreamとの差分が追跡できる

## Phase 2 - DoujinTools MVP

目標: Playniteに既に登録されているツクールゲームのセーブを手動管理できる。

- [ ] `Doujin.Core` 作成
- [ ] Engine model作成
- [ ] RPG Maker 2000 detector
- [ ] RPG Maker 2003 detector
- [ ] RPG Maker XP detector
- [ ] RPG Maker VX detector
- [ ] RPG Maker VX Ace detector
- [ ] RPG Maker MV detector
- [ ] RPG Maker MZ detector
- [ ] Save Locator
- [ ] Backup service
- [ ] Restore service
- [ ] Manifest
- [ ] Playnite Generic Plugin作成
- [ ] `Backup Save` メニュー
- [ ] `Restore Save` メニュー
- [ ] ゲーム単位のEngine/Save Path上書き
- [ ] Unit tests

## Phase 3 - Automatic Backup / Archive

目標: プレイ終了後のセーブ保護と、安全なゲーム本体削除を実現する。

- [ ] Playnite game stoppedイベント連携
- [ ] 変更検出
- [ ] 自動バックアップ
- [ ] SHA-256等による検証
- [ ] バックアップ世代管理
- [ ] Backup History UI
- [ ] Archive manifest
- [ ] upstream `GameManagement` の削除処理を参考にPlaynite連携を実装
- [ ] `Archive & Remove`
- [ ] 削除前確認UI
- [ ] バックアップ失敗時に削除されないことのテスト
- [ ] 復元前の既存セーブ保護

## Phase 4 - Store Library Integration

目標: Metadata Providerとは別に、購入済み作品をPlayniteライブラリへ取り込めるようにする。

### DLsite

- [ ] 現行の購入履歴/ライブラリ取得方法調査
- [ ] account/session方針
- [ ] Library Plugin設計
- [ ] 購入作品同期
- [ ] RJ番号管理
- [ ] ローカルインストールとの紐付け

### FANZA / DMM

- [ ] 現行の購入履歴取得方法・利用条件調査
- [ ] account/session方針
- [ ] Library Plugin設計
- [ ] 購入作品同期
- [ ] 商品ID管理
- [ ] ローカルインストールとの紐付け

Metadata Providerの復旧を購入履歴同期の完成待ちにしない。

## Phase 5 - Install Lifecycle

目標: アーカイブ済みゲームを再び遊ぶまでの手順を短縮する。

- [ ] Storeからの再取得支援
- [ ] 展開/インストール管理
- [ ] Engine再検出
- [ ] 保存済みセーブの存在通知
- [ ] セーブ自動復元
- [ ] RTP / ランタイム支援

## Future

候補であり、初期実装を複雑にしない。

- WOLF RPG Editor
- NScripter
- KiriKiri / KAG
- TyranoScript
- Ren'Py
- Unity系ゲームの限定的なSave Locator
- RPG Developer Bakin
- バックアップ保存先のクラウド同期支援
- 複数PC間でのセーブ管理
- ゲームバージョン/アップデートとセーブスナップショットの関連付け
